using System;
using System.Linq;
using System.Net;
using System.Windows;

namespace RecordLion.RecordsManager.Client.Controls
{
    public partial class PassiveSigninDialog : Window
    {
        public PassiveSigninDialog(string recordsManagerUrl)
        { 
            InitializeComponent();

            this.SignInControl.RecordsManagerUrl = recordsManagerUrl;            
        }


        public CookieContainer CookieContainer
        {
            get
            {
                return this.SignInControl.CookieContainer;
            }
        }


        private void SignInControl_SignInComplete(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SignInControl.SignIn();
        }
    }
}