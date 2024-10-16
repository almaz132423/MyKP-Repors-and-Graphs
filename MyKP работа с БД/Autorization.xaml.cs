using System.Windows;

namespace MyKP_работа_с_БД
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();
            FrameForLogin.Content = new Pages.auto();
        }

        private void newUser_Click(object sender, RoutedEventArgs e)
        {
            FrameForLogin.Content = new Pages.reg();
        }

        private void autoUser_Click(object sender, RoutedEventArgs e)
        {
            FrameForLogin.Content = new Pages.auto();
        }
    }
}
