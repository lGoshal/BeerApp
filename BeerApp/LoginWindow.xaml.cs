using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

namespace BeerApp
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private PivchanskiyEntities1 db;

        public LoginWindow()
        {
            InitializeComponent();
            db = new PivchanskiyEntities1();
        }

        // Кнопка войти
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (ValidateUser(username, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorMessageTextBlock.Text = "Invalid username or password!";
            }
        }

        // Кнопка регистрации
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (username == "" || password == "")
            {
                ErrorMessageTextBlock.Text = "Registration failed!";
            }
            else if (RegisterUser(username, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        // Метод регистрации пользователя
        private bool RegisterUser(string username, string password)
        {
            // Проверяем, существует ли пользователь
            if (db.User.Any(u => u.Login == username))
            {
                ErrorMessageTextBlock.Text = "User already exists!";
                return false;
            }

            // Генерируем соль и хешируем пароль
            byte[] salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(password, salt);

            // Создаем нового пользователя и сохраняем в базе данных
            var newUser = new User
            {
                Login = username,
                Password = password,
                PasswordHash = hashedPassword,
                CustomerID = 1,
                Salt = Convert.ToBase64String(salt)
            };

            db.User.Add(newUser);
            db.SaveChanges();
            return true;
        }

        // Метод проверки пользователя
        private bool ValidateUser(string username, string password)
        {
            // Ищем пользователя по имени
            var user = db.User.SingleOrDefault(u => u.Login == username);
            if (user == null)
            {
                return false;
            }

            // Получаем соль из базы данных и проверяем пароль
            byte[] storedSalt = Convert.FromBase64String(user.Salt);
            return PasswordHelper.VerifyPassword(password, user.PasswordHash, storedSalt);
        }
    }
}
