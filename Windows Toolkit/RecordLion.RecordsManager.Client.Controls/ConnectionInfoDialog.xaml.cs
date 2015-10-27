using System;
using System.Linq;
using System.Windows;

namespace RecordLion.RecordsManager.Client.Controls
{
    public partial class ConnectionInfoDialog : Window
    {
        public ConnectionInfoDialog()
        {
            InitializeComponent();
        }


        public string RecordsManagerUrl
        {
            get
            {
                return this.ConnectionInfo.RecordsManagerUrl;
            }
        }


        private bool Validate()
        {
            if (!string.IsNullOrEmpty(this.RecordsManagerUrl))
            {
                RecordsManagerClient client = new RecordsManagerClient(this.RecordsManagerUrl);

                if (client.GetSystemInfo() != null)
                {
                    return true;
                }
                else
                    this.Message.Text = LocalStrings.ValidationUrl;
            }
            else
                this.Message.Text = LocalStrings.ValidationUrl;

            return false;
        }


        private void ConnectionInfo_Updated(object sender, EventArgs e)
        {
            this.Validate();
        }


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (this.Validate())
            {
                this.DialogResult = true;
                this.Close();
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}