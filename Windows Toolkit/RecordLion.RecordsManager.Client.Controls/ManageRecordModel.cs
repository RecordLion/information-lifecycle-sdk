using RecordLion.RecordsManager.Client.Controls.Utils;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;

namespace RecordLion.RecordsManager.Client.Controls
{
    public class ManageRecordModel : Model
    {
        private ManageRecordMode mode = ManageRecordMode.Details;
        private string recordsManagerUrl = string.Empty;
        private string recordUri = string.Empty;
        private bool isLoggedIn = false;
        private Record record = null;        
        private Exception error = null;


        public ManageRecordModel()
        {
        }


        public ManageRecordMode Mode 
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
                this.OnPropertyChanged("Mode");
                this.OnPropertyChanged("TargetUrl");
                this.OnPropertyChanged("TargetHeight");
                this.OnPropertyChanged("TargetWidth");
            }
        }


        public string RecordsManagerUrl
        {
            get
            {
                return this.recordsManagerUrl;
            }
            set
            {
                this.recordsManagerUrl = value;
                this.OnPropertyChanged("RecordsManagerUrl");
                this.OnPropertyChanged("TargetUrl");
            }
        }


        public string RecordUri
        {
            get
            {
                return this.recordUri;
            }
            set
            {
                this.recordUri = value;
                this.OnPropertyChanged("RecordUri");
                this.OnPropertyChanged("TargetUrl");
            }
        }


        public string TargetUrl
        {
            get
            {
                string url = this.RecordsManagerUrl.TrimEnd('/');

                switch(this.Mode)
                {
                    case ManageRecordMode.Audit:
                        return string.Format("{0}/audit/record?uri={1}&api=true", url, this.RecordUri);
                    case ManageRecordMode.Classification:
                        return string.Format("{0}/record/classifyrecordbyuri?uri={1}&api=true", url, this.RecordUri);
                    case ManageRecordMode.Declaration:
                        return string.Format("{0}/record/recorddeclarationbyuri?uri={1}&api=true", url, this.RecordUri);
                    case ManageRecordMode.Details:
                        return string.Format("{0}/record/detailsbyuri?uri={1}&api=true", url, this.RecordUri);
                    case ManageRecordMode.LegalHolds:
                        return string.Format("{0}/legalhold/createapi?uri={1}&api=true", url, this.RecordUri);
                    case ManageRecordMode.Request:
                        return string.Format("{0}/request/record?uri={1}&api=true", url, this.RecordUri);
                    case ManageRecordMode.Properties:
                        return string.Format("{0}/record/propertiesbyuri?uri={1}&api=true", url, this.RecordUri);
                }

                return null;
            }
        }


        public double TargetHeight
        {
            get
            {
                switch (this.Mode)
                {                    
                    case ManageRecordMode.Declaration:
                    case ManageRecordMode.LegalHolds:
                        return 365;
                    case ManageRecordMode.Classification:
                        return 485;
                    case ManageRecordMode.Audit:
                    case ManageRecordMode.Details:
                    case ManageRecordMode.Request:
                    case ManageRecordMode.Properties:
                    default:
                        return 650;
                }
            }
        }


        public double TargetWidth
        {
            get
            {
                switch (this.Mode)
                {
                    case ManageRecordMode.Audit:
                    case ManageRecordMode.Request:
                        return 800;                    
                    case ManageRecordMode.Details:
                        return 700;    
                    case ManageRecordMode.Classification:
                    case ManageRecordMode.LegalHolds:
                    case ManageRecordMode.Declaration:
                    case ManageRecordMode.Properties:
                    default:
                        return 600;
                }
            }
        }


        public Record Record
        {
            get
            {
                return this.record;
            }
            private set
            {
                this.record = value;
                this.OnPropertyChanged("Record");
            }
        }


        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }
            private set
            {
                this.isLoggedIn = value;
                this.OnPropertyChanged("IsLoggedIn");
            }
        }


        public Exception Error
        {
            get
            {
                return this.error;                    
            }
            set 
            {
                this.error = value;
                this.OnPropertyChanged("Error");
            }
        }
    }
}