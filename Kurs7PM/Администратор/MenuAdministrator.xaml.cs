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
            this.Close();
        }

        //Кнопка для перехода к другой активности
        private void filials(object sender, RoutedEventArgs e)
        {
            Branch go = new Branch();
            go.Show();
            this.Close();
        }

        //Кнопка для перехода к другой активности
        private void exit(object sender, RoutedEventArgs e)
        {

        }

        //Кнопка для перехода к другой активности
        private void admins(object sender, RoutedEventArgs e)
        {
            Administrator go = new Administrator();
            go.Show();
            this.Close();
        }
    }
}
