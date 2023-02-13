using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Авторизация.Регистрация;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;

namespace Kurs7PM.Клиент
{

    public partial class ShoppingCart : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        ShoppingCartTableAdapter STA = new ShoppingCartTableAdapter();
        ShoppingCartHelpTableAdapter SHTA = new ShoppingCartHelpTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        string medicine;
        public ShoppingCart(string name_medicine)
        {
            InitializeComponent();
            data.ItemsSource = DataSet.ShoppingCart.DefaultView;
            STA.Fill(DataSet.ShoppingCart);
            SHTA.Fill(DataSet.ShoppingCartHelp);

            medicine = name_medicine;

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
            int pomindex = index + 1;

            STA.Fill(DataSet.ShoppingCart);

            string Sql = "select * from dbo.ShoppingCart";
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

            //Добавляем корзине
            int pribavquantity = Int32.Parse(quantity[index]);
            pribavquantity++;

            //Количество товаров
            int quan = Int32.Parse(quantity[index].ToString());

            try
            {
            string quantitycompany = "";
            string podchet = "";
            string Sql6 = "select * from " + medicine + " WHERE Название =" + "'" + names[index] + "';";
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


            
            int podind = Int32.Parse(quantity[index].ToString());
            int pod = Int32.Parse(podchet.ToString()) * pribavquantity;

            //Прибавляем количество
            string Sql4 = "UPDATE dbo.ShoppingCart SET Количество = " + pribavquantity + ", Цена = " + pod + " WHERE Название=" + "'" + names[index] + "';";
            SqlConnection connection4 = new SqlConnection(Kurs7ConnectionString);
            connection4.Open();
            SqlCommand command4 = new SqlCommand();
            command4.CommandText = Sql4;
            command4.Connection = connection4;
            command4.ExecuteNonQuery();
            connection4.Close();

            string Sql3 = "select * from " + medicine;
            SqlConnection connection3 = new SqlConnection(Kurs7ConnectionString);
            connection3.Open();
            SqlCommand command3 = new SqlCommand(Sql3, connection3);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<string> names3 = new List<string>();
            List<string> quantity3 = new List<string>();
            List<string> price3 = new List<string>();
            while (reader3.Read())
            {
                names3.Add(reader3["Название"].ToString());
                quantity3.Add(reader3["Количество"].ToString());
                price3.Add(reader3["Цена"].ToString());
            }
            reader3.Close();
            connection3.Close();

            //  Отнимаем у магазина
            int pribavquantity1 = Int32.Parse(quantity3[index]);
            pribavquantity1--;





                //Если товар есть, то отнимает количество
             if (pribavquantity1 >= 1)
            {
                string Sql1 = "UPDATE " + medicine + " SET Количество = " + pribavquantity1 + " WHERE Название=" + "'" + names3[index] + "';";
                SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
                connection1.Open();
                SqlCommand command1 = new SqlCommand();
                command1.CommandText = Sql1;
                command1.Connection = connection1;
                command1.ExecuteNonQuery();
                connection1.Close();
            }
            }
            catch
            {
                MessageBox.Show("Товар закончился!");
            }

            STA.Fill(DataSet.ShoppingCart);

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
            int pomindex = index + 1;

            //Переменная с таблицей корзины
            string shop = "dbo.ShoppingCart";

            //Вывод таблицы с препаратами
            string Sql = "select * from " + medicine;
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

            //Вывод таблицы с корзиной
            string Sql3 = "select * from " + shop;
            SqlConnection connection3 = new SqlConnection(Kurs7ConnectionString);
            connection3.Open();
            SqlCommand command3 = new SqlCommand(Sql3, connection3);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<string> names3 = new List<string>();
            List<string> quantity3 = new List<string>();
            List<string> price3 = new List<string>();
            while (reader3.Read())
            {
                names3.Add(reader3["Название"].ToString());
                quantity3.Add(reader3["Количество"].ToString());
                price3.Add(reader3["Цена"].ToString());
            }
            reader3.Close();
            connection3.Close();

            //Добавляем магазину
            int pribavquantity = Int32.Parse(quantity[index]);
            pribavquantity++;

            string quantitycompany = "";
            string podchet = "";
            string Sql6 = "select * from " + medicine  + " WHERE Название =" + "'" + names3[index] + "';";
                SqlConnection connection6 = new SqlConnection(Kurs7ConnectionString);
                connection6.Open();
                SqlCommand command6 = new SqlCommand(Sql6, connection6);
                SqlDataReader reader6 = command6.ExecuteReader();
                while (reader6.Read())
                {
                podchet = reader6["Цена"].ToString();
                quantitycompany = reader6["Количество"].ToString();
                }
                reader6.Close();
                connection6.Close();

            //Количество товаров
            int quan = Int32.Parse(quantitycompany.ToString());
            quan++;

            //Прибавляем количество
            string Sql4 = "UPDATE " + medicine + " SET Количество = " + quan + " WHERE Название=" + "'" + names3[index] + "';";
            SqlConnection connection4 = new SqlConnection(Kurs7ConnectionString);
            connection4.Open();
            SqlCommand command4 = new SqlCommand();
            command4.CommandText = Sql4;
            command4.Connection = connection4;
            command4.ExecuteNonQuery();
            connection4.Close();

            int quanminus = Int32.Parse(quantity3[index].ToString());
            quanminus--;
            int pod = Int32.Parse(podchet.ToString()) * quanminus;
            string Sql1 = "UPDATE dbo.ShoppingCart" + " SET Количество = " + quanminus + ", Цена = " + pod + " WHERE Название=" + "'" + names3[index] + "';";
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlCommand command1 = new SqlCommand();
            command1.CommandText = Sql1;
            command1.Connection = connection1;
            command1.ExecuteNonQuery();
            connection1.Close();



                //Если товара нет, то удаляет его
                if (quanminus == 0)
                {
                    string Sql10 = "DELETE  FROM " + shop + " WHERE Название=" + "'" + names3[index] + "';";
                    SqlConnection connection10 = new SqlConnection(Kurs7ConnectionString);
                    connection10.Open();
                    SqlCommand command10 = new SqlCommand(Sql10, connection10);
                    SqlDataReader reader10 = command10.ExecuteReader();
                    reader10.Close();
                    connection10.Close();
                }

            STA.Fill(DataSet.ShoppingCart);

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
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Close();
            connection1.Close();
            STA.Fill(DataSet.ShoppingCart);
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
