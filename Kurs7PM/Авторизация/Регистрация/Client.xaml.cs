using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
using Kurs7PM.API.Models;

namespace Kurs7PM.Авторизация.Регистрация
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        HttpClient client = new HttpClient();
        //string Kurs7ConnectionString = Properties.Settings.Default.Kurs7ConnectionString1;

        public Client()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:7005/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            GetClient();
        }

        private async void GetClient()
        {
            var responce = await client.GetStringAsync("client");
            var clients = JsonConvert.DeserializeObject<List<Kurs7PM.API.Models.Client>>(responce);
            //data.DataContext = clients;
        }

        private async void SaveClient(Kurs7PM.API.Models.Client client1)
        {
            await client.PostAsJsonAsync("client", client1);
        }

        private async void UpdateClient(Kurs7PM.API.Models.Client client1)
        {
            await client.PostAsJsonAsync("client/" + client1.ID_client, client1);
        }

        private async void DeleteClient(int clientID)
        {
            await client.DeleteAsync("client" + clientID);
        }

        //Добавление клиента
        private void add_Provider(object sender, RoutedEventArgs e)
        {
            var client = new Kurs7PM.API.Models.Client()
            {
                Логин = login.Text,

                Пароль = password.Text,

                Фамилия = familia.Text,

                Имя = name.Text,

                Отчество = middle_name.Text
            };

            login.Text = "";
            password.Text = "";
            familia.Text = "";
            name.Text = ""; 
            middle_name.Text = "";

            this.SaveClient(client);
            GetClient();
            MessageBox.Show("Вы успешно зарегистрированы!");
        }

        //Переход к меню
        private void auth(object sender, RoutedEventArgs e)
        {
            MainWindow go = new MainWindow();
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
    }
}
