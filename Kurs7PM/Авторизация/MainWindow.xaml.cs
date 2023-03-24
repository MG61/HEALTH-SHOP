using Kurs7PM.Kurs7DataSetTableAdapters;
using Kurs7PM.Авторизация.Регистрация;
using Kurs7PM.Администратор;
using Kurs7PM.Бухгалтер;
using Kurs7PM.Клиент;
using Kurs7PM.Сотрудник;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Kurs7PM.Авторизация
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Kurs7DataSet DataSet = new Kurs7DataSet();
        AdministratorTableAdapter ATA = new AdministratorTableAdapter();
        EmployeeTableAdapter ETA = new EmployeeTableAdapter();
        ProvidersTableAdapter PTA = new ProvidersTableAdapter();
        ClientsTableAdapter CTA = new ClientsTableAdapter();
        BuxgalterTableAdapter BTA = new BuxgalterTableAdapter();
        BuxgalteriaTableAdapter BTAB = new BuxgalteriaTableAdapter();
        string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public MainWindow()
        {
            InitializeComponent();
            ATA.Fill(DataSet.Administrator);
            ETA.Fill(DataSet.Employee);
            PTA.Fill(DataSet.Providers);
            CTA.Fill(DataSet.Clients);
            BTA.Fill(DataSet.Buxgalter);
            if (0 == DataSet.Administrator.Rows.Count)
            {
                ATA.InsertQuery("Admin", "Admin", "Тестер", "Тестер", "Тестер");
                ATA.Fill(DataSet.Administrator);
            }
            if (0 == DataSet.Buxgalteria.Rows.Count)
            {
                BTAB.InsertQuery(100000);
                BTAB.Fill(DataSet.Buxgalteria);
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

        private void Voyti_Click(object sender, RoutedEventArgs e)
        {
            var Number = new Regex(@"[0-9]+");
            var Angl = new Regex(@"[A-Z]+");
            var MinAngl = new Regex(@"[a-z]+");
            var Rus = new Regex(@"[А-Я]+");
            var MinRus = new Regex(@"[а-я]+");
            var Minsimbols = new Regex(@".{4,50}");
            var Effects = new Regex(@"[!@#$%^&*()_+=[{]};:<>|./?,-]");

            if (!string.IsNullOrWhiteSpace(login.Text) && !string.IsNullOrWhiteSpace(password.Password))
            {
                if (Angl.IsMatch(password.Password) && MinAngl.IsMatch(password.Password) && Minsimbols.IsMatch(password.Password) && Effects.IsMatch(password.Password))
                {
                    string log = login.Text;
                    string pass = password.Password;
                    AdminAuth(log, pass);
                    EmployeeAuth(log, pass);
                    ProviderAuth(log, pass);
                    ClientAuth(log, pass);
                    BuxAuth(log, pass);
                }
            }
            else { MessageBox.Show("Проверьте правильность введённых данных!"); }
        }

        int nepov = 0;
        int prov = 1;

        private void AdminAuth(string adminlog, string adminpass)
        {
            try
            {
                for (int i = 0; i < DataSet.Administrator.Rows.Count; i++)
                {
                    if (adminlog == DataSet.Administrator.Rows[i][1].ToString() && adminpass == DataSet.Administrator.Rows[i][2].ToString())
                    {
                        if (nepov == 0)
                        {
                            prov = 1;
                            MenuAdministrator go = new MenuAdministrator();
                            go.Show();
                            Close();
                        }

                        nepov = 1;
                    }
                    else if (prov == 0)
                    {
                        MessageBox.Show("Введите данные!");
                    }
                }
            }
            catch
            {
                return;
            }
            nepov = 0;
        }

        private void EmployeeAuth(string sotrlog, string sotrpass)
        {
            try
            {
                for (int i = 0; i < DataSet.Employee.Rows.Count; i++)
                {
                    if (sotrlog == DataSet.Employee.Rows[i][1].ToString() && GetHash(sotrpass) == DataSet.Employee.Rows[i][2].ToString())
                    {
                        prov = 1;
                        string Sql = "select * from dbo.Employee";
                        SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
                        connection.Open();
                        SqlCommand command = new SqlCommand(Sql, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        List<string> names = new List<string>();
                        while (reader.Read())
                        {
                            names.Add(reader["Место_работы"].ToString());
                        }
                        reader.Close();
                        connection.Close();

                        Pharmacy da = new Pharmacy(names[i]);
                        da.Show();
                        this.Close();
                    }
                    else if (prov == 0)
                    {
                        MessageBox.Show("Введите данные!");
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void ProviderAuth(string custlog, string custpass)
        {
            try
            {
                for (int i = 0; i < DataSet.Providers.Rows.Count; i++)
                {
                    if (custlog == DataSet.Providers.Rows[i][1].ToString() && custpass == DataSet.Providers.Rows[i][2].ToString())
                    {
                        prov = 1;

                        string Sql = "select * from dbo.Providers";
                        SqlConnection connection = new SqlConnection(Kurs7ConnectionString);
                        connection.Open();
                        SqlCommand command = new SqlCommand(Sql, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        List<string> names = new List<string>();
                        while (reader.Read())
                        {
                            names.Add(reader["Склад"].ToString());
                        }
                        reader.Close();
                        connection.Close();

                        Поставщик.Provider da1 = new Поставщик.Provider(names[i]);
                        da1.Show();
                        this.Close();
                    }
                    else if (prov == 0)
                    {
                        MessageBox.Show("Введите данные!");
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void ClientAuth(string custlog, string custpass)
        {
            try
            {
                for (int i = 0; i < DataSet.Clients.Rows.Count; i++)
                {
                    if (custlog == DataSet.Clients.Rows[i][1].ToString() && custpass == DataSet.Clients.Rows[i][2].ToString())
                    {
                        prov = 1;
                        Клиент.Store da1 = new Клиент.Store();
                        da1.Show();
                        this.Close();
                    }
                    else if (prov == 0)
                    {
                        MessageBox.Show("Введите данные!");
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void BuxAuth(string custlog, string custpass)
        {
            try
            {
                for (int i = 0; i < DataSet.Buxgalter.Rows.Count; i++)
                {
                    if (custlog == DataSet.Buxgalter.Rows[i][1].ToString() && custpass == DataSet.Buxgalter.Rows[i][2].ToString())
                    {
                        prov = 1;
                        Buxgalter da1 = new Buxgalter();
                        da1.Show();
                        this.Close();
                    }
                    else if (prov == 0)
                    {
                        MessageBox.Show("Введите данные!");
                    }
                }
            }
            catch
            {
                return;
            }
        }

        //Хэширование
        public string GetHash(string pass)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));

            return Convert.ToBase64String(hash);
        }


        private void provider(object sender, RoutedEventArgs e)
        {
            Авторизация.Регистрация.Provider go = new Авторизация.Регистрация.Provider("");
            go.Show();
            Close();
        }

        private void log_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNMйцукёенгшщзхъэждлорпавыфячсмитьбю.ЙЦУКЕНГШЩЗХЪЭЖДЛОРПАВЫФЯЧСМИТЬБЮЁ".IndexOf(e.Text) < 0;
        }

        private void pass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNMйцукёенгшщзхъэждлорпавыфячсмитьбю.ЙЦУКЕНГШЩЗХЪЭЖДЛОРПАВЫФЯЧСМИТЬБЮЁ".IndexOf(e.Text) < 0;
        }

        private void clientreg(object sender, RoutedEventArgs e)
        {
            Client da1 = new Client();
            da1.Show();
            this.Close();
        }
    }
}
