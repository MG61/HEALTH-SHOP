using Kurs7PM.Kurs7DataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Kurs7PM.Клиент
{
    /// <summary>
    /// Логика взаимодействия для ShoppingCart.xaml
    /// </summary>
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
        }


        int allpricetovar = 0;
        int chet = 0;

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
        }

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
        }

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

        //Убирает первый столбец id
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID_cart")
            {
                e.Cancel = true;
            }
        }


        private void store(object sender, RoutedEventArgs e)
        {
            Store go = new Store();
            go.Show();
            this.Close();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
