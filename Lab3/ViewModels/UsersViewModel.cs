using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using Lab3.Exceptions;
using Lab3.Models;
using Lab3.Views;

namespace Lab3.ViewModels
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _people;
        private ICollectionView _peopleView;
        private Person _selectedPerson;
        private string _filterText;
        private bool _sortAscending = true;
        private string[] _sortableProperties = new[]
        {
            "FirstName", "LastName", "Email", "BirthDate",
            "IsAdult", "SunSign", "ChineseSign", "IsBirthday"
        };
        private string _selectedSortProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People
        {
            get => _people;
            set
            {
                _people = value;
                OnPropertyChanged(nameof(People));
            }
        }

        public ICollectionView PeopleView
        {
            get => _peopleView;
            private set
            {
                _peopleView = value;
                OnPropertyChanged(nameof(PeopleView));
            }
        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                PeopleView?.Refresh();
            }
        }

        public IEnumerable<string> SortableProperties => _sortableProperties;

        public string SelectedSortProperty
        {
            get => _selectedSortProperty;
            set
            {
                _selectedSortProperty = value;
                OnPropertyChanged(nameof(SelectedSortProperty));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        public ICommand SaveCommand { get; }

        public UsersViewModel()
        {
            LoadOrGenerateUsers();

            PeopleView = CollectionViewSource.GetDefaultView(People);
            PeopleView.Filter = PeopleFilter;

            AddCommand = new RelayCommand(AddUser);
            EditCommand = new RelayCommand(EditUser, () => SelectedPerson != null);
            DeleteCommand = new RelayCommand(DeleteUser, () => SelectedPerson != null);
            SaveCommand = new RelayCommand(SaveUsers);
            SortAscCommand = new RelayCommand(() => SortUsers(true));
            SortDescCommand = new RelayCommand(() => SortUsers(false));
        }

        private bool PeopleFilter(object item)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            if (item is Person person)
            {
                return person.FirstName.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.LastName.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.BirthDate.ToShortDateString().Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.IsAdult.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.SunSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.ChineseSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || person.IsBirthday.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private void LoadOrGenerateUsers()
        {
            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                var loaded = JsonSerializer.Deserialize<ObservableCollection<Person>>(json);
                People = loaded ?? new ObservableCollection<Person>();
            }
            else
            {
                People = new ObservableCollection<Person>(MockDataGenerator.GeneratePeople(50));
            }
        }

        private void SaveUsers()
        {
            string json = JsonSerializer.Serialize(People);
            File.WriteAllText("users.json", json);
        }

        private void SortUsers(bool ascending)
        {
            if (string.IsNullOrEmpty(SelectedSortProperty))
                return;

            var sorted = ascending
                ? People.OrderBy(p => p.GetType().GetProperty(SelectedSortProperty)?.GetValue(p, null))
                : People.OrderByDescending(p => p.GetType().GetProperty(SelectedSortProperty)?.GetValue(p, null));

            People = new ObservableCollection<Person>(sorted);
            PeopleView = CollectionViewSource.GetDefaultView(People);
            PeopleView.Filter = PeopleFilter;
        }

        private void AddUser()
        {
            var tempPerson = new Person("Новий", "Користувач", "test@mail.com", DateTime.Now);
            var window = new EditUserWindow(tempPerson);

            if (window.ShowDialog() == true)
            {
                try
                {
                    var editedPerson = window.PersonToEdit;
                    People.Add(editedPerson);
                    SelectedPerson = editedPerson;
                }
                catch (FutureDateOfBirthException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidEmailException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (TooOldDateOfBirthException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditUser()
        {
            if (SelectedPerson == null)
                return;

            var editWindow = new EditUserWindow(SelectedPerson);
            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    SelectedPerson.FirstName = editWindow.PersonToEdit.FirstName;
                    SelectedPerson.LastName = editWindow.PersonToEdit.LastName;
                    SelectedPerson.Email = editWindow.PersonToEdit.Email;
                    SelectedPerson.BirthDate = editWindow.PersonToEdit.BirthDate;

                    OnPropertyChanged(nameof(People));
                    PeopleView.Refresh();
                }
                catch (FutureDateOfBirthException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidEmailException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (TooOldDateOfBirthException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteUser()
        {
            if (SelectedPerson != null)
            {
                People.Remove(SelectedPerson);
                SelectedPerson = null;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
