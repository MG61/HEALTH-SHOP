using Kurs7PM.Администратор;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Definitions.Charts;
using LiveCharts.Charts;
using LiveCharts.Defaults;
using System.ComponentModel;
using System.Globalization;
using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Авторизация.Регистрация;

namespace Kurs7PM.Бухгалтер
{
    public partial class Buxgalter : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        BuxgalteriaTableAdapter BTA = new BuxgalteriaTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public Buxgalter()
        {
            InitializeComponent();
            BTA.Fill(DataSet.Buxgalteria);
            Graph();
            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }

        public void Graph()
        {
            SeriesCollection series = new SeriesCollection();

            ChartValues<int> values = new ChartValues<int>();

            List<String> dates = new List<String>();

            foreach (var Row in DataSet.Buxgalteria)
            {
                values.Add(Row.Остаток);

                //dates.Add(Row.Дата.ToString());
            }

            cartesianChart1.AxisX.Clear();

            cartesianChart1.AxisX.Add(new Axis()
            {
                Title = "Остаток",
                Labels = dates
            });

            LineSeries line = new LineSeries();

            line.Title = "Бухгалтер";
            line.Values = values;

            series.Add(line);

            cartesianChart1.Series = series;
        }
        
        //Переход к меню
        private void menu(object sender, RoutedEventArgs e)
        {
            MenuAdministrator go = new MenuAdministrator();
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

        //Добавление сотрудника
        private void add_branch(object sender, RoutedEventArgs e)
        {
            int lastsumm = 0;
            string Sql = "SELECT * FROM Buxgalteria WHERE ID_bux=(SELECT max(ID_bux) FROM Buxgalteria);";
            SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(Sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lastsumm= Int32.Parse(reader["Остаток"].ToString());
            }
            reader.Close();
            connection.Close();

            int ostatok = Int32.Parse(filials.Text.ToString()) + lastsumm;
            BTA.InsertQuery(ostatok);
            BTA.Fill(DataSet.Buxgalteria);
            Graph();
        }
    }
}