using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Авторизация;
using Kurs7PM.Клиент;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kurs7PM.Сотрудник
{

    public partial class Pharmacy : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        ShoppingCartsTableAdapter STA = new ShoppingCartsTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public Pharmacy(string address)
        {
            InitializeComponent();

            addressap.Text = address;

            string Sql = "select * from " + address;
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlDataAdapter command = new SqlDataAdapter(Sql, connection);
            DataSet ds = new DataSet();
            command.Fill(ds, address);
            connection.Close();

            data.ItemsSource = ds.Tables[address].DefaultView; ;
        }


        //Переход к окну авторизации
        private void authorization(object sender, RoutedEventArgs e)
        {
            MainWindow go = new MainWindow();
            go.Show();
            Close();
        }

        //Переход к окну продажи
        private void Store(object sender, RoutedEventArgs e)
        {
            Store go = new Store(addressap.Text);
            go.Show();
            Close();
        }

        //Переход к окну корзины
        private void provider_buy(object sender, RoutedEventArgs e)
        {
            Zakup go = new Zakup(addressap.Text);
            go.Show();
            Close();
        }

        //Убирает первый столбец id
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID_medication")
            {
                e.Cancel = true;
            }
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
