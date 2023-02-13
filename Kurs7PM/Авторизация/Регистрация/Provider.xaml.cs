using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Администратор;
using System;
using System.Collections.Generic;
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

namespace Kurs7PM.Авторизация.Регистрация
{
    public partial class Provider : Window
    {

        Kurs7DataSet DataSet = new Kurs7DataSet();
        ProviderTableAdapter PTA = new ProviderTableAdapter();
        SkladTableAdapter STA = new SkladTableAdapter();

        public Provider(string sklada)
        {
            InitializeComponent();

            PTA.Fill(DataSet.Provider);
            STA.Fill(DataSet.Sklad);

            if (0 != DataSet.Sklad.Rows.Count)
            {
                sklad.Items.Add(sklada);
            }
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

        //Добавление поставщика
        private void add_Provider(object sender, RoutedEventArgs e)
        {
            PTA.InsertQuery(login.Text, password.Text, familia.Text, name.Text, middle_name.Text, sklad.Text);
            PTA.Fill(DataSet.Provider);
            MessageBox.Show("Вы успешно зарегистрированы!");
            MainWindow go = new MainWindow();
            go.Show();
            Close();
        }

        //Переход
        private void sklad11(object sender, RoutedEventArgs e)
        {
            Sklad go = new Sklad();
            go.Show();
            Close();
        }

    }
}
