using Kurs7PM.API.Models;
using Kurs7PM.Kurs7DataSetTableAdapters;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Excel = Microsoft.Office.Interop.Excel;

namespace Kurs7PM.Сотрудник
{
    public partial class Check : Window
    {

        Kurs7DataSet DataSet = new Kurs7DataSet();
        BuxgalteriaTableAdapter BTA = new BuxgalteriaTableAdapter();
        ShoppingCartsTableAdapter STA = new ShoppingCartsTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;
        string carts = "ShoppingCartEmployee";
        string medicine;
        public Check(string medicined)
        {
            InitializeComponent();
            data.ItemsSource = DataSet.ShoppingCarts.DefaultView;

            //Обновление таблицы

            string Sql11 = "select * from " + carts;
            SqlConnection connection11 = new SqlConnection(Kurs7ConnectionString);
            connection11.Open();
            SqlDataAdapter command11 = new SqlDataAdapter(Sql11, connection11);
            DataSet ds11 = new DataSet();
            command11.Fill(ds11, carts);
            connection11.Close();
            data.ItemsSource = ds11.Tables[carts].DefaultView;

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data.ItemsSource)
            {
                sum += Int32.Parse(row["Цена"].ToString());
            }
            summ.Text = sum.ToString();
            medicine = medicined;

            int lastsumm = 0;
            string Sql = "SELECT * FROM Buxgalteria WHERE ID_bux=(SELECT max(ID_bux) FROM Buxgalteria);";
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lastsumm = Int32.Parse(reader["Остаток"].ToString());
            }
            reader.Close();
            connection.Close();

            int ostatok = Int32.Parse(summ.Text.ToString()) + lastsumm;
            BTA.InsertQuery(ostatok);
            BTA.Fill(DataSet.Buxgalteria);
        }

        //Переход к окну магазина
        private void authorization(object sender, RoutedEventArgs e)
        {
            string Sql1 = "Truncate table dbo.ShoppingCartEmployee";
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Close();
            connection1.Close();

            Сотрудник.Pharmacy go = new Сотрудник.Pharmacy(medicine);
            go.Show();
            Close();
        }

        //Убирает первый столбец id
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
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

        //Помощник для экспорта
        public static DataTable DataViewAsDataTable(DataView dv)
        {
            DataTable dt = dv.Table.Clone();
            foreach (DataRowView drv in dv)
                dt.ImportRow(drv.Row);
            return dt;
        }

        //Экспорт в Excel файл и удаление данных из корзины
        private void export(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = null;
            Excel.Workbook wb = null;

            object missing = Type.Missing;
            Excel.Worksheet ws = null;
            Excel.Range rng = null;

            excel = new Microsoft.Office.Interop.Excel.Application();
            wb = excel.Workbooks.Add();
            ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;

            DataView view = (DataView)data.ItemsSource;
            DataTable dt = DataViewAsDataTable(view);

            for (int Idx = 0; Idx < dt.Columns.Count; Idx++)
            {
                ws.Range["A1"].Offset[0, Idx].Value = dt.Columns[Idx].ColumnName;
            }

            for (int Idx = 0; Idx < dt.Rows.Count; Idx++)
            {
                ws.Range["A2"].Offset[Idx].Resize[1, dt.Columns.Count].Value = dt.Rows[Idx].ItemArray;
            }

            excel.Visible = true;
            wb.Activate();

            string Sql1 = "Truncate table dbo.ShoppingCartEmployee";
            SqlConnection connection1 = new SqlConnection(Kurs7ConnectionString);
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Close();
            connection1.Close();
            STA.Fill(DataSet.ShoppingCarts);

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data.ItemsSource)
            {
                sum += Int32.Parse(row["Цена"].ToString());
            }
            summ.Text = sum.ToString();


            Pharmacy go = new Pharmacy(medicine);
            go.Show();
            Close();
        }
    }
}
