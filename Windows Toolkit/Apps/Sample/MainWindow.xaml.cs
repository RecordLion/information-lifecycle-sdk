using RecordLion.RecordsManager.Client;
using RecordLion.RecordsManager.Client.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Xml.Serialization;

namespace Sample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void DemoDetailsDialogButton_Click(object sender, RoutedEventArgs e)
        {
            RecordDetailsDialog dialog = new RecordDetailsDialog(this.RMUrl.Text, this.Signin(), this.RUri.Text);
            dialog.Show();
        }

        private void DemoClientButton_Click(object sender, RoutedEventArgs e)
        {
            CookieContainer cookieContainer = this.Signin();

            RecordsManagerClient client = new RecordsManagerClient(this.RMUrl.Text, cookieContainer);
            MessageBox.Show(client.GetRecordsLastEdit().ToString());
        }            

        private void DemoManageRecordButton_Click(object sender, RoutedEventArgs e)
        {
            ManageRecordDialog dialog = new ManageRecordDialog(this.RMUrl.Text, this.RUri.Text);
            dialog.ShowDialog();
        }

        private void DemoDetailsControlButton_Click(object sender, RoutedEventArgs e)
        {
            RecordDetails window = new RecordDetails(this.RMUrl.Text, this.RUri.Text);
            window.ShowDialog();
        }


        private CookieContainer Signin()
        {
            CookieContainer cookieContainer = new CookieContainer();

            PassiveSigninDialog dialog = new PassiveSigninDialog(this.RMUrl.Text);

            if (dialog.ShowDialog() ?? false)
            {
                cookieContainer = dialog.CookieContainer;
            }

            return cookieContainer;
        }
    }
}
