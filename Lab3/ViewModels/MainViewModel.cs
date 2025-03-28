using System.ComponentModel;
using Lab3.Models;
using System.Windows.Input;
using System.Windows;
using Lab3.Exceptions;

namespace Lab3.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private DateTime _birthDate = DateTime.Today;
        private Person _person;
        private bool _isProcessing;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); UpdateProceedCommand(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); UpdateProceedCommand(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); UpdateProceedCommand(); }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); UpdateProceedCommand(); }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set { _isProcessing = value; OnPropertyChanged(nameof(IsProcessing)); }
        }

        public string Info => _person != null ? $"Ім'я: {_person.FirstName} {_person.LastName}\nEmail: {_person.Email}\n" +
            $"Дата народження: {_person.BirthDate.ToShortDateString()}\nДорослий: {_person.IsAdult}\n" +
            $"Західний знак: {_person.SunSign}\nКитайський знак: {_person.ChineseSign}\nДень народження: {_person.IsBirthday}" : "";

        public ICommand ProceedCommand { get; }

        public MainViewModel()
        {
            ProceedCommand = new RelayCommand(async () => await ProceedAsync(), () => CanProceed());
        }

        private async Task ProceedAsync()
        {
            IsProcessing = true;
            await Task.Delay(500);

            try
            {
                _person = new Person(FirstName, LastName, Email, BirthDate);
                OnPropertyChanged(nameof(Info));

                if (_person.IsBirthday)
                {
                    MessageBox.Show("Вітаємо з Днем Народження! 🎉", "Свято!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (FutureDateOfBirthException)
            {
                MessageBox.Show("Дата народження не може бути в майбутньому!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TooOldDateOfBirthException)
            {
                MessageBox.Show("Дата народження занадто далека в минулому!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidEmailException)
            {
                MessageBox.Show("Невірний формат електронної пошти!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася невідома помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            IsProcessing = false;
        }

        private bool CanProceed()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(Email) && BirthDate != default;
        }

        private void UpdateProceedCommand()
        {
            (ProceedCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}