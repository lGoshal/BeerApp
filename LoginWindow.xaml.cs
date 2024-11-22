using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace BeerApp
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private BeerRepository _repository;

        public LoginWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PivchanskiyConnection"].ConnectionString;
            _repository = new BeerRepository(connectionString);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordBox.Password;

            if (AuthenticateUser(username, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверные учетные данные.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // Реализуйте логику проверки учетных данных
            return true; // Для теста возвращаем true
        }
    }
}
