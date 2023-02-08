﻿using Kurs7PM.Клиент;
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
using Kurs7PM.Kurs7DataSetTableAdapters;

namespace Kurs7PM.Администратор
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        EmployeeTableAdapter ETA = new EmployeeTableAdapter();
        BranchTableAdapter BTA = new BranchTableAdapter();

        public Employee()
        {
            InitializeComponent();
            data.ItemsSource = DataSet.Employee.DefaultView;
            ETA.Fill(DataSet.Employee);
            BTA.Fill(DataSet.Branch);

            string Sql1 = "select Name from dbo.Branch";
            SqlConnection connection1 = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Kurs7;Integrated Security=True");
            connection1.Open();
            SqlCommand command1 = new SqlCommand(Sql1, connection1);
            SqlDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                filial.Items.Add(reader1["Name"].ToString());
            }

            reader1.Close();
            connection1.Close();
        }

        //Удаляет запись выбранной ячейки
        private void minus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int id = index + 1;
            ETA.DeleteQuery(id);
            ETA.Fill(DataSet.Employee);
        }

        //Отправляет удаление в конец datagrid
        private void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "Удалить")
                {
                    item.DisplayIndex = grid.Columns.Count - 1;
                    break;
                }
            }
        }

        //Убирает первый столбец datagrid (id)
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID_employee")
            {
                e.Cancel = true;
            }
        }

        //Переход к меню
        private void menu(object sender, RoutedEventArgs e)
        {
            MenuAdministrator go = new MenuAdministrator();
            go.Show();
            this.Close();
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
        private void add_employee(object sender, RoutedEventArgs e)
        {
            ETA.InsertQuery(login.Text, password.Text, familia.Text, name.Text, middle_name.Text, filial.Text);
            ETA.Fill(DataSet.Employee);
        }

        //Переход к филиалам
        private void branch(object sender, RoutedEventArgs e)
        {
            Branch go =  new Branch();
            go.Show();
            this.Close();
        }
    }


}
