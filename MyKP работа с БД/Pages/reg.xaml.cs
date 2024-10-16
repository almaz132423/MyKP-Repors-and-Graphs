using MyKP_работа_с_БД.DataBase;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MyKP_работа_с_БД.Pages
{
    /// <summary>
    /// Логика взаимодействия для reg.xaml
    /// </summary>
    public partial class reg : Page
    {
        public reg()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {

            if (tbLogin.Text == "")
            {
                //MessageBox.Show("Введите логин.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                tbLogin.ToolTip = "Введите логин.";
                tbLogin.BorderBrush = Brushes.Red;
            }
            else if (tbEmail.Text == "")
            {
                //MessageBox.Show("Введите эмейл.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                tbEmail.ToolTip = "Введите эмейл.";
                tbEmail.BorderBrush = Brushes.Red;
            }
            else if (tbPassword.Password == "")
            {
                //MessageBox.Show("Введите пароль.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                tbPassword.ToolTip = "Введите пароль.";
                tbPassword.BorderBrush = Brushes.Red;
            }
            else if (tbPassword2.Password == "")
            {
                //MessageBox.Show("Введите пароль для проверки.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                tbPassword2.ToolTip = "Введите пароль для проверки.";
                tbPassword2.BorderBrush = Brushes.Red;
            }
            else if (tbPassword.Password != tbPassword2.Password)
            {
                _ = MessageBox.Show("Пароль должны быть одинаковыми.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                tbPassword.BorderBrush = Brushes.Red;
                tbPassword2.BorderBrush = Brushes.Red;
            }
            else
            {
                string sql = "INSERT INTO Users (Login, Password, Email) VALUES (@log, @pass, @email)";
                SQLiteParameter[] parameters = {
                    new("@log", tbLogin.Text),
                    new("@pass", tbPassword.Password),
                    new("@email", tbEmail.Text)
                };
                new DB_Operation().Query(sql, parameters);

                _ = MessageBox.Show("Вы успешно прошли регистрацию.", "Поздравляем!", MessageBoxButton.OK, MessageBoxImage.Information);
                _ = NavigationService.Navigate(new Pages.auto());
            }
        }
    }
}
