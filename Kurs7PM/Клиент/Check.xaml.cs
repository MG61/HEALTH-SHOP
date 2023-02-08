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
using System.Drawing;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using Kurs7PM.Kurs7DataSetTableAdapters;
using System.Data.SqlClient;
using System.Reflection;

namespace Kurs7PM.Клиент
{
    public partial class Check : Window
    {

        Kurs7DataSet DataSet = new Kurs7DataSet();
        ShoppingCartTableAdapter STA = new ShoppingCartTableAdapter();
        ShoppingCartHelpTableAdapter SHTA = new ShoppingCartHelpTableAdapter();

        public Check()
        {
            InitializeComponent();
            data.ItemsSource = DataSet.ShoppingCart.DefaultView;
            STA.Fill(DataSet.ShoppingCart);

            //Подсчёт суммы
            int sum = 0;
            foreach (DataRowView row in data.ItemsSource)
            {
                sum += (int)row["Цена"];
            }
            summ.Text = sum.ToString();
        }

        //Переход к окну магазина
        private void authorization(object sender, RoutedEventArgs e)
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

            Store go = new Store();
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


            Store go = new Store();
            go.Show();
            Close();
        }
    }
}
