using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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

namespace Kurs7PM.Авторизация.Регистрация
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {

        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public Client()
        {
            InitializeComponent();
        }

        //Добавление клиента
        private void add_Provider(object sender, RoutedEventArgs e)
        {
            string Sql5 = "INSERT INTO dbo.Client" + " VALUES (" + "'" + login.Text + "'" + ", " + "'" + password.Text + "'" + ", " + "'" + familia.Text + "'" + ", " + "'" + name.Text + "'" + ", " + "'" + middle_name.Text + "');";
            SqlConnection connection5 = new SqlConnection(Kurs7ConnectionString);
            connection5.Open();
            SqlCommand command5 = new SqlCommand();
            command5.CommandText = Sql5;
            command5.Connection = connection5;
            command5.ExecuteNonQuery();
            connection5.Close();
            MessageBox.Show("Вы успешно зарегистрированы!");
            MainWindow go = new MainWindow();
            go.Show();
            Close();
        }


        //Переход к меню
        private void auth(object sender, RoutedEventArgs e)
        {
            MainWindow go = new MainWindow();
            go.Show();
            Close();
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
    }
}
