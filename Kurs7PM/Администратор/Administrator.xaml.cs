﻿using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Авторизация.Регистрация;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kurs7PM.Администратор
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        AdministratorTableAdapter ATA = new AdministratorTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public Administrator()
        {
            InitializeComponent();
            data.ItemsSource = DataSet.Administrator.DefaultView;
            ATA.Fill(DataSet.Administrator);

            if (0 == DataSet.Administrator.Rows.Count)
            {
                ATA.InsertQuery("Admin", "Admin", "Тестер", "Тестер", "Тестер");
                ATA.Fill(DataSet.Administrator);
            }
        }

        //Удаляет запись выбранной ячейки
        private void minus_korz(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Int32.Parse(button.Tag.ToString());
            int id = index + 1;
            ATA.DeleteQuery(id);
            ATA.Fill(DataSet.Administrator);
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
            if (e.PropertyName == "ID_administrator")
            {
                e.Cancel = true;
            }
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

        //Добавление
        private void add_administrator(object sender, RoutedEventArgs e)
        {
            var Number = new Regex(@"[0-9]+");
            var Angl = new Regex(@"[A-Z]+");
            var MinAngl = new Regex(@"[a-z]+");
            var Rus = new Regex(@"[А-Я]+");
            var MinRus = new Regex(@"[а-я]+");
            var Minsimbols = new Regex(@".{4,50}");
            var Effects = new Regex(@"[!@#$%^&*()_+=[{]};:<>|./?,-]");

            if (!string.IsNullOrWhiteSpace(login.Text) && !string.IsNullOrWhiteSpace(password.Password) && !string.IsNullOrWhiteSpace(familia.Text) && !string.IsNullOrWhiteSpace(name.Text) && !string.IsNullOrWhiteSpace(middle_name.Text))
            {
                if (Angl.IsMatch(password.Password) && MinAngl.IsMatch(password.Password) && Minsimbols.IsMatch(password.Password) && Effects.IsMatch(password.Password))
                {
                    ATA.InsertQuery(login.Text, password.Password, familia.Text, name.Text, middle_name.Text);
                    ATA.Fill(DataSet.Administrator);
                }
            }
            else { MessageBox.Show("Проверьте правильность введённых данных!"); }
        }

        //Переход к сотрудникам
        private void employee(object sender, RoutedEventArgs e)
        {
            Employee go = new Employee();
            go.Show();
            Close();
        }
    }
}

