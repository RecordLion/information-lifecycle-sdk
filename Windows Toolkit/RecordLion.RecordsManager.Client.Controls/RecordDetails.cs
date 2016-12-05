using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace RecordLion.RecordsManager.Client.Controls
{
    public class RecordDetails : Control
    {
        private FrameworkElement modelHost = null;
        private Hyperlink uriLink = null;

        static RecordDetails()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RecordDetails), new FrameworkPropertyMetadata(typeof(RecordDetails)));
        }


        public RecordDetails()
        {
        }
    
        
        public string RecordsManagerUrl
        {
            get
            {
                return base.GetValue(RecordsManagerUrlProperty) as string;
            }
            set 
            {
                base.SetValue(RecordsManagerUrlProperty, value); 
            }
        }


        public CookieContainer CookieContainer
        {
            get
            {
                return base.GetValue(CookieContainerProperty) as CookieContainer;
            }
            set
            {
                base.SetValue(CookieContainerProperty, value);
            }
        }


        public RecordsManagerCredentials Credentials
        {
            get
            {
                return base.GetValue(CredentialsProperty) as RecordsManagerCredentials;
            }
            set 
            {
                base.SetValue(CredentialsProperty, value); 
            }
        }


        public string RecordUri
        {
            get
            {
                return base.GetValue(RecordUriProperty) as string;
            }
            set 
            {
                base.SetValue(RecordUriProperty, value); 
            }
        }


        public RecordDetailsModel Model
        {
            get
            {
                return (this.ModelHost != null) ? this.ModelHost.TryFindResource("Model") as RecordDetailsModel : null;
            }
        }


        protected FrameworkElement ModelHost
        {
            get
            {
                return this.modelHost;
            }
            set
            {
                this.modelHost = value;
            }
        }


        protected Hyperlink UriLink
        {
            get
            {
                return this.uriLink;
            }
            set
            {
                this.uriLink = value;

                if (this.uriLink != null)
                {
                    this.uriLink.RequestNavigate += (sender, e) =>
                    {
                        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                        e.Handled = true;
                    };
                }
            }
        }


        public override void OnApplyTemplate()
        {
            //var root = this.get(0);
            //ResourceDictionary resources = new ResourceDictionary();
            //resources.Add("Model", new RecordDetailsModel());
            //root.Resources.MergedDictionaries.Add(resources);
            this.ModelHost = this.GetTemplateChild("ModelHost") as FrameworkElement;

            this.UriLink = this.GetTemplateChild("UriLink") as Hyperlink;

            this.ConfigureModel();

            base.OnApplyTemplate();
        }


        private void ConfigureModel()
        {
            if (this.Model != null)
            {
                if (!string.IsNullOrEmpty(this.RecordsManagerUrl))
                {
                    if (this.CookieContainer != null)
                    {
                        this.Model.Client = RecordsManagerClientFactory.Create(this.RecordsManagerUrl, this.CookieContainer);
                    }
                    else if (this.Credentials != null)
                    {
                        this.Model.Client = RecordsManagerClientFactory.Create(this.RecordsManagerUrl, this.Credentials);
                    }
                    else
                        this.Model.Client = RecordsManagerClientFactory.Create(this.RecordsManagerUrl);
                }
                else
                    this.Model.Client = null;

                this.Model.Load(this.RecordUri);
            }
        }

        #region Dependency Properties
        
        public static readonly DependencyProperty RecordsManagerUrlProperty = DependencyProperty.Register("RecordsManagerUrl", typeof(string), typeof(RecordDetails), new PropertyMetadata(null, (sender, e) =>
        {
            RecordDetails control = (RecordDetails)sender;
            control.ConfigureModel();
        }));
        
        public static readonly DependencyProperty CookieContainerProperty = DependencyProperty.Register("CookieContainer", typeof(CookieContainer), typeof(RecordDetails), new PropertyMetadata(null, (sender, e) =>
        {
            RecordDetails control = (RecordDetails)sender;
            control.ConfigureModel();
        }));
        
        public static readonly DependencyProperty CredentialsProperty = DependencyProperty.Register("Credentials", typeof(RecordsManagerCredentials), typeof(RecordDetails), new PropertyMetadata(null, (sender, e) =>
        {
            RecordDetails control = (RecordDetails)sender;
            control.ConfigureModel();
        }));
        
        public static readonly DependencyProperty RecordUriProperty = DependencyProperty.Register("RecordUri", typeof(string), typeof(RecordDetails), new PropertyMetadata(null, (sender, e) =>
        {
            RecordDetails control = (RecordDetails)sender;
            control.ConfigureModel();
        }));
        
        #endregion
    }
}