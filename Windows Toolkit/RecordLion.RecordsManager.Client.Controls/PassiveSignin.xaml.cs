using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using RecordLion.RecordsManager.Client.Controls.Utils;

namespace RecordLion.RecordsManager.Client.Controls
{
    public partial class PassiveSignin : UserControl
    {
        public event EventHandler SignInComplete = null;


        private string recordsManagerUrl = string.Empty;

        public PassiveSignin() : this(null)
        {
        }


        public PassiveSignin(string recordsManagerUrl)
        {
            InitializeComponent();
            this.RecordsManagerUrl = recordsManagerUrl;
        }


        public string RecordsManagerUrl
        {
            get
            {
                return this.recordsManagerUrl;
            }
            set
            {
                this.recordsManagerUrl = (!string.IsNullOrEmpty(value)) ? new Uri(value).GetLeftPart(UriPartial.Authority) :
                                         value;
            }
        }


        public CookieContainer CookieContainer { get; private set; }


        public void SignIn()
        {
            bool isInvalidUrl = false;

            try
            {
                isInvalidUrl = (new RecordsManagerClient(this.RecordsManagerUrl).GetSystemInfo() == null);
            }
            catch (Exception)
            {
                isInvalidUrl = true;
            }
         
            if (isInvalidUrl)
            {
                this.Message.Content = LocalStrings.ValidationUrl;
                this.Message.Visibility = System.Windows.Visibility.Visible;
                this.Browser.Visibility = System.Windows.Visibility.Hidden;

                this.OnSignInComplete();
            }
            else
            {
                this.Browser.Navigate(new Uri(this.RecordsManagerUrl + "?api=true"));
                this.Message.Visibility = System.Windows.Visibility.Hidden;
                this.Browser.Visibility = System.Windows.Visibility.Visible;
            }
        }      


        private void OnSignInComplete()
        {
            if (this.SignInComplete != null)
                this.SignInComplete(this, new EventArgs());
        }


        private void Browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (AuthUtils.IsSignedIn(this.RecordsManagerUrl, e.Uri.ToString()))
            {
                this.Browser.Navigate(new Uri("about:blank"));

                this.CookieContainer = AuthUtils.GetCookieContainer(new Uri(this.RecordsManagerUrl));

                this.OnSignInComplete();
            }
        }
    }
}