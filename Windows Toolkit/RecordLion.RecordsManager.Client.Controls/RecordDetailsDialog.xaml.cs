using System;
using System.Linq;
using System.Net;
using System.Windows;

namespace RecordLion.RecordsManager.Client.Controls
{
    public partial class RecordDetailsDialog : Window
    {
        public RecordDetailsDialog(string recordsManagerUrl, string recordUri) : this(recordsManagerUrl, new RecordsManagerCredentials(), recordUri)
        {
        }


        public RecordDetailsDialog(string recordsManagerUrl, CookieContainer cookieContainer, string recordUri) : this(recordsManagerUrl, (RecordsManagerCredentials)null, recordUri)
        {
            this.CookieContainer = cookieContainer;
        }


        public RecordDetailsDialog(string recordsManagerUrl, RecordsManagerCredentials credentials, string recordUri)
        { 
            InitializeComponent();

            this.RecordsManagerUrl = recordsManagerUrl;
            this.Credentials = credentials;
            this.RecordUri = recordUri;
        }


        public string RecordsManagerUrl
        {
            get
            {
                return this.RecordDetails.RecordsManagerUrl;
            }
            set
            {
                this.RecordDetails.RecordsManagerUrl = value;
            }
        }


        public CookieContainer CookieContainer
        {
            get
            {
                return this.RecordDetails.CookieContainer;
            }
            set
            {
                this.RecordDetails.CookieContainer = value;
            }
        }


        public RecordsManagerCredentials Credentials
        {
            get
            {
                return this.RecordDetails.Credentials;
            }
            set
            {
                this.RecordDetails.Credentials = value;
            }
        }


        public string RecordUri
        {
            get
            {
                return this.RecordDetails.RecordUri;
            }
            set
            {
                this.RecordDetails.RecordUri = value;
            }
        }
    }
}