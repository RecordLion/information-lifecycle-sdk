using RecordLion.RecordsManager.Client.Controls.Utils;
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace RecordLion.RecordsManager.Client.Controls
{
    public partial class ManageRecordDialog : Window, IDisposable
    {
        private static CookieContainer cookies = null;


        private string recordsManagerUrl = null;
        private string recordUri = null;               


        public ManageRecordDialog(string recordsManagerUrl, string recordUri)
        {
            InitializeComponent();

            this.recordsManagerUrl = recordsManagerUrl;
            this.recordUri = recordUri;
        }


        public ManageRecordModel Model
        {
            get
            {
                return this.DataContext as ManageRecordModel;
            }
        }

        public bool HideDetailsLink
        {
            get
            {
                return this.DetailsLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.DetailsLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool HideClassificationLink
        {
            get
            {
                return this.ClassificationLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.ClassificationLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool HideDeclarationLink
        {
            get
            {
                return this.DeclarationLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.DeclarationLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool HideHoldsLink
        {
            get
            {
                return this.HoldLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.HoldLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool HideAuditLink
        {
            get
            {
                return this.AuditLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.AuditLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool HideRequestLink
        {
            get
            {
                return this.RequestLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.RequestLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool HidePropertiesLink
        {
            get
            {
                return this.PropertiesLink.Visibility == Visibility.Visible;
            }
            set
            {
                this.PropertiesLink.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetActive(this.DetailsLink);

            this.Model.Mode = ManageRecordMode.Details;            
            this.Model.RecordsManagerUrl = this.recordsManagerUrl;
            this.Model.RecordUri = this.recordUri;
            this.Model.PropertyChanged += Model_PropertyChanged;

            this.Height = this.Model.TargetHeight;
            this.Width = this.Model.TargetWidth;

            this.Browser.Navigate(new Uri(this.Model.TargetUrl));
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TargetUrl")
                this.Browser.Navigate(new Uri(this.Model.TargetUrl));

            if (e.PropertyName == "TargetHeight")
                this.Height = this.Model.TargetHeight;

            if (e.PropertyName == "TargetWidth")
                this.Width = this.Model.TargetWidth;                  

            if (e.PropertyName == "Error" && this.Model.Error != null)
                this.HandleError();
        }

        private void AuditLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.Audit;
            this.SetActive((TextBlock)sender);
        }

        private void HoldLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.LegalHolds;
            this.SetActive((TextBlock)sender);
        }

        private void DeclarationLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.Declaration;
            this.SetActive((TextBlock)sender);
        }

        private void ClassificationLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.Classification;
            this.SetActive((TextBlock)sender);
        }

        private void RequestLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.Request;
            this.SetActive((TextBlock)sender);
        }

        private void DetailsLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.Details;
            this.SetActive((TextBlock)sender);
        }

        private void PropertiesLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.Mode = ManageRecordMode.Properties;
            this.SetActive((TextBlock)sender);
        }


        private void SetActive(TextBlock active)
        {
            this.AuditLink.Style = this.FindResource("NavigationLink") as Style;
            this.HoldLink.Style = this.FindResource("NavigationLink") as Style;
            this.DeclarationLink.Style = this.FindResource("NavigationLink") as Style;
            this.ClassificationLink.Style = this.FindResource("NavigationLink") as Style;
            this.RequestLink.Style = this.FindResource("NavigationLink") as Style;
            this.DetailsLink.Style = this.FindResource("NavigationLink") as Style;
            this.PropertiesLink.Style = this.FindResource("NavigationLink") as Style;

            active.Style = this.FindResource("NavigationLinkActive") as Style;
        }


        private void HandleError()
        {
            MessageBox.Show(string.Format("An unexepected error has occurred: {0}", this.Model.Error));

            this.Model.Error = null;
        }


        public void Dispose()
        {
            if (this.Browser != null)
            {
                this.Browser.Dispose();
                this.Browser = null;
            }
        }
    }
}