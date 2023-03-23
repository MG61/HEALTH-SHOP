using Kurs7PM.Клиент;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kurs7PM.Сотрудник
{
    public partial class Store : Window
    {
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;
        HttpClient client = new HttpClient();
        
        string medicine;
        string carts = "ShoppingCartEmployee";
        public Store(string sklad_priem)
        {
            InitializeComponent();
            medicine = sklad_priem;

            client.BaseAddress = new Uri("https://localhost:7005/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string Sql = "select * from " + medicine;
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlDataAdapter command = new SqlDataAdapter(Sql, connection);
            DataSet ds = new DataSet();
            command.Fill(ds, medicine);
            connection.Close();
            data.ItemsSource = ds.Tables[medicine].DefaultView;

            string Sql1 = "select * from " + carts;
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlDataAdapter command1 = new SqlDataAdapter(Sql1, connection1);
            DataSet ds1 = new DataSet();
            command1.Fill(ds1, carts);
            connection1.Close();
            data1.ItemsSource = ds1.Tables[carts].DefaultView;

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data1.ItemsSource)
            {
                sum += Int32.Parse(row["Цена"].ToString());
            }
            summ.Text = sum.ToString();
        }

        //Добавление товара в корзину
        private void Dob_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int pomindex = index + 1;



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



            //Проверка на уже существующие записи
            string Sql3 = "select * from " + carts;
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
            string Sql6 = "select * from " + carts + " WHERE Название =" + "'" + names[index] + "';";
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


            //  Отнимаем у магазина
            int pribavquantity = Int32.Parse(quantity[index]);
            pribavquantity--;

            if (pribavquantity <= 0)
            {
                MessageBox.Show("Товар закончился!");
            }
            //Если товар есть, то отнимает количество
            else if (pribavquantity > 0)
            {
                string Sql1 = "UPDATE " + medicine + " SET Количество = " + pribavquantity + "WHERE Название=" + "'" + names[index] + "';";
                SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
                connection1.Open();
                SqlCommand command1 = new SqlCommand();
                command1.CommandText = Sql1;
                command1.Connection = connection1;
                command1.ExecuteNonQuery();
                connection1.Close();


                if (temp == 1)
                {
                    int quan = Int32.Parse(quantitycompany.ToString());
                    quan++;
                    int pod = Int32.Parse(podchet.ToString());
                    int podind = Int32.Parse(price[index].ToString());
                    pod += podind;
                    string Sql2 = "UPDATE " + carts + " SET Количество = " + quan + ", Цена = " + pod + " WHERE Название=" + "'" + names[index] + "';";
                    SqlConnection connection2 = new SqlConnection(Kurs7ConnectionString);
                    connection2.Open();
                    SqlCommand command2 = new SqlCommand();
                    command2.CommandText = Sql2;
                    command2.Connection = connection2;
                    command2.ExecuteNonQuery();
                    connection2.Close();
                }
                else if (temp == 2)
                {
                    string Sql5 = "INSERT INTO " + carts + " (Название, Количество, Цена)" + " VALUES (" + "'" + names[index] + "'" + ", " + 1 + ", " + price[index] + ");";
                    SqlConnection connection5 = new SqlConnection(Kurs7ConnectionString);
                    connection5.Open();
                    SqlCommand command5 = new SqlCommand();
                    command5.CommandText = Sql5;
                    command5.Connection = connection5;
                    command5.ExecuteNonQuery();
                    connection5.Close();
                }
            }

            temp = 2;


            //Обновление таблицы
            string Sql9 = "select * from " + medicine;
            SqlConnection connection9 = new SqlConnection(Kurs7ConnectionString);
            connection9.Open();
            SqlDataAdapter command9 = new SqlDataAdapter(Sql9, connection9);
            DataSet ds9 = new DataSet();
            command9.Fill(ds9, medicine);
            connection9.Close();
            data.ItemsSource = ds9.Tables[medicine].DefaultView;

            //Обновление таблицы
            string Sql12 = "select * from " + carts;
            SqlConnection connection12 = new SqlConnection(Kurs7ConnectionString);
            connection12.Open();
            SqlDataAdapter command12 = new SqlDataAdapter(Sql12, connection12);
            DataSet ds12 = new DataSet();
            command12.Fill(ds12, carts);
            connection12.Close();
            data1.ItemsSource = ds12.Tables[carts].DefaultView;

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data1.ItemsSource)
            {
                sum += Int32.Parse(row["Цена"].ToString());
            }
            summ.Text = sum.ToString();
        }

        //Переход к окну авторизации
        private void store(object sender, RoutedEventArgs e)
        {
            Pharmacy go = new Pharmacy(medicine);
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

        //Убирает первый столбец id
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID_medication")
            {
                e.Cancel = true;
            }
            if (e.PropertyName == "ID_cart")
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

        //Увеличивает количество товара
        private void plus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int pomindex = index + 1;

            string Sql1 = "select * from " + carts;
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlDataAdapter command1 = new SqlDataAdapter(Sql1, connection1);
            DataSet ds1 = new DataSet();
            command1.Fill(ds1, carts);
            connection1.Close();
            data1.ItemsSource = ds1.Tables[carts].DefaultView;

            string Sql = "select * from " + carts;
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
                int pribavquantity1 = Int32.Parse(quantitycompany);
                pribavquantity1--;

                //Если товар есть, то отнимает количество
                if (pribavquantity1 <= 0)
                {
                        string Sql10 = "DELETE  FROM " + medicine + " WHERE Название=" + "'" + names3[index] + "';";
                        SqlConnection connection10 = new SqlConnection(Kurs7ConnectionString);
                        connection10.Open();
                        SqlCommand command10 = new SqlCommand(Sql10, connection10);
                        SqlDataReader reader10 = command10.ExecuteReader();
                        reader10.Close();
                        connection10.Close();
                    
                }
                else if (pribavquantity1 > 0)
                {

                    string Sql10 = "UPDATE " + medicine + " SET Количество = " + pribavquantity1 + " WHERE Название=" + "'" + names[index] + "';";
                    SqlConnection connection10 = new SqlConnection(Kurs7ConnectionString);
                    connection10.Open();
                    SqlCommand command10 = new SqlCommand();
                    command10.CommandText = Sql10;
                    command10.Connection = connection10;
                    command10.ExecuteNonQuery();
                    connection10.Close();

                    //Прибавляем количество
                    string Sql4 = "UPDATE " + carts + " SET Количество = " + pribavquantity + ", Цена = " + pod + " WHERE Название=" + "'" + names[index] + "';";
                    SqlConnection connection4 = new SqlConnection(Kurs7ConnectionString);
                    connection4.Open();
                    SqlCommand command4 = new SqlCommand();
                    command4.CommandText = Sql4;
                    command4.Connection = connection4;
                    command4.ExecuteNonQuery();
                    connection4.Close();
                }
            }

            catch
            {
                MessageBox.Show("Товар закончился!");
            }

            string Sql11 = "select * from " + carts;
            SqlConnection connection11 = new SqlConnection(Kurs7ConnectionString);
            connection11.Open();
            SqlDataAdapter command11 = new SqlDataAdapter(Sql11, connection11);
            DataSet ds11 = new DataSet();
            command11.Fill(ds11, carts);
            connection11.Close();
            data1.ItemsSource = ds11.Tables[carts].DefaultView;

            string Sql13 = "select * from " + medicine;
            SqlConnection connection13 = new SqlConnection(Kurs7ConnectionString);
            connection13.Open();
            SqlDataAdapter command13 = new SqlDataAdapter(Sql13, connection13);
            DataSet ds13 = new DataSet();
            command13.Fill(ds13, medicine);
            connection13.Close();
            data.ItemsSource = ds13.Tables[medicine].DefaultView;

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data1.ItemsSource)
            {
                sum += Int32.Parse(row["Цена"].ToString());
            }
            summ.Text = sum.ToString();
        }

        //Уменьшает количество товара
        private void minus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int pomindex = index + 1;


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
            string Sql3 = "select * from " + carts;
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
            string Sql6 = "select * from " + medicine + " WHERE Название =" + "'" + names3[index] + "';";
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

            //Если товара нет, то удаляет его
            if (quanminus == 0)
            {
                string Sql10 = "DELETE  FROM " + carts + " WHERE Название=" + "'" + names3[index] + "';";
                SqlConnection connection10 = new SqlConnection(Kurs7ConnectionString);
                connection10.Open();
                SqlCommand command10 = new SqlCommand(Sql10, connection10);
                SqlDataReader reader10 = command10.ExecuteReader();
                reader10.Close();
                connection10.Close();
            }
            else if (quanminus > 0)
            {
                string Sql15 = "UPDATE " + carts + " SET Количество = " + quanminus + ", Цена = " + pod + " WHERE Название=" + "'" + names3[index] + "';";
                SqlConnection connection15 = new SqlConnection(Kurs7ConnectionString);
                connection15.Open();
                SqlCommand command15 = new SqlCommand();
                command15.CommandText = Sql15;
                command15.Connection = connection15;
                command15.ExecuteNonQuery();
                connection15.Close();

            }

            //Обновление таблиц
            string Sql1 = "select * from " + carts;
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlDataAdapter command1 = new SqlDataAdapter(Sql1, connection1);
            DataSet ds1 = new DataSet();
            command1.Fill(ds1, carts);
            connection1.Close();
            data1.ItemsSource = ds1.Tables[carts].DefaultView;

            string Sql13 = "select * from " + medicine;
            SqlConnection connection13 = new SqlConnection(Kurs7ConnectionString);
            connection13.Open();
            SqlDataAdapter command13 = new SqlDataAdapter(Sql13, connection13);
            DataSet ds13 = new DataSet();
            command13.Fill(ds13, medicine);
            connection13.Close();
            data.ItemsSource = ds13.Tables[medicine].DefaultView;

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data1.ItemsSource)
            {
                sum += Int32.Parse(row["Цена"].ToString());
            }
            summ.Text = sum.ToString();


        }
        private async void Update(Kurs7PM.API.Models.ShoppingCartEmployee client1)
        {
            await client.PutAsJsonAsync("shoppingcartemployee/" + client1.ID_cart, client1);
        }

        //Удаление записи
        private async void Delete(int clientID)
        {
            await client.DeleteAsync("shoppingcartemployee/" + clientID);
        }

        //Получение всех данных
        private async void GetClient()
        {
            var responce = await client.GetStringAsync("shoppingcartemployee");
            var clients = JsonConvert.DeserializeObject<List<Kurs7PM.API.Models.ShoppingCartEmployee>>(responce);
            data1.DataContext = clients;
        }


        //Переход к чеку
        private void oplata(object sender, RoutedEventArgs e)
        {
            Сотрудник.Check go = new Сотрудник.Check(medicine);
            go.Show();
            Close();
        }
    }
}
