using Kurs7PM.Авторизация;
using Kurs7PM.Бухгалтер;
using System.Windows;
using System.Windows.Input;

namespace Kurs7PM.Администратор
{
    /// <summary>
    /// Логика взаимодействия для MenuAdministrator.xaml
    /// </summary>
    public partial class MenuAdministrator : Window
    {
        public MenuAdministrator()
        {
            InitializeComponent();
        }



        //Позволяет перетаскивать окно
        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //Работа кнопки выключения программы
        private void Exit_with_application(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //Работа кнопки разворачивания программы
        private void Maximized_with_application(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else { this.WindowState = WindowState.Normal; }
        }

        //Работа кнопки сворачивания программы
        private void Minimazed_with_application(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //Кнопка для перехода к другой активности
        private void employees(object sender, RoutedEventArgs e)
        {
            Employee go = new Employee();
            go.Show();
            Close();
        }

        //Кнопка для перехода к другой активности
        private void filials(object sender, RoutedEventArgs e)
        {
            Branch go = new Branch();
            go.Show();
            Close();
        }

        //Кнопка для перехода к другой активности
        private void exit(object sender, RoutedEventArgs e)
        {
            MainWindow go = new MainWindow();
            go.Show();
            Close();
        }

        //Кнопка для перехода к другой активности
        private void admins(object sender, RoutedEventArgs e)
        {
            Administrator go = new Administrator();
            go.Show();
            Close();
        }

        private void Buxgalter(object sender, RoutedEventArgs e)
        {
            Buxgaltery go = new Buxgaltery();
            go.Show();
            Close();
        }

        private void Provider(object sender, RoutedEventArgs e)
        {
            Provider go = new Provider();
            go.Show();
            Close();
        }
    }
}
