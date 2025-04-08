using System.Windows;
using Lab3.ViewModels;

namespace Lab3.Views
{
    public partial class UsersWindow : Window
    {
        public UsersWindow()
        {
            InitializeComponent();
            this.DataContext = new UsersViewModel();
        }
    }
}
