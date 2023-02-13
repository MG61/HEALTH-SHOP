using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Авторизация;
using Kurs7PM.Администратор;

namespace Kurs7PM.Клиент
{
    public partial class Store : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        ShoppingCartTableAdapter STA = new ShoppingCartTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public Store()
        {
            InitializeComponent();
            STA.Fill(DataSet.ShoppingCart);


            string Sql = "select * from dbo.Branch";
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> names = new List<string>();
            while (reader.Read())
            {
                comboboxsklad.Items.Add(reader["Name"].ToString());
            }
            reader.Close();
            connection.Close();
        }

        string sklad;
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sklad = (sender as ComboBox).SelectedItem as string;

            string Sql = "select * from " + sklad;
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlDataAdapter command = new SqlDataAdapter(Sql, connection);
            DataSet ds = new DataSet();
            command.Fill(ds, sklad);
            connection.Close();
            data.ItemsSource = ds.Tables[sklad].DefaultView; 

        }

        //Добавление товара в корзину
        private void Dob_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int pomindex = index + 1;

            string Sql = "select * from " + sklad;
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> names = new List<string>();
            List<string> quantity = new List<string>();
            List<string> price = new List<string>();
            while (reader.Read())
            {
                names.Add(reader["Название"].ToString());
                quantity.Add(reader["Количество"].ToString());
                price.Add(reader["Цена"].ToString());
            }
            reader.Close();
            connection.Close();

            //  Отнимаем у магазина
            int pribavquantity = Int32.Parse(quantity[index]);
            pribavquantity--;

            //Если товар есть, то отнимает количество
            if (pribavquantity >= 1)
            {
                string Sql1 = "UPDATE " + sklad + " SET Количество = " + pribavquantity + "WHERE Название=" + "'" + names[index] + "';";
                SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
                connection1.Open();
                SqlCommand command1 = new SqlCommand();
                command1.CommandText = Sql1;
                command1.Connection = connection1;
                command1.ExecuteNonQuery();
                connection1.Close();
            }

            //Проверка на уже существующие записи
            string Sql3 = "select * from dbo.ShoppingCart";
            SqlConnection connection3 = new SqlConnection(Kurs7ConnectionString);
            connection3.Open();
            SqlCommand command3 = new SqlCommand(Sql3, connection3);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<string> names3 = new List<string>();
            List<string> quantity3 = new List<string>();
            while (reader3.Read())
            {
                names3.Add(reader3["Название"].ToString());
                quantity3.Add(reader3["Количество"].ToString());
            }
            reader3.Close();
            connection3.Close();

            int temp = 2;
            if (names3.Contains(names[index]))
            {
                temp = 1;
            }

            string quantitycompany = "";
            string podchet = "";
            string Sql6 = "select * from dbo.ShoppingCart" + " WHERE Название =" + "'" + names[index] + "';";
            SqlConnection connection6 = new SqlConnection(Kurs7ConnectionString);
            connection6.Open();
            SqlCommand command6 = new SqlCommand(Sql6, connection6);
            SqlDataReader reader6 = command6.ExecuteReader();
            while (reader6.Read())
            {
                quantitycompany = reader6["Количество"].ToString();
                podchet = reader6["Цена"].ToString();
            }
            reader6.Close();
            connection6.Close();

            if (temp == 1)
            {
                int quan = Int32.Parse(quantitycompany.ToString()); 
                quan++;
                int pod = Int32.Parse(podchet.ToString());
                int podind = Int32.Parse(price[index].ToString());
                pod += podind;
                string Sql1 = "UPDATE dbo.ShoppingCart" + " SET Количество = " + quan + ", Цена = " + pod + " WHERE Название=" + "'" + names[index] + "';";
                SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
                connection1.Open();
                SqlCommand command1 = new SqlCommand();
                command1.CommandText = Sql1;
                command1.Connection = connection1;
                command1.ExecuteNonQuery();
                connection1.Close();
            }
            else if (temp == 2)
            {
                string Sql5 = "INSERT INTO dbo.ShoppingCart" + " (Название, Количество, Цена)" + " VALUES (" + "'" + names[index] + "'" + ", " + 1 + ", " + price[index] + ");";
                SqlConnection connection5 = new SqlConnection(Kurs7ConnectionString);
                connection5.Open();
                SqlCommand command5 = new SqlCommand();
                command5.CommandText = Sql5;
                command5.Connection = connection5;
                command5.ExecuteNonQuery();
                connection5.Close();
            }

            temp = 2;


            //Обновление таблицы
            string Sql9 = "select * from " + sklad;
            SqlConnection connection9 = new SqlConnection(Kurs7ConnectionString);
            connection9.Open();
            SqlDataAdapter command9 = new SqlDataAdapter(Sql9, connection9);
            DataSet ds9 = new DataSet();
            command9.Fill(ds9, sklad);
            connection9.Close();
            data.ItemsSource = ds9.Tables[sklad].DefaultView; ;
        }

        //Переход к окну авторизации
        private void authorization(object sender, RoutedEventArgs e)
        {
            MainWindow go = new MainWindow();
            go.Show();
            Close();
        }

        //Переход к окну корзины
        private void korzina(object sender, RoutedEventArgs e)
        {
            ShoppingCart go = new ShoppingCart(sklad);
            go.Show();
            Close();
        }

        //Отправляет корзину в конец datagrid
        private void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "Корзина")
                {
                    item.DisplayIndex = grid.Columns.Count - 1;
                    break;
                }
            }
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
