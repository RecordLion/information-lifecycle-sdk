using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RecordLion.RecordsManager.Client.Controls
{
    public class RecordDetailsModel : Model
    {
        private bool isBusy = false;
        private bool isLoaded = false;
        private Record record = null;
        private List<PhaseSummary> lifecycleSummary = null;
        private List<LegalHold> legalHolds = null;
        private Exception error = null;

        public RecordDetailsModel()
        {
        }


        public RecordsManagerClient Client { get; set; }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            private set
            {
                this.isBusy = value;
                this.OnPropertyChanged("IsBusy");
            }
        }


        public bool IsLoaded
        {
            get
            {
                return this.isLoaded;
            }
            private set
            {
                this.isLoaded = value;
                this.OnPropertyChanged("IsLoaded");
                this.OnPropertyChanged("ShowLifecycleSummary");
            }
        }


        public Exception Error
        {
            get
            {
                return this.error;
            }
            private set
            { 
                this.error = value;
                this.OnPropertyChanged("Error");
                this.OnPropertyChanged("ErrorAsString");
            }
        }


        public string ErrorAsString
        {
            get
            {
                string errorString = null;

                if (this.Error != null)
                {
                    errorString += this.Error.ToString();
                }

                return errorString;
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


        public List<PhaseSummary> LifecycleSummary
        {
            get
            {
                return this.lifecycleSummary;
            }
            private set
            {
                this.lifecycleSummary = value;
                this.OnPropertyChanged("LifecycleSummary");
                this.OnPropertyChanged("ShowLifecycleSummary");
            }
        }


        public bool ShowLifecycleSummary
        {
            get
            {
                return this.IsLoaded && this.LifecycleSummary != null && this.LifecycleSummary.Count > 0;
            }
        }


        public List<LegalHold> LegalHolds
        {
            get
            {
                return this.legalHolds;
            }
            private set
            {
                this.legalHolds = value;
                this.OnPropertyChanged("LegalHolds");
                this.OnPropertyChanged("ShowLegalHolds");
            }
        }


        public bool ShowLegalHolds
        {
            get
            {
                return this.IsLoaded && this.LegalHolds.Count > 0;
            }
        }


        public void Load(string uri)
        {
            this.Reset();

            if (this.Client != null && !string.IsNullOrEmpty(uri))
            {
                this.IsBusy = true;

                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        this.Record = this.Client.GetRecordByUri(uri);

                        if (this.Record != null)
                        {
                            this.LifecycleSummary = this.LoadLifecycleSummary(record);
                            this.LegalHolds = this.LoadLegalHolds(record);

                            this.IsLoaded = true;
                        }

                        this.IsBusy = false;
                    }
                    catch (Exception ex)
                    {
                        this.IsLoaded = false;
                        this.IsBusy = false;

                        this.Error = ex;
                    }
                });
            }
        }


        private List<PhaseSummary> LoadLifecycleSummary(Record record)
        {
            List<LifecyclePhaseSummary> summaries = this.Client.GetLifecyclesSummary(record.LifecycleId).ToList();

            List<PhaseSummary> lifecycleSummary = new List<PhaseSummary>();

            foreach(var phaseSummary in summaries.OrderBy(x => x.Order))
            {
                lifecycleSummary.Add(new PhaseSummary()
                {
                    DisplayOrder = string.Format("{0}. ", (phaseSummary.Order + 1)),
                    Summary = phaseSummary.Summary,
                    IsCurrent = (record.CurrentPhase.HasValue && record.CurrentPhase.Value == phaseSummary.Order),
                    IsCompleted = (record.CurrentPhase.HasValue && (record.CurrentPhase.Value > phaseSummary.Order || record.CurrentPhase.Value == -1))
                });
            }

            return lifecycleSummary;
        }


        private List<LegalHold> LoadLegalHolds(Record record)
        {
            return this.Client.SearchOpenLegalHolds(record.Uri).ToList();
        }

        
        public void Reset()
        {
            this.Error = null;
            this.IsLoaded = false;
            this.Record = null;
            this.LifecycleSummary = null;
        }

    }
}