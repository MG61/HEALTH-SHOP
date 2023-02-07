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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurs7PM.Авторизация
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Kurs7DataSet dataSet = new Kurs7DataSet();
        //AdminTableAdapter ATA = new AdminTableAdapter();

        public MainWindow()
        {
            InitializeComponent();
            //ATA.Fill(dataSet.Admin);

            //if (0 == dataSet.Admin.Rows.Count)
            //{
            //    ATA.InsertQuery("Admin", "Admin", "Admin");
            //    ATA.Fill(dataSet.Admin);
            //}
        }

        private void Voyti_Click(object sender, RoutedEventArgs e)
        {
            string log = LOG.Text;
            string pass = PASS.Text;
            //AdminAuth(log, pass);
            //MCKAuth(log, pass);
            //RukAuth(log, pass);
        }

        //private void AdminAuth(string adminlog, string adminpass)
        //{
        //    try
        //    {
        //        for (int i = 0; i < dataSet.Admin.Rows.Count; i++)
        //        {
        //            if (i > dataSet.Admin.Rows.Count)
        //            {
        //                return;
        //            }
        //            else if (adminlog == dataSet.Admin.Rows[i][0].ToString() && adminpass == dataSet.Admin.Rows[i][1].ToString() && "Admin" == dataSet.Admin.Rows[i][2].ToString())
        //            {
        //                MenuAdmin adm = new MenuAdmin();
        //                adm.Show();
        //                this.Close();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        //private void MCKAuth(string sotrlog, string sotrpass)
        //{
        //    try
        //    {
        //        for (int i = 0; i < dataSet.Admin.Rows.Count; i++)
        //        {
        //            if (i > dataSet.Admin.Rows.Count)
        //            {
        //                return;
        //            }
        //            else if (sotrlog == dataSet.Admin.Rows[i][0].ToString() && sotrpass == dataSet.Admin.Rows[i][1].ToString() && "PMCK" == dataSet.Admin.Rows[i][2].ToString())
        //            {
        //                MenuMCK da = new MenuMCK();
        //                da.Show();
        //                this.Close();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        //private void RukAuth(string custlog, string custpass)
        //{
        //    try
        //    {
        //        for (int i = 0; i < dataSet.Admin.Rows.Count; i++)
        //        {
        //            if (i > dataSet.Admin.Rows.Count)
        //            {
        //                return;
        //            }
        //            else if (custlog == dataSet.Admin.Rows[i][0].ToString() && custpass == dataSet.Admin.Rows[i][1].ToString() && "Ruk" == dataSet.Admin.Rows[i][2].ToString())
        //            {
        //                Ruk da1 = new Ruk();
        //                da1.Show();
        //                this.Close();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        private void log_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNMйцукёенгшщзхъэждлорпавыфячсмитьбю.ЙЦУКЕНГШЩЗХЪЭЖДЛОРПАВЫФЯЧСМИТЬБЮЁ".IndexOf(e.Text) < 0;
        }
        private void pass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNMйцукёенгшщзхъэждлорпавыфячсмитьбю.ЙЦУКЕНГШЩЗХЪЭЖДЛОРПАВЫФЯЧСМИТЬБЮЁ".IndexOf(e.Text) < 0;
        }
    }
}
