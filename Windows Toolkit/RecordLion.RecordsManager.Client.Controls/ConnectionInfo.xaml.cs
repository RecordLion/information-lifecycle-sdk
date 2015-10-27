using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecordLion.RecordsManager.Client.Controls
{
    /// <summary>
    /// Interaction logic for ConnectionInfo.xaml
    /// </summary>
    public partial class ConnectionInfo : UserControl
    {
        public event EventHandler Updated = null;

        public ConnectionInfo()
        {
            InitializeComponent();
        }
        

        public string RecordsManagerUrl
        {
            get
            {
                return this.Url.Text;
            }
        }


        private void OnUpdated()
        {
            if (this.Updated != null)
                this.Updated(this, new EventArgs());
        }


        private void Url_LostFocus(object sender, RoutedEventArgs e)
        {
            this.OnUpdated();
        }


        private void Url_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                this.OnUpdated();
        }
    }
}