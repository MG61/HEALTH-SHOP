using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Администратор;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

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

        //Добавление записи
        private void add_branch(object sender, RoutedEventArgs e)
        {
            var Number = new Regex(@"[0-9]+");
            var Angl = new Regex(@"[A-Z]+");
            var MinAngl = new Regex(@"[a-z]+");
            var Rus = new Regex(@"[А-Я]+");
            var MinRus = new Regex(@"[а-я]+");
            var Minsimbols = new Regex(@".{4,50}");
            var Effects = new Regex(@"[!@#$%^&*()_+=[{]};:<>|./?,-]");

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
                    int ostatok = Int32.Parse(filials.Text.ToString()) + lastsumm;

            if (!string.IsNullOrWhiteSpace(ostatok.ToString()))
            {
                if (Number.IsMatch(ostatok.ToString()) || Number.IsMatch(ostatok.ToString()))
                {
                    BTA.InsertQuery(ostatok);
                    BTA.Fill(DataSet.Buxgalteria);
                    Graph();
                }
            }
            else { MessageBox.Show("Проверьте правильность введённых данных!"); }
        }
    }
}