using Kurs7PM.Kurs7DataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kurs7PM.Клиент
{

    public partial class ShoppingCart : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        ShoppingCartTableAdapter STA = new ShoppingCartTableAdapter();
        ShoppingCartHelpTableAdapter SHTA = new ShoppingCartHelpTableAdapter();

        public ShoppingCart()
        {
            InitializeComponent();
            data.ItemsSource = DataSet.ShoppingCart.DefaultView;
            STA.Fill(DataSet.ShoppingCart);
            SHTA.Fill(DataSet.ShoppingCartHelp);

            //Подсчёт суммы
            int sum = 0;
            foreach(DataRowView row in data.ItemsSource) { 
                sum += (int)row["Цена"];
            }
            summ.Text = sum.ToString();
        }

        //Увеличивает количество товара
        private void plus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());

            string Sql1 = "select Price from dbo.ShoppingCartHelp";
            SqlConnection connection1 = new SqlConnection("Data Source=DESKTOP-1KN5R8D;Initial Catalog=Kurs7;Integrated Security=True");
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            List<string> priceone = new List<string>();
            while (reader1.Read())
            {
                priceone.Add(reader1["Price"].ToString());
            }
            reader1.Close();
            connection1.Close();

            string Sql = "select * from dbo.ShoppingCart";
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1KN5R8D;Initial Catalog=Kurs7;Integrated Security=True");
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> names = new List<string>();
            List<string> quantity = new List<string>();
            List<string> price = new List<string>();
            while (reader.Read())
            {
                names.Add(reader["Name_medication"].ToString());
                quantity.Add(reader["Quatinty_medication"].ToString());
                price.Add(reader["Price"].ToString());
            }
            reader.Close();
            connection.Close();



            int pribavquantity = Int32.Parse(quantity[index]);
            pribavquantity++;
            int pribavprice = Int32.Parse(price[index]);
            int pribavpriceone = Int32.Parse(priceone[index]);
            pribavprice += pribavpriceone;

            if (data.SelectedItem != null)
            {
                int id = index + 1;
                STA.UpdateQuery(names[index], pribavquantity, pribavprice,  id);
                STA.Fill(DataSet.ShoppingCart);
            }

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data.ItemsSource)
            {
                sum += (int)row["Цена"];
            }
            summ.Text = sum.ToString();
        }

        //Уменьшает количество товара
        private void minus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());


            string Sql = "select * from dbo.ShoppingCart";
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1KN5R8D;Initial Catalog=Kurs7;Integrated Security=True");
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> names = new List<string>();
            List<string> quantity = new List<string>();
            List<string> price = new List<string>();
            while (reader.Read())
            {
                names.Add(reader["Name_medication"].ToString());
                quantity.Add(reader["Quatinty_medication"].ToString());
                price.Add(reader["Price"].ToString());
            }
            reader.Close();
            connection.Close();

            string Sql1 = "select Price from dbo.ShoppingCartHelp";
            SqlConnection connection1 = new SqlConnection("Data Source=DESKTOP-1KN5R8D;Initial Catalog=Kurs7;Integrated Security=True");
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            List<string> priceone = new List<string>();
            while (reader1.Read())
            {
                priceone.Add(reader1["Price"].ToString());
            }
            reader1.Close();
            connection1.Close();

            int pribavquantity = Int32.Parse(quantity[index]);
            pribavquantity--;
            int pribavprice = Int32.Parse(price[index]);
            int pribavpriceone = Int32.Parse(priceone[index]);
            pribavprice -= pribavpriceone;


            if (pribavquantity == 0)
            {
                MessageBox.Show("Вы не можете удалить товар!");
            }
            else if(pribavquantity >= 1)
            {
                int id = index + 1;
                STA.UpdateQuery(names[index], pribavquantity, pribavprice, id);
                STA.Fill(DataSet.ShoppingCart);
            }

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data.ItemsSource)
            {
                sum += (int)row["Цена"];
            }
            summ.Text = sum.ToString();
        }

        //Удаляет все данные из корзины
        private void delete_korz(object sender, RoutedEventArgs e)
        {
            string Sql1 = "Truncate table dbo.ShoppingCart";
            SqlConnection connection1 = new SqlConnection("Data Source=DESKTOP-1KN5R8D;Initial Catalog=Kurs7;Integrated Security=True");
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Close();
            connection1.Close();
            string Sql = "Truncate table dbo.ShoppingCartHelp";
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-1KN5R8D;Initial Catalog=Kurs7;Integrated Security=True");
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            connection.Close();
            STA.Fill(DataSet.ShoppingCart);
            SHTA.Fill(DataSet.ShoppingCartHelp);

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data.ItemsSource)
            {
                sum += (int)row["Цена"];
            }
            summ.Text = sum.ToString();
        }

        //Отправляет корзину в конец datagrid
        private void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "+ товар")
                {
                    item.DisplayIndex = grid.Columns.Count - 1;
                    break;
                }
            }
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "- товар")
                {
                    item.DisplayIndex = grid.Columns.Count - 1;
                    break;
                }
            }

        }

        //Убирает первый столбец datagrid (id)
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID_cart")
            {
                e.Cancel = true;
            }
        }

        //Переход к магазину
        private void store(object sender, RoutedEventArgs e)
        {
            Store go = new Store();
            go.Show();
            Close();
        }

        //Переход к чеку
        private void oplata(object sender, RoutedEventArgs e)
        {
            Check go = new Check();
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
