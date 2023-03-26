using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Kurs7PM.Авторизация.Регистрация
{
    public partial class Provider : Window
    {

        HttpClient client = new HttpClient();

        public Provider(string sklada)
        {
            InitializeComponent();

            client.BaseAddress = new Uri("https://localhost:7005/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Get();
            sklad.Items.Add(sklada);
        }

        //Получение всех данных
        private async void Get()
        {
            var responce = await client.GetStringAsync("provider");
            var providers = JsonConvert.DeserializeObject<List<Kurs7PM.API.Models.Provider>>(responce);
            //data.DataContext = clients;
        }

        //Создание записи
        private async void Save(Kurs7PM.API.Models.Provider client1)
        {
            await client.PostAsJsonAsync("provider", client1);
        }

        //Обновление записи
        private async void Update(Kurs7PM.API.Models.Provider client1)
        {
            await client.PostAsJsonAsync("provider/" + client1.ID_provider, client1);
        }

        //Удаление записи
        private async void Delete(int ProviderID)
        {
            await client.DeleteAsync("provider" + ProviderID);
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

        //Добавление поставщика
        private void add_Provider(object sender, RoutedEventArgs e)
        {
            var Number = new Regex(@"[0-9]+");
            var Angl = new Regex(@"[A-Z]+");
            var MinAngl = new Regex(@"[a-z]+");
            var Rus = new Regex(@"[А-Я]+");
            var MinRus = new Regex(@"[а-я]+");
            var Minsimbols = new Regex(@".{4,50}");
            var Effects = new Regex(@"[!@#$%^&*()_+=[{]};:<>|./?,-]");

            if (!string.IsNullOrWhiteSpace(login.Text) )
            {
                if (Angl.IsMatch(password.Password) && MinAngl.IsMatch(password.Password)) {
                    var provider = new Kurs7PM.API.Models.Provider()
                    {
                        Логин = login.Text,

                        Пароль = password.Password,

                        Фамилия = familia.Text,

                        Имя = name.Text,

                        Отчество = middle_name.Text,

                        Склад = sklad.Text
                    };

                    login.Text = "";
                    password.Password = "";
                    familia.Text = "";
                    name.Text = "";
                    middle_name.Text = "";
                    sklad.Text = "";
                    this.Save(provider);
            
                    MessageBox.Show("Вы успешно зарегистрированы!");
                }
            }
            else { MessageBox.Show("Проверьте правильность введённых данных!"); }

            Get();
        }

        //Переход
        private void sklad11(object sender, RoutedEventArgs e)
        {
            Sklad go = new Sklad();
            go.Show();
            Close();
        }

    }
}
