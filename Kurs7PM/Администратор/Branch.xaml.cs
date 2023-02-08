﻿using Kurs7PM.Kurs7DataSetTableAdapters;
using System;
using System.Collections.Generic;
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

namespace Kurs7PM.Администратор
{
    public partial class Branch : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        BranchTableAdapter BTA = new BranchTableAdapter();

        public Branch()
        {
            InitializeComponent();
            data.ItemsSource = DataSet.Branch.DefaultView;
            BTA.Fill(DataSet.Branch);
        }

        //Удаляет запись выбранной ячейки
        private void minus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int id = index + 1;
            BTA.DeleteQuery(id);
            BTA.Fill(DataSet.Branch);
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
            if (e.PropertyName == "ID_branch")
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
        private void add_branch(object sender, RoutedEventArgs e)
        {
            BTA.InsertQuery(filials.Text);
            BTA.Fill(DataSet.Branch);
        }

        //Переход к сотрудникам
        private void employee(object sender, RoutedEventArgs e)
        {
            Employee go = new Employee();
            go.Show();
            this.Close();
        }
    }
}
