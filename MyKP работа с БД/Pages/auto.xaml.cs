using MyKP_работа_с_БД.DataBase;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyKP_работа_с_БД.Pages
{
    /// <summary>
    /// Логика взаимодействия для auto.xaml
    /// </summary>
    public partial class auto : Page
    {
        public auto()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.Generic.List<Users> result = new DB_Operation().ExecuteQuery<Users>("SELECT * FROM Users;");

            Users users = result.Where(w =>
                w.Login == tbLogin.Text &&
                w.Password == tbPassword.Password)
                .ToList().LastOrDefault();

            if (users != null)
            {
                _ = MessageBox.Show(
                    "Вы успешно авторизовались.",
                    "Успех!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                MainWindow
                    mainWindow = new();
                mainWindow.Show();

                Window
                    window = Window.GetWindow(this);
                window?.Close();
            }
            else
            {
                _ = MessageBox.Show(
                    "Логин или пароль были введены не верно.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
