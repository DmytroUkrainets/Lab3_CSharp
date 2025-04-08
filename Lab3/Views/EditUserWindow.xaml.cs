using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lab3.Models;

namespace Lab3.Views
{
    public partial class EditUserWindow : Window
    {
        public Person PersonToEdit { get; set; }

        public EditUserWindow(Person person)
        {
            InitializeComponent();
            PersonToEdit = new Person(person.FirstName, person.LastName, person.Email, person.BirthDate);
            this.DataContext = PersonToEdit;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (HasValidationError(this))
            {
                MessageBox.Show("Будь ласка, виправте помилки у введених даних перед збереженням.",
                                "Валідаційна помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private bool HasValidationError(DependencyObject obj)
        {
            if (Validation.GetHasError(obj))
                return true;

            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (HasValidationError(child))
                    return true;
            }
            return false;
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString(), "Валідаційна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
