using System;
using System.Collections.Generic;
using System.IO;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

namespace RecordLion.RecordsManager.Client
{
    public sealed class RecordsManagerClient : IRecordsManagerClient
    {
        private static JsonSerializerSettings incomingJsonSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };

        private static JsonSerializerSettings outgoingJsonSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        };

        private string baseUrl = string.Empty;
        private int timeout = 600000; //Default 10 minutes
        private RecordsManagerCredentials credentials = null;
        private CookieContainer cookieContainer = null;
        private ISecurityTokenRequestor securityTokenRequestor = null;
        private string issuer = string.Empty;
        private SecurityToken token = null;

        public RecordsManagerClient() : this(null, new RecordsManagerCredentials(), new SecurityTokenRequestorWsTrust())
        {
        }

        public RecordsManagerClient(string url) : this(url, new RecordsManagerCredentials(), new SecurityTokenRequestorWsTrust())
        {
        }

        public RecordsManagerClient(string url, RecordsManagerCredentials credentials) : this(url, credentials, new SecurityTokenRequestorWsTrust())
        {
        }

        public RecordsManagerClient(string url, RecordsManagerCredentials credentials, ISecurityTokenRequestor securityTokenRequestor)
        {
            this.Url = url;
            this.Credentials = credentials;
            this.SecurityTokenRequestor = securityTokenRequestor;
        }

        public RecordsManagerClient(string url, CookieContainer cookieContainer)
        {
            this.Url = url;
            this.CookieContainers = cookieContainer;
        }

        public string Url
        {
            get
            {
                return this.baseUrl;
            }
            set
            {
                this.baseUrl = value;
            }
        }

        public int Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                this.timeout = value;
            }
        }

        public RecordsManagerCredentials Credentials
        {
            get
            {
                return this.credentials;
            }
            set
            {
                this.credentials = value;
            }
        }

        public CookieContainer CookieContainers
        {
            get
            {
                return this.cookieContainer;
            }
            set
            {
                this.cookieContainer = value;
            }
        }

        public ISecurityTokenRequestor SecurityTokenRequestor
        {
            get
            {
                return this.securityTokenRequestor;
            }
            set
            {
                this.securityTokenRequestor = value;
            }
        }

        public string Issuer
        {
            get
            {
                return this.issuer;
            }
        }

        public SystemInfo GetSystemInfo()
        {
            using (var client = this.GetClient())
            {
                var response = client.DownloadString(new Uri(GET_SYSTEMINFO, UriKind.Relative));

                return JsonConvert.DeserializeObject<SystemInfo>(response);
            }
        }

        public string NewRMUID()
        {
            return this.Get<string>(GET_NEWRMUID.FormatResourceUrl());
        }

        #region Record Classes
        
        public string GetAllRecordClassesAsJson()
        {
            return this.GetAllRecordClassesAsJson(0, 0);
        }
        

        public string GetAllRecordClassesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDCLASSES_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        

        public string GetRecordClassesAsJson(long? parentId = null)
        {
            return this.GetRecordClassesAsJson(0, 0, parentId);
        }
        

        public string GetRecordClassesAsJson(int page, int pageSize, long? parentId = null)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDCLASSES.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty), UriKind.Relative));
            }
        }
        

        public string GetAllOpenRecordClassesAsJson()
        {
            return this.GetAllOpenRecordClassesAsJson(0, 0);
        }
        

        public string GetAllOpenRecordClassesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDCLASSES_ALL_OPEN.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        

        public string GetOpenRecordClassesAsJson(long? parentId = null)
        {
            return this.GetOpenRecordClassesAsJson(0, 0, parentId);
        }
        

        public string GetOpenRecordClassesAsJson(int page, int pageSize, long? parentId = null)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDCLASSES_OPEN.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty), UriKind.Relative));
            }
        }        
        

        public IEnumerable<RecordClass> GetRecordClassesFromJson(string json)
        {
            var page = this.GetRecordClassesWithPageDataFromJson(json);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetRecordClassesWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<RecordClass>>(json);
        }
        

        public DateTime GetRecordClassesLastEdit()
        {
            return this.Get<DateTime>(GET_RECORDCLASSES_LASTEDIT.FormatResourceUrl());
        }
        

        public IEnumerable<RecordClass> SearchRecordClasses(string titleOrCode)
        {
            var page = this.SearchRecordClasses(titleOrCode, 0, 0);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> SearchRecordClasses(string titleOrCode, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClass>>(GET_RECORDCLASSES_CONTAINING_TITLEORCODE.FormatResourceUrl(titleOrCode, page, pageSize));
        }
        

        public IEnumerable<RecordClass> GetAllRecordClasses()
        {
            var page = this.GetAllRecordClasses(0, 0);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetAllRecordClasses(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClass>>(GET_RECORDCLASSES_ALL.FormatResourceUrl(page, pageSize));
        }
        

        public IEnumerable<RecordClass> GetRecordClasses(long? parentId = null)
        {
            var page = this.GetRecordClasses(0, 0, parentId);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetRecordClasses(int page, int pageSize, long? parentId = null)
        {
            return this.Get<ClientPagedItems<RecordClass>>(GET_RECORDCLASSES.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }
        

        public IEnumerable<RecordClass> GetAllOpenRecordClasses()
        {
            var page = this.GetAllOpenRecordClasses(0, 0);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetAllOpenRecordClasses(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClass>>(GET_RECORDCLASSES_ALL_OPEN.FormatResourceUrl(page, pageSize));
        }
        

        public IEnumerable<RecordClass> GetOpenRecordClasses(long? parentId = null)
        {
            var page = this.GetOpenRecordClasses(0, 0, parentId);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetOpenRecordClasses(int page, int pageSize, long? parentId = null)
        {
            return this.Get<ClientPagedItems<RecordClass>>(GET_RECORDCLASSES_OPEN.FormatResourceUrl(page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }
        

        public RecordClass GetRecordClass(long id)
        {
            return this.Get<RecordClass>(GET_RECORDCLASS_WITH_ID.FormatResourceUrl(id));
        }
        

        public RecordClass GetRecordClass(string code)
        {
            return this.Get<RecordClass>(GET_RECORDCLASS_WITH_CODE.FormatResourceUrl(code));
        }
        

        public RecordClass CreateRecordClass(RecordClass recordClass)
        {
            return this.Post<RecordClass>(POST_RECORDCLASS.FormatResourceUrl(), recordClass);
        }


        public RecordClass CreateRecordClassCaseFile(RecordClass recordClass)
        {
            recordClass.IsCaseFile = true;

            return this.CreateRecordClass(recordClass);
        }
        

        public RecordClass UpdateRecordClass(RecordClass recordClass)
        {
            return this.Put<RecordClass>(PUT_RECORDCLASS.FormatResourceUrl(), recordClass);
        }
        

        public void DeleteRecordClass(long id)
        {
            this.Delete(DELETE_RECORDCLASS_WITH_ID.FormatResourceUrl(id));
        }

        public void ClearImportTables()
        {
            this.Delete(CLEAR_IMPORT_TABLES);
        }
        
        #endregion
        
        #region Records

        public DateTime GetRecordsLastEdit()
        {
            return this.Get<DateTime>(GET_RECORDS_LASTEDIT.FormatResourceUrl());
        }
            
        
        public string GetRecordsAsJson()
        {
            return this.GetRecordsAsJson(0, 0);
        }
            
        
        public string GetRecordsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }


        public string GetRecordsForRecordClassAsJson(long recordClassId)
        {
            return this.GetRecordsForRecordClassAsJson(recordClassId, 0, 0);
        }


        public string GetRecordsForRecordClassAsJson(long recordClassId, int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDS_FOR_RECORDCLASS.FormatResourceUrl(recordClassId, page, pageSize), UriKind.Relative));
            }
        }


        public IEnumerable<Record> GetRecordsFromJson(string json)
        {
            var page = this.GetRecordsWithPageDataFromJson(json);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<Record> GetRecordsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<Record>>(json);
        }
            
        
        public IEnumerable<Record> SearchRecords(string titleOrUri)
        {
            var page = this.SearchRecords(titleOrUri, 0, 0);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<Record> SearchRecords(string titleOrUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Record>>(GET_RECORDS_CONTAINING_TITLEORURI.FormatResourceUrl(titleOrUri, page, pageSize));
        }                 
            
        
        public IEnumerable<Record> GetRecords()
        {
            var page = this.GetRecords(0, 0);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<Record> GetRecords(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Record>>(GET_RECORDS_ALL.FormatResourceUrl(page, pageSize));
        }


        public IEnumerable<Record> GetRecordsForRecordClass(long recordClassId)
        {
            var page = this.GetRecordsForRecordClass(recordClassId, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<Record> GetRecordsForRecordClass(long recordClassId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Record>>(GET_RECORDS_FOR_RECORDCLASS.FormatResourceUrl(recordClassId, page, pageSize));
        }


        public Record GetRecord(long id)
        {
            return this.Get<Record>(GET_RECORD_WITH_ID.FormatResourceUrl(id));
        }
            
        
        public Record GetRecordByIdentifier(string identifier)
        {
            return this.Get<Record>(GET_RECORD_WITH_IDENTIFIER.FormatResourceUrl(identifier));
        }
            
        
        public Record GetRecordByUri(string uri)
        {
            return this.Get<Record>(GET_RECORD_WITH_URI.FormatResourceUrl(uri));
        }
            
        
        public void DeclareRecord(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION.FormatResourceUrl(uri), declaration);
        }
            
        
        public void UndeclareRecord(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION.FormatResourceUrl(uri), declaration);
        }
            
        
        public void DeclareVital(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION.FormatResourceUrl(uri), declaration);
        }
            
        
        public void UndeclareVital(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION.FormatResourceUrl(uri), declaration);
        }
            
        
        public void DeclareObsolete(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION.FormatResourceUrl(uri), declaration);
        }
            
        
        public void UndeclareObsolete(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION.FormatResourceUrl(uri), declaration);
        }
            
        
        public void DeclareRecordById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_ID.FormatResourceUrl(id), declaration);
        }
            
        
        public void UndeclareRecordById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_ID.FormatResourceUrl(id), declaration);
        }
            
        
        public void DeclareVitalById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_ID.FormatResourceUrl(id), declaration);
        }
            
        
        public void UndeclareVitalById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_ID.FormatResourceUrl(id), declaration);
        }
            
        
        public void DeclareObsoleteById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_ID.FormatResourceUrl(id), declaration);
        }
            
        
        public void UndeclareObsoleteById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_ID.FormatResourceUrl(id), declaration);
        }
            
        
        public void DeclareRecordByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_IDENTIFIER.FormatResourceUrl(identifier), declaration);
        }
            
        
        public void UndeclareRecordByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_IDENTIFIER.FormatResourceUrl(identifier), declaration);
        }
            
        
        public void DeclareVitalByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_IDENTIFIER.FormatResourceUrl(identifier), declaration);
        }
            
        
        public void UndeclareVitalByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_IDENTIFIER.FormatResourceUrl(identifier), declaration);
        }
            
        
        public void DeclareObsoleteByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Declare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_IDENTIFIER.FormatResourceUrl(identifier), declaration);
        }
            
        
        public void UndeclareObsoleteByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Undeclare
            };
                
            this.Put(PUT_RECORD_DECLARATION_WITH_IDENTIFIER.FormatResourceUrl(identifier), declaration);
        }

        #endregion

        #region Record Properties

        public IEnumerable<RecordProperty> GetRecordProperties(long recordId)
        {
            return this.Get<IEnumerable<RecordProperty>>(GET_RECORDPROPERTY_RECORD_ID.FormatResourceUrl(recordId));
        }

        #endregion

        #region Recordization

        public IEnumerable<Record> ProcessRecordization(IEnumerable<Recordize> recordizers)
        {
            return this.Post<IEnumerable<Record>>(POST_RECORDIZERS.FormatResourceUrl(), recordizers.ToArray());
        }

        
        public void DeleteRecordization(string recordizedUrl, bool deleteAll)
        {
            this.Delete(DELETE_RECORDIZERS.FormatResourceUrl(recordizedUrl, deleteAll));
        }

        #endregion
            
        #region Triggers

        public string GetTriggersAsJson()
        {
            return this.GetTriggersAsJson(0, 0);
        }
        

        public string GetTriggersAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_TRIGGERS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        
            
        public IEnumerable<RetentionTrigger> GetTriggersFromJson(string json)
        {
            var page = this.GetTriggersWithPageDataFromJson(json);
        
            return page.Items;
        }
        
        
        public IClientPagedItems<RetentionTrigger> GetTriggersWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<RetentionTrigger>>(json);
        }


        public DateTime GetTriggersLastEdit()
        {
            return this.Get<DateTime>(GET_TRIGGERS_LASTEDIT.FormatResourceUrl());
        }


        public IEnumerable<RetentionTrigger> SearchTriggers(string title)
        {
            var page = this.SearchTriggers(title, 0, 0);
        
            return page.Items;
        }
        
        
        public IClientPagedItems<RetentionTrigger> SearchTriggers(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RetentionTrigger>>(GET_TRIGGERS_CONTAINING_TITLE.FormatResourceUrl(title, page, pageSize));
        }


        public IEnumerable<RetentionTrigger> GetTriggers()
        {
            var page = this.GetTriggers(0, 0);
        
            return page.Items;
        }
        
        
        public IClientPagedItems<RetentionTrigger> GetTriggers(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RetentionTrigger>>(GET_TRIGGERS_ALL.FormatResourceUrl(page, pageSize));
        }


        public RetentionTrigger GetTrigger(long id)
        {
            return this.Get<RetentionTrigger>(GET_TRIGGER_WITH_ID.FormatResourceUrl(id));
        }


        public RetentionTrigger CreateTrigger(RetentionTrigger trigger)
        {
            return this.Post<RetentionTrigger>(POST_TRIGGER.FormatResourceUrl(), trigger);
        }


        public RetentionTrigger UpdateTrigger(RetentionTrigger trigger)
        {
            return this.Put<RetentionTrigger>(PUT_TRIGGER.FormatResourceUrl(), trigger);
        }


        public void DeleteTrigger(long id)
        {
            this.Delete(DELETE_TRIGGER_WITH_ID.FormatResourceUrl(id));
        }

        #endregion
        
        #region Retentions
        
        public string GetRetentionsAsJson()
        {
            return this.GetRetentionsAsJson(0, 0);
        }

        
        public string GetRetentionsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RETENTIONS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        
        
        public IEnumerable<Retention> GetRetentionsFromJson(string json)
        {
            var page = this.GetRetentionsWithPageDataFromJson(json);
            
            return page.Items;
        }

        
        public IClientPagedItems<Retention> GetRetentionsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<Retention>>(json);
        }
        

        public DateTime GetRetentionsLastEdit()
        {
            return this.Get<DateTime>(GET_RETENTIONS_LASTEDIT.FormatResourceUrl());
        }
        

        public IEnumerable<Retention> GetRetentions()
        {
            var page = this.GetRetentions(0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<Retention> GetRetentions(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Retention>>(GET_RETENTIONS_ALL.FormatResourceUrl(page, pageSize));
        }
        

        public IEnumerable<Retention> SearchRetentions(string titleOrAuthority)
        {
            var page = this.SearchRetentions(titleOrAuthority, 0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<Retention> SearchRetentions(string titleOrAuthority, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Retention>>(GET_RETENTIONS_CONTAINING_TITLEORAUTHORITY.FormatResourceUrl(titleOrAuthority, page, pageSize));
        }
        

        public Retention GetRetention(long id)
        {
            return this.Get<Retention>(GET_RETENTION_WITH_ID.FormatResourceUrl(id));
        }


        public Retention CreateRetention(Retention retention)
        {
            return this.Post<Retention>(POST_RETENTION.FormatResourceUrl(), retention);
        }


        public Retention UpdateRetention(Retention retention)
        {
            return this.Put<Retention>(PUT_RETENTION.FormatResourceUrl(), retention);
        }
        

        public void DeleteRetention(long id)
        {
            this.Delete(DELETE_RETENTION_WITH_ID.FormatResourceUrl(id));
        }
        
        #endregion
        
        #region Lifecycles
            
        public string GetLifecylesAsJson()
        {
            return this.GetLifecylesAsJson(0, 0);
        }


        public string GetLifecylesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_LIFECYCLES_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }

        
        public IEnumerable<Lifecycle> GetLifecylesFromJson(string json)
        {
            var page = this.GetLifecyclesWithPageDataFromJson(json);
                
            return page.Items;
        }


        public IClientPagedItems<Lifecycle> GetLifecyclesWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<Lifecycle>>(json);
        }
            
        
        public DateTime GetLifecyclesLastEdit()
        {
            return this.Get<DateTime>(GET_LIFECYCLES_LASTEDIT.FormatResourceUrl());
        }
            
        
        public IEnumerable<Lifecycle> SearchLifecycles(string title)
        {
            var page = this.SearchLifecycles(title, 0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<Lifecycle> SearchLifecycles(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Lifecycle>>(GET_LIFECYCLES_CONTAINING_TITLE.FormatResourceUrl(title, page, pageSize));
        }
            
        
        public IEnumerable<Lifecycle> GetLifecycles()
        {
            var page = this.GetLifecycles(0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<Lifecycle> GetLifecycles(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Lifecycle>>(GET_LIFECYCLES_ALL.FormatResourceUrl(page, pageSize));
        }
            
        
        public Lifecycle GetLifecycle(long id)
        {
            return this.Get<Lifecycle>(GET_LIFECYCLE_WITH_ID.FormatResourceUrl(id));
        }


        public List<LifecyclePhaseSummary> GetLifecyclesSummary(long id)
        {
            return this.Get<List<LifecyclePhaseSummary>>(GET_LIFECYCLE_SUMMARY_WITH_ID.FormatResourceUrl(id));
        }
            
        
        public Lifecycle CreateLifecycle(Lifecycle lifecycle)
        {
            return this.Post<Lifecycle>(POST_LIFECYCLE.FormatResourceUrl(), lifecycle);
        }
            
        
        public Lifecycle UpdateLifecycle(Lifecycle lifecycle)
        {
            return this.Put<Lifecycle>(PUT_LIFECYCLE.FormatResourceUrl(), lifecycle);
        }
            
        
        public void DeleteLifecycle(long id)
        {
            this.Delete(DELETE_LIFECYCLE_WITH_ID.FormatResourceUrl(id));
        }
            
        #endregion

        #region RecordClass Lifecycles
        
        public string GetRecordClassLifecyclesAsJson()
        {
            return this.GetRecordClassLifecyclesAsJson(0, 0);
        }
        

        public string GetRecordClassLifecyclesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RECORDCLASSLIFECYCLES_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }


        public IEnumerable<RecordClassLifecycle> GetRecordClassLifecyclesFromJson(string json)
        {
            var page = this.GetRecordClassLifecyclesWithPageDataFromJson(json);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClassLifecycle> GetRecordClassLifecyclesWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<RecordClassLifecycle>>(json);
        }

            
        public DateTime GetRecordClassLifecyclesLastEdit()
        {
            return this.Get<DateTime>(GET_RECORDCLASSLIFECYCLES_LASTEDIT.FormatResourceUrl());
        }
        
            
        public IEnumerable<RecordClassLifecycle> SearchRecordClassLifecycles(string title)
        {
            var page = this.SearchRecordClassLifecycles(title, 0, 0);
        
            return page.Items;
        }
        

        public IClientPagedItems<RecordClassLifecycle> SearchRecordClassLifecycles(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClassLifecycle>>(GET_RECORDCLASSLIFECYCLES_RECORDCLASSTITLEORLIFECYCLETITLE.FormatResourceUrl(title, page, pageSize));
        }

            
        public IEnumerable<RecordClassLifecycle> GetRecordClassLifecycles()
        {
            var page = this.GetRecordClassLifecycles(0, 0);
        
            return page.Items;
        }
        

        public IClientPagedItems<RecordClassLifecycle> GetRecordClassLifecycles(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClassLifecycle>>(GET_RECORDCLASSLIFECYCLES_ALL.FormatResourceUrl(page, pageSize));
        }

            
        public RecordClassLifecycle GetRecordClassLifecycle(long id)
        {
            return this.Get<RecordClassLifecycle>(GET_RECORDCLASSLIFECYCLE_WITH_ID.FormatResourceUrl(id));
        }

        public RecordClassLifecycle GetRecordClassLifecycleForRecordClass(long recordClassId)
        {
            return this.Get<RecordClassLifecycle>(GET_RECORDCLASSLIFECYCLE_WITH_RECORDCLASSID.FormatResourceUrl(recordClassId));
        }


        public RecordClassLifecycle CreateRecordClassLifecycle(RecordClassLifecycle lifecycle)
        {
            return this.Post<RecordClassLifecycle>(POST_RECORDCLASSLIFECYCLE.FormatResourceUrl(), lifecycle);
        }
        
            
        public RecordClassLifecycle UpdateRecordClassLifecycle(RecordClassLifecycle lifecycle)
        {
            return this.Put<RecordClassLifecycle>(PUT_RECORDCLASSLIFECYCLE.FormatResourceUrl(), lifecycle);
        }
        
            
        public void DeleteRecordClassLifecycle(long id)
        {
            this.Delete(DELETE_RECORDCLASSLIFECYCLE_WITH_ID.FormatResourceUrl(id));
        }
        
        #endregion

        #region Event Occurrences
        
        public string GetEventOccurrencesAsJson()
        {
            return this.GetEventOccurrencesAsJson(0, 0);
        }

        
        public string GetEventOccurrencesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_EVENTOCCURRENCES_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        

        public IEnumerable<EventOccurrence> GetEventOccurrencesFromJson(string json)
        {
            var page = this.GetEventOccurrencesWithPageDataFromJson(json);
            
            return page.Items;
        }
            
        
        public IClientPagedItems<EventOccurrence> GetEventOccurrencesWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<EventOccurrence>>(json);
        }
            

        public DateTime GetEventOccurrencesLastEdit()
        {
            return this.Get<DateTime>(GET_EVENTOCCURRENCES_LASTEDIT.FormatResourceUrl());
        }
        
        
        public IEnumerable<EventOccurrence> SearchEventOccurrences(string eventTitle)
        {
            var page = this.SearchEventOccurrences(eventTitle, 0, 0);

            return page.Items;
        }
            
        
        public IClientPagedItems<EventOccurrence> SearchEventOccurrences(string eventTitle, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<EventOccurrence>>(GET_EVENTOCCURRENCES_CONTAINING_EVENTTITLE.FormatResourceUrl(eventTitle, page, pageSize));
        }
            

        public IEnumerable<EventOccurrence> GetEventOccurrences()
        {
            var page = this.GetEventOccurrences(0, 0);

            return page.Items;
        }
            
        
        public IClientPagedItems<EventOccurrence> GetEventOccurrences(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<EventOccurrence>>(GET_EVENTOCCURRENCES_ALL.FormatResourceUrl(page, pageSize));
        }
            

        public EventOccurrence GetEventOccurrences(long id)
        {
            return this.Get<EventOccurrence>(GET_EVENTOCCURRENCE_WITH_ID.FormatResourceUrl(id));
        }
        
        
        public EventOccurrence CreateEventOccurrence(EventOccurrence eventOccurrence)
        {
            return this.Post<EventOccurrence>(POST_EVENTOCCURRENCE.FormatResourceUrl(), eventOccurrence);
        }
        
        
        public void DeleteEventOccurrence(long id)
        {
            this.Delete(DELETE_EVENTOCCURRENCE_WITH_ID.FormatResourceUrl(id));
        }
        
        #endregion        
        
        #region Audit

        public string GetAuditsAsJson()
        {
            return this.GetAuditsAsJson(0, 0);
        }


        public string GetAuditsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_AUDIT_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
            
        
        public IEnumerable<AuditEntry> GetAuditsFromJson(string json)
        {
            var page = this.GetAuditsWithPageDataFromJson(json);
        
            return page.Items;
        }
                
            
        public IClientPagedItems<AuditEntry> GetAuditsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<AuditEntry>>(json);
        }
        
            
        public DateTime GetAuditsLastEdit()
        {
            return this.Get<DateTime>(GET_AUDIT_LASTEDIT.FormatResourceUrl());
        }

        
        public IEnumerable<AuditEntry> SearchAudits(DateTime rangeStart, DateTime rangeEnd)
        {
            var page = this.SearchAudits(rangeStart, rangeEnd, 0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> SearchAudits(DateTime rangeStart, DateTime rangeEnd, int page, int pageSize)
        {
            string startDateString = rangeStart.ToString("s", CultureInfo.InvariantCulture);
            string endDateString = rangeEnd.ToString("s", CultureInfo.InvariantCulture);
            return this.Get<ClientPagedItems<AuditEntry>>(GET_AUDIT_IN_RANGE.FormatResourceUrl(startDateString, endDateString, page, pageSize));
        }
        
            
        public IEnumerable<AuditEntry> GetAudits()
        {
            var page = this.GetAudits(0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> GetAudits(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(GET_AUDIT_ALL.FormatResourceUrl(page, pageSize));
        }
        
            
        public IEnumerable<AuditEntry> GetAudits(AuditTarget target, long targetId)
        {
            var page = this.GetAudits(target, targetId, 0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> GetAudits(AuditTarget target, long targetId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(GET_AUDIT_FOR_TARGET.FormatResourceUrl(target, targetId, page, pageSize));
        }
        
            
        public IEnumerable<AuditEntry> GetAuditsForRecord(string recordUri)
        {
            var page = this.GetAuditsForRecord(recordUri, 0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> GetAuditsForRecord(string recordUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(GET_AUDIT_FOR_RECORDURI.FormatResourceUrl(recordUri, page, pageSize));
        }
        
            
        public AuditEntry CreateAudit(NewAuditEntry auditEntry)
        {
            return this.Post<AuditEntry>(POST_AUDIT.FormatResourceUrl(), auditEntry);
        }

        #endregion
            
        #region Heartbeat

        public HeartbeatResult Heartbeat(Heartbeat heartbeat)
        {
            return this.Heartbeat(heartbeat, false);
        }
        

        public HeartbeatResult Heartbeat(Heartbeat heartbeat, bool supportsFailover)
        {
            return this.Post<HeartbeatResult>(POST_HEARTBEAT.FormatResourceUrl(supportsFailover), heartbeat);
        }
        
        #endregion
        
        #region Legal Cases
        
        public string GetLegalCasesAsJson()
        {
            return this.GetLegalCasesAsJson(0, 0);
        }
            
        
        public string GetLegalCasesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_LEGALCASES_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        
        
        public string GetOpenLegalCasesAsJson()
        {
            return this.GetOpenLegalCasesAsJson(0, 0);
        }
        
        
        public string GetOpenLegalCasesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_LEGALCASES_OPEN.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }
        
        
        public IEnumerable<LegalCase> GetLegalCasesFromJson(string json)
        {
            var page = this.GetLegalCasesWithPageDataFromJson(json);

            return page.Items;
        }
            
            
        public IClientPagedItems<LegalCase> GetLegalCasesWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<LegalCase>>(json);
        }

        
        public DateTime GetLegalCasesLastEdit()
        {
            return this.Get<DateTime>(GET_LEGALCASES_LASTEDIT.FormatResourceUrl());
        }
        

        public IEnumerable<LegalCase> SearchLegalCases(string titleOrNumber)
        {
            var page = this.SearchLegalCases(titleOrNumber, 0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<LegalCase> SearchLegalCases(string titleOrNumber, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalCase>>(GET_LEGALCASES_CONTAINING_TITLEORNUMBER.FormatResourceUrl(titleOrNumber, page, pageSize));
        }

        
        public IEnumerable<LegalCase> GetLegalCases()
        {
            var page = this.GetLegalCases(0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<LegalCase> GetLegalCases(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalCase>>(GET_LEGALCASES_ALL.FormatResourceUrl(page, pageSize));
        }

        
        public IEnumerable<LegalCase> GetOpenLegalCases()
        {
            var page = this.GetOpenLegalCases(0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<LegalCase> GetOpenLegalCases(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalCase>>(GET_LEGALCASES_OPEN.FormatResourceUrl(page, pageSize));
        }

        
        public LegalCase GetLegalCase(long id)
        {
            return this.Get<LegalCase>(GET_LEGALCASES_WITH_ID.FormatResourceUrl(id));
        }
        

        public LegalCase CreateLegalCase(LegalCase legalCase)
        {
            return this.Post<LegalCase>(POST_LEGALCASES.FormatResourceUrl(), legalCase);
        }
        

        public LegalCase UpdateLegalCase(LegalCase legalCase)
        {
            return this.Put<LegalCase>(PUT_LEGALCASES.FormatResourceUrl(), legalCase);
        }
        

        public void DeleteLegalCase(long id)
        {
            this.Delete(DELETE_LEGALCASES_WITH_ID.FormatResourceUrl(id));
        }
        
        #endregion
        
        #region Legal Holds
            
        public string GetLegalHoldsAsJson()
        {
            return this.GetLegalHoldsAsJson(0, 0);
        }
        
            
        public string GetLegalHoldsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_LEGALHOLDS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }

        
        public string GetOpenLegalHoldsAsJson()
        {
            return this.GetOpenLegalHoldsAsJson(0, 0);
        }

        
        public string GetOpenLegalHoldsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_LEGALHOLDS_OPEN.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }

        
        public IEnumerable<LegalHold> GetLegalHoldsFromJson(string json)
        {
            var page = this.GetLegalHoldsWithPageDataFromJson(json);

            return page.Items;
        }
        
            
        public IClientPagedItems<LegalHold> GetLegalHoldsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<LegalHold>>(json);
        }


        public IEnumerable<LegalHold> GetLegalHolds()
        {
            var page = this.GetLegalHolds(0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> GetLegalHolds(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(GET_LEGALHOLDS_ALL.FormatResourceUrl(page, pageSize));
        }


        public IEnumerable<LegalHold> GetOpenLegalHolds()
        {
            var page = this.GetOpenLegalHolds(0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> GetOpenLegalHolds(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(GET_LEGALHOLDS_OPEN.FormatResourceUrl(page, pageSize));
        }


        public IEnumerable<LegalHold> SearchLegalHolds(string uri)
        {
            var page = this.SearchLegalHolds(uri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> SearchLegalHolds(string uri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(GET_LEGALHOLDS_CONTAINING_URI.FormatResourceUrl(uri, page, pageSize));
        }


        public IEnumerable<LegalHold> SearchOpenLegalHolds(string uri)
        {
            var page = this.SearchOpenLegalHolds(uri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> SearchOpenLegalHolds(string uri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(GET_LEGALHOLDS_OPEN_CONTAINING_URI.FormatResourceUrl(uri, page, pageSize));
        }


        public IEnumerable<LegalHold> SearchLegalHolds(long legalCaseId)
        {
            var page = this.SearchLegalHolds(legalCaseId, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> SearchLegalHolds(long legalCaseId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(GET_LEGALHOLDS_CONTAINING_LEGALCASEID.FormatResourceUrl(legalCaseId, page, pageSize));
        }


        public LegalHold GetLegalHoldById(long id)
        {
            return this.Get<LegalHold>(GET_LEGALHOLD_WITH_ID.FormatResourceUrl(id));
        }
            
        
        public LegalHold CreateLegalHold(LegalHold legalHold)
        {
            return this.Post<LegalHold>(POST_LEGALHOLD.FormatResourceUrl(), legalHold);
        }
            
        
        public LegalHold UpdateLegalHold(LegalHold legalHold)
        {
            return this.Put<LegalHold>(PUT_LEGALHOLD.FormatResourceUrl(), legalHold);
        }
            
        
        public void DeleteLegalHold(long legalCaseId, string uri)
        {
            this.Delete(DELETE_LEGALHOLD_FOR_CASE_WITH_URI.FormatResourceUrl(legalCaseId, uri));
        }
            
        
        public void DeleteLegalHold(long id)
        {
            this.Delete(DELETE_LEGALHOLD_WITH_ID.FormatResourceUrl(id));
        }
            
        #endregion

        #region Action Items
        
        public DateTime GetActionItemsLastEdit()
        {
            return this.Get<DateTime>(GET_ACTIONITEMS_LASTEDIT.FormatResourceUrl());
        }
        
        
        public IEnumerable<ActionItem> GetActionItemsFromJson(string json)
        {
            var page = this.GetActionItemsWithPageDataFromJson(json);

            return page.Items;
        }

        
        public IClientPagedItems<ActionItem> GetActionItemsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<ActionItem>>(json);
        }
        

        public ActionItem GetActionItem(long id)
        {
            return this.Get<ActionItem>(GET_ACTIONITEM_WITH_ID.FormatResourceUrl(id));
        }

            
        public ActionItem GetActionItemForRecord(long recordId)
        {
            return this.Get<ActionItem>(GET_ACTIONITEM_FOR_RECORDID.FormatResourceUrl(recordId));
        }
        
            
        public ActionItem GetActionItemForRecord(string identifier)
        {
            return this.Get<ActionItem>(GET_ACTIONITEM_FOR_RECORDIDENTIFIER.FormatResourceUrl(identifier));
        }
        
            
        public ActionItem UpdateActionItem(ActionItem actionItem)
        {
            return this.Put<ActionItem>(PUT_ACTIONITEM.FormatResourceUrl(), actionItem);
        }
        
        #region Pending Action Items
        
        public string GetPendingActionItemsAsJson()
        {
            return this.GetPendingActionItemsAsJson(0, 0);
        }
            
        
        public string GetPendingActionItemsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_PENDING_ACTIONITEMS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }

        
        public IEnumerable<ActionItem> SearchPendingActionItemsByTitleOrUri(string recordTitleOrUri)
        {
            var page = this.SearchPendingActionItemsByTitleOrUri(recordTitleOrUri, 0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<ActionItem> SearchPendingActionItemsByTitleOrUri(string recordTitleOrUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(GET_PENDING_ACTIONITEMS_CONTAINING_RECORDTITLEORURI.FormatResourceUrl(recordTitleOrUri, page, pageSize));
        }


        public IEnumerable<ActionItem> SearchPendingActionItemsByUri(string recordUri)
        {
            var page = this.SearchPendingActionItemsByUri(recordUri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<ActionItem> SearchPendingActionItemsByUri(string recordUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(GET_PENDING_ACTIONITEMS_CONTAINING_RECORDURI.FormatResourceUrl(recordUri, page, pageSize));
        }


        public IEnumerable<ActionItem> SearchPendingSystemActionItemsByUri(string recordUri)
        {
            var page = this.SearchPendingSystemActionItemsByUri(recordUri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<ActionItem> SearchPendingSystemActionItemsByUri(string recordUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(GET_PENDING_SYSTEM_ACTIONITEMS_CONTAINING_RECORDURI.FormatResourceUrl(recordUri, page, pageSize));
        }

        
        public IEnumerable<ActionItem> GetPendingActionItems()
        {
            var page = this.GetPendingActionItems(0, 0);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<ActionItem> GetPendingActionItems(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(GET_PENDING_ACTIONITEMS_ALL.FormatResourceUrl(page, pageSize));
        }
            
        
        public ActionItem UpdatePendingActionItemCompleted(long id)
        {
            return this.Put<ActionItem>(PUT_PENDING_ACTIONITEM_COMPLETED.FormatResourceUrl(id), id);
        }
            

        public ActionItem UpdatePendingActionItemFailed(long id)
        {
            return this.Put<ActionItem>(PUT_PENDING_ACTIONITEM_FAILED.FormatResourceUrl(id), id);
        }
        
        
        public ActionItem UpdatePendingActionItemUnsupported(long id)
        {
            return this.Put<ActionItem>(PUT_PENDING_ACTIONITEM_UNSUPPORTED.FormatResourceUrl(id), id);
        }
        
        #endregion
            
        
        #region Inbox Action Items

        public string GetInboxActionItemsAsJson()
        {
            return this.GetInboxActionItemsAsJson(0, 0);
        }


        public string GetInboxActionItemsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_INBOX_ACTIONITEMS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }

        public IEnumerable<InboxActionItem> GetInboxActionItemsFromJson(string json)
        {
            var page = this.GetInboxActionItemsWithPageDataFromJson(json);

            return page.Items;
        }


        public IClientPagedItems<InboxActionItem> GetInboxActionItemsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<InboxActionItem>>(json);
        }


        public IEnumerable<InboxActionItem> SearchInboxActionItems(string title)
        {
            var page = this.SearchInboxActionItems(title, 0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<InboxActionItem> SearchInboxActionItems(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<InboxActionItem>>(GET_INBOX_ACTIONITEMS_CONTAINING_TITLE.FormatResourceUrl(title, page, pageSize));
        }


        public IEnumerable<InboxActionItem> GetInboxActionItems()
        {
            var page = this.GetInboxActionItems(0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<InboxActionItem> GetInboxActionItems(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<InboxActionItem>>(GET_INBOX_ACTIONITEMS_ALL.FormatResourceUrl(page, pageSize));
        }


        public IClientPagedItems<ActionItem> GetInboxActionItemsForCase(InboxActionItem caseItem, int page, int pageSize)
        {
            if (!caseItem.IsCaseFile)
                return null;

            return this.Get<ClientPagedItems<ActionItem>>(GET_INBOX_ACTIONITEMS_CASE.FormatResourceUrl(caseItem.Id, page, pageSize));
        }
            

        public void UpdateInboxActionItemApprove(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_APPROVE.FormatResourceUrl(id), id);
        }


        public void UpdateInboxActionItemDismiss(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_DISMISS.FormatResourceUrl(id), id);
        }


        public void UpdateInboxActionItemRetry(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_RETRY.FormatResourceUrl(id), id);
        }


        public void UpdateInboxActionItemCompleted(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_COMPLETED.FormatResourceUrl(id), id);
        }


        public void UpdateInboxCaseApprove(InboxActionItem caseItem)
        {
            this.Put(PUT_INBOX_ACTIONITEMS_APPROVE_CASE.FormatResourceUrl(caseItem.Id), null);
        }


        public void UpdateInboxCaseDismiss(InboxActionItem caseItem)
        {
            this.Put(PUT_INBOX_ACTIONITEMS_DISMISS_CASE.FormatResourceUrl(caseItem.Id), null);
        }


        public void UpdateInboxCaseRetry(InboxActionItem caseItem)
        {
            this.Put(PUT_INBOX_ACTIONITEMS_RETRY_CASE.FormatResourceUrl(caseItem.Id), null);
        }


        #endregion
        

        #endregion

        #region Managed Properties

        public string GetManagedPropertiesAsJson()
        {
            return this.GetManagedPropertiesAsJson(0, 0);
        }


        public string GetManagedPropertiesAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_MANAGEDPROPERTIES_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }


        public IEnumerable<ManagedProperty> GetManagedPropertiesFromJson(string json)
        {
            var page = this.GetManagedPropertiesWithPageDataFromJson(json);

            return page.Items;
        }


        public IClientPagedItems<ManagedProperty> GetManagedPropertiesWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<ManagedProperty>>(json);
        }


        public DateTime GetManagedPropertiesLastEdit()
        {
            return this.Get<DateTime>(GET_MANAGEDPROPERTIES_LASTEDIT.FormatResourceUrl());
        }


        public IEnumerable<ManagedProperty> SearchManagedProperties(string name)
        {
            var page = this.SearchManagedProperties(name, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<ManagedProperty> SearchManagedProperties(string name, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ManagedProperty>>(GET_MANAGEDPROPERTIES_CONTAINING_NAME.FormatResourceUrl(name, page, pageSize));
        }


        public IEnumerable<ManagedProperty> GetManagedProperties()
        {
            var page = this.GetManagedProperties(0, 0);

            return page.Items;
        }


        public IClientPagedItems<ManagedProperty> GetManagedProperties(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ManagedProperty>>(GET_MANAGEDPROPERTIES_ALL.FormatResourceUrl(page, pageSize));
        }


        public ManagedProperty GetManagedProperty(long id)
        {
            return this.Get<ManagedProperty>(GET_MANAGEDPROPERTY_WITH_ID.FormatResourceUrl(id));
        }


        public ManagedProperty CreateManagedProperty(ManagedProperty property)
        {
            return this.Post<ManagedProperty>(POST_MANAGEDPROPERTY.FormatResourceUrl(), property);
        }


        public ManagedProperty UpdateManagedProperty(ManagedProperty property)
        {
            return this.Put<ManagedProperty>(PUT_MANAGEDPROPERTY.FormatResourceUrl(), property);
        }


        public void DeleteManagedProperty(long id)
        {
            this.Delete(DELETE_MANAGEDPROPERTY_WITH_ID.FormatResourceUrl(id));
        }

        #endregion

        #region Rule Sets

        public string GetRuleSetsAsJson()
        {
            return this.GetRuleSetsAsJson(0, 0);
        }


        public string GetRuleSetsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(GET_RULESETS_ALL.FormatResourceUrl(page, pageSize), UriKind.Relative));
            }
        }


        public IEnumerable<RuleSet> GetRuleSetsFromJson(string json)
        {
            var page = this.GetRuleSetsWithPageDataFromJson(json);

            return page.Items;
        }


        public IClientPagedItems<RuleSet> GetRuleSetsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<RuleSet>>(json);
        }


        public DateTime GetRuleSetsLastEdit()
        {
            return this.Get<DateTime>(GET_RULESETS_LASTEDIT.FormatResourceUrl());
        }


        public IEnumerable<RuleSet> SearchRuleSets(string title)
        {
            var page = this.SearchRuleSets(title, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<RuleSet> SearchRuleSets(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RuleSet>>(GET_RULESETS_CONTAINING_TITLE.FormatResourceUrl(title, page, pageSize));
        }


        public IEnumerable<RuleSet> GetRuleSets()
        {
            var page = this.GetRuleSets(0, 0);

            return page.Items;
        }


        public IClientPagedItems<RuleSet> GetRuleSets(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RuleSet>>(GET_RULESETS_ALL.FormatResourceUrl(page, pageSize));
        }


        public RuleSet GetRuleSet(long id)
        {
            return this.Get<RuleSet>(GET_RULESET_WITH_ID.FormatResourceUrl(id));
        }


        public RuleSet CreateRuleSet(RuleSet ruleSet)
        {
            return this.Post<RuleSet>(POST_RULESET.FormatResourceUrl(), ruleSet);
        }


        public RuleSet UpdateRuleSet(RuleSet ruleSet)
        {
            return this.Put<RuleSet>(PUT_RULESET.FormatResourceUrl(), ruleSet);
        }


        public void DeleteRuleSet(long id)
        {
            this.Delete(DELETE_RULESET_WITH_ID.FormatResourceUrl(id));
        }

        #endregion

        #region Private

        private WebClient GetClient()
        {
            WebClientWithCookies client = new WebClientWithCookies(this.Timeout);

            client.BaseAddress = this.baseUrl;
            client.Headers.Add(HttpRequestHeader.ContentType, HEADER_CONTENTTYPE);
        
            if (this.cookieContainer != null)
                client.CookieContainer = this.cookieContainer;
            else
                client.Headers.Add(HttpRequestHeader.Authorization, this.GetAuthorizationHeaderValue());
        
            return client;
        }
        
        public string Get(string resourceUrl)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(resourceUrl, UriKind.Relative));
            }
        }

        public T Get<T>(string resourceUrl)
        {
            using (var client = this.GetClient())
            {
                var response = client.DownloadString(new Uri(resourceUrl, UriKind.Relative));
                return JsonConvert.DeserializeObject<T>(response, incomingJsonSettings);
            }
        }
        
        public T Post<T>(string resourceUrl, object data)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    this.SetDateTimeKindsToLocal(data);

                    var request = JsonConvert.SerializeObject(data, outgoingJsonSettings);
                    var response = client.UploadString(new Uri(resourceUrl, UriKind.Relative), "POST", request);
            
                    return JsonConvert.DeserializeObject<T>(response, incomingJsonSettings);
                }
            }
            catch (WebException ex)
            {
                var updateException = this.ConvertValidationDetailsToUpdateException(ex);

                if (updateException != null)
                    throw updateException;
            
                throw;
            }
        }
                    
        
        public T Put<T>(string resourceUrl, object data)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    this.SetDateTimeKindsToLocal(data);
            
                    var request = JsonConvert.SerializeObject(data, outgoingJsonSettings);
                    var response = client.UploadString(new Uri(resourceUrl, UriKind.Relative), "PUT", request);
                
                    return JsonConvert.DeserializeObject<T>(response, incomingJsonSettings);
                }
            }
            catch (WebException ex)
            {
                var updateException = this.ConvertValidationDetailsToUpdateException(ex);

                if (updateException != null)
                    throw updateException;
            
                throw;
            }
        }
                    
        
        private void Put(string resourceUrl, object data)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    this.SetDateTimeKindsToLocal(data);
            
                    var request = JsonConvert.SerializeObject(data, outgoingJsonSettings);
                    client.UploadString(new Uri(resourceUrl, UriKind.Relative), "PUT", request);
                }
            }
            catch (WebException ex)
            {
                var updateException = this.ConvertValidationDetailsToUpdateException(ex);
        
                if (updateException != null)
                    throw updateException;
        
                throw;
            }
        }
                
                
        public void Delete(string resourceUrl)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    client.UploadData(new Uri(resourceUrl, UriKind.Relative), "DELETE", new byte[0]);
                }
            }
            catch (WebException ex)
            {
                var updateException = this.ConvertValidationDetailsToUpdateException(ex);
                
                if (updateException != null)
                    throw updateException;
        
                throw;
            }
        }
        
            
        private string GetAuthorizationHeaderValue()
        {
            if (this.credentials != null)
            {
                switch (this.credentials.CredentialType)
                {
                    case RecordsManagerCredentialType.Forms:
                        return GetBasicAuthorizationHeaderValue();
                    case RecordsManagerCredentialType.Claims:
                        return GetClaimsAuthorizationHeaderValue();
                }
            }
            
            return string.Empty;
        }
        
        
        private string GetBasicAuthorizationHeaderValue()
        {
            if (credentials != null)
            {
                var buffer = Encoding.ASCII.GetBytes($"{this.credentials.Username}:{this.credentials.Password}");
                return $"Basic {Convert.ToBase64String(buffer)}";
            }
                    
            return string.Empty;
        }
                        
                
        private string GetClaimsAuthorizationHeaderValue()
        {
            if (string.IsNullOrEmpty(this.issuer))
            {
                using (var client = new WebClient())
                {
                    client.BaseAddress = this.baseUrl;
        
                    var response = client.DownloadString(new Uri(GET_WSTRUST_ADDRESS, UriKind.Relative));
                    this.issuer = JsonConvert.DeserializeObject<string>(response);
                }
            }
            
            if (this.token == null || this.token.ExpiresOn < DateTime.Now)
                this.token = this.securityTokenRequestor.RequestToken(this.issuer, this.baseUrl, this.credentials);
        
            if (this.token != null)
            {
                var buffer = Encoding.ASCII.GetBytes(token.Token);
                return $"{this.token.TokenType} {Convert.ToBase64String(buffer)}";
            }
            
            return string.Empty;
        }
                    
        
        private UpdateException ConvertValidationDetailsToUpdateException(WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            
            if (response != null)
            {
                UpdateException updateException = new UpdateException(response.StatusDescription);
                
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    if (response.Headers["RM-Validation-Details"] == bool.TrueString)
                    {
                        updateException.ErrorDetail = "Retrieve the Errors property for for a list of the update errors.";
                        
                        ProcessRMValidationDetails(updateException, sr);
                    }
                    else
                    {
                        updateException.ErrorDetail = sr.ReadToEnd();
                    }
                }
                
                return updateException;
            }
                
            return null;
        }
                
                    
        private static void ProcessRMValidationDetails(UpdateException updateException, StreamReader sr)
        {
            string content = sr.ReadLine();
                        
            while (content != null)
            {
                //We skip the first line by reading the line again
                content = sr.ReadLine();
                    
                if (!string.IsNullOrEmpty(content))
                {
                    string[] parts = content.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
            
                    string message = parts[0];
            
                    if (parts.Length > 1)
                    {
                        string memberValue = parts[1];
        
                        if (!string.IsNullOrEmpty(memberValue))
                        {
                            string[] members = memberValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
                            foreach (var member in members)
                            {
                                updateException.Errors.Add(new UpdateError(member, message));
                            }
                        }
                    }
                    else
                        updateException.Errors.Add(new UpdateError(message));
                }
            }
        }
                    
                        
        private void SetDateTimeKindsToLocal(object data)
        {
            foreach (var property in data.GetType()
                                         .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         .Where(x => x.PropertyType == typeof(DateTime) ||
                                                     x.PropertyType == typeof(DateTime?)))
            {
                var getMethod = property.GetGetMethod();
                var setMethod = property.GetSetMethod();
                        
                if (getMethod != null && setMethod != null)
                {
                    DateTime? currentValue = getMethod.Invoke(data, null) as DateTime?;
                
                    if (currentValue.HasValue && currentValue.Value.Kind == DateTimeKind.Unspecified)
                    {
                        setMethod.Invoke(data, new object[] { DateTime.SpecifyKind(currentValue.Value, DateTimeKind.Local) });
                    }
                }
            }
        }                     

                                  
        #endregion
            
        #region Constants
                
        private const string HEADER_CONTENTTYPE = "application/json";
                
        private const string GET_WSTRUST_ADDRESS = "/api/v1/trust";
        
        private const string GET_SYSTEMINFO = "/api/system";
                    
        private const string GET_NEWRMUID = "/api/v1/rmuid";
                    
        private const string GET_RECORDCLASSES_LASTEDIT = "/api/v1/recordclasses?lastedit";
        private const string GET_RECORDCLASSES = "/api/v1/recordclasses?page={0}&pageSize={1}&parentId={2}";
        private const string GET_RECORDCLASSES_ALL = "/api/v1/recordclasses?all=true&page={0}&pageSize={1}";
        private const string GET_RECORDCLASSES_CONTAINING_TITLEORCODE = "/api/v1/recordclasses?titleOrCode={0}&page={1}&pageSize={2}";
        private const string GET_RECORDCLASSES_OPEN = "/api/v1/recordclasses?&openOnly=true&page={0}&pageSize={1}&parentId={2}";
        private const string GET_RECORDCLASSES_ALL_OPEN = "/api/v1/recordclasses?all=true&openOnly=true&page={0}&pageSize={1}";
        private const string GET_RECORDCLASS_WITH_ID = "/api/v1/recordclasses/{0}";
        private const string GET_RECORDCLASS_WITH_CODE = "/api/v1/recordclasses?code={0}";
        private const string POST_RECORDCLASS = "/api/v1/recordclasses";
        private const string PUT_RECORDCLASS = "/api/v1/recordclasses";
        private const string DELETE_RECORDCLASS_WITH_ID = "/api/v1/recordclasses/{0}";
        private const string CLEAR_IMPORT_TABLES = "/api/v1/import";
        
        private const string GET_RECORDS_LASTEDIT = "/api/v1/records?lastedit";
        private const string GET_RECORDS_ALL = "/api/v1/records?page={0}&pageSize={1}";        
        private const string GET_RECORDS_CONTAINING_TITLEORURI = "/api/v1/records?titleOrUri={0}&page={1}&pageSize={2}";
        private const string GET_RECORDS_FOR_RECORDCLASS = "/api/v1/records?classId={0}&page={1}&pageSize={2}";
        private const string GET_RECORD_WITH_ID = "/api/v1/records/{0}";
        private const string GET_RECORD_WITH_IDENTIFIER = "/api/v1/records?identifier={0}";
        private const string GET_RECORD_WITH_URI = "/api/v1/records?uri={0}";
        private const string PUT_RECORD_DECLARATION = "/api/v1/records?uri={0}";
        private const string PUT_RECORD_DECLARATION_WITH_ID = "/api/v1/records?id={0}";
        private const string PUT_RECORD_DECLARATION_WITH_IDENTIFIER = "/api/v1/records?identifier={0}";

        private const string GET_RECORDPROPERTY_RECORD_ID = "/api/v1/recordproperty?recordId={0}";

        private const string POST_RECORDIZERS = "/api/v1/recordization";
        private const string DELETE_RECORDIZERS = "/api/v1/recordization?uri={0}&all={1}";

        private const string GET_TRIGGERS_LASTEDIT = "/api/v1/triggers?lastedit";
        private const string GET_TRIGGERS_ALL = "/api/v1/triggers?page={0}&pageSize={1}";
        private const string GET_TRIGGERS_CONTAINING_TITLE = "/api/v1/triggers?title={0}&page={1}&pageSize={2}";
        private const string GET_TRIGGER_WITH_ID = "/api/v1/triggers/{0}";
        private const string POST_TRIGGER = "/api/v1/triggers";
        private const string PUT_TRIGGER = "/api/v1/triggers";
        private const string DELETE_TRIGGER_WITH_ID = "/api/v1/triggers/{0}";

        private const string GET_RETENTIONS_LASTEDIT = "/api/v1/retentions?lastedit";
        private const string GET_RETENTIONS_ALL = "/api/v1/retentions?page={0}&pageSize={1}";
        private const string GET_RETENTIONS_CONTAINING_TITLEORAUTHORITY = "/api/v1/retentions?titleOrAuthority={0}&page={1}&pageSize={2}";
        private const string GET_RETENTION_WITH_ID = "/api/v1/retentions/{0}";
        private const string POST_RETENTION = "/api/v1/retentions";
        private const string PUT_RETENTION = "/api/v1/retentions";
        private const string DELETE_RETENTION_WITH_ID = "/api/v1/retentions/{0}";

        private const string GET_LIFECYCLES_LASTEDIT = "/api/v1/lifecycles?lastedit";
        private const string GET_LIFECYCLES_ALL = "/api/v1/lifecycles?page={0}&pageSize={1}";
        private const string GET_LIFECYCLES_CONTAINING_TITLE = "/api/v1/lifecycles?title={0}&page={1}&pageSize={2}";
        private const string GET_LIFECYCLE_WITH_ID = "/api/v1/lifecycles/{0}";
        private const string GET_LIFECYCLE_SUMMARY_WITH_ID = "/api/v1/lifecycles/{0}?summary";
        private const string POST_LIFECYCLE = "/api/v1/lifecycles";
        private const string PUT_LIFECYCLE = "/api/v1/lifecycles";
        private const string DELETE_LIFECYCLE_WITH_ID = "/api/v1/lifecycles/{0}";

        private const string GET_MANAGEDPROPERTIES_LASTEDIT = "/api/v1/managedproperties?lastedit";
        private const string GET_MANAGEDPROPERTIES_ALL = "/api/v1/managedproperties?page={0}&pageSize={1}";
        private const string GET_MANAGEDPROPERTIES_CONTAINING_NAME = "/api/v1/managedproperties?name={0}&page={1}&pageSize={2}";
        private const string GET_MANAGEDPROPERTY_WITH_ID = "/api/v1/managedproperties/{0}";
        private const string POST_MANAGEDPROPERTY = "/api/v1/managedproperties";
        private const string PUT_MANAGEDPROPERTY = "/api/v1/managedproperties";
        private const string DELETE_MANAGEDPROPERTY_WITH_ID = "/api/v1/managedproperties/{0}";
        
        private const string GET_RECORDCLASSLIFECYCLES_LASTEDIT = "/api/v1/recordclasslifecycles?lastedit";
        private const string GET_RECORDCLASSLIFECYCLES_ALL = "/api/v1/recordclasslifecycles?page={0}&pageSize={1}";
        private const string GET_RECORDCLASSLIFECYCLES_RECORDCLASSTITLEORLIFECYCLETITLE = "/api/v1/recordclasslifecycles?recordClassTitleOrLifecycleTitle={0}&page={1}&pageSize={2}";
        private const string GET_RECORDCLASSLIFECYCLE_WITH_ID = "/api/v1/recordclasslifecycles/{0}";
        private const string GET_RECORDCLASSLIFECYCLE_WITH_RECORDCLASSID = "/api/v1/recordclasslifecycles?recordClassId={0}";
        private const string POST_RECORDCLASSLIFECYCLE = "/api/v1/recordclasslifecycles";
        private const string PUT_RECORDCLASSLIFECYCLE = "/api/v1/recordclasslifecycles";
        private const string DELETE_RECORDCLASSLIFECYCLE_WITH_ID = "/api/v1/recordclasslifecycles/{0}";
        
        private const string GET_EVENTOCCURRENCES_LASTEDIT = "/api/v1/eventoccurrences?lastedit";
        private const string GET_EVENTOCCURRENCES_ALL = "/api/v1/eventoccurrences?page={0}&pageSize={1}";
        private const string GET_EVENTOCCURRENCES_CONTAINING_EVENTTITLE = "/api/v1/eventoccurrences?eventTitle={0}&page={1}&pageSize={2}";
        private const string GET_EVENTOCCURRENCE_WITH_ID = "/api/v1/eventoccurrences/{0}";
        private const string POST_EVENTOCCURRENCE = "/api/v1/eventoccurrences";
        private const string DELETE_EVENTOCCURRENCE_WITH_ID = "/api/v1/eventoccurrences/{0}";
        
        private const string GET_ACTIONITEMS_LASTEDIT = "/api/v1/actionitems?lastedit";
        private const string GET_ACTIONITEM_WITH_ID = "/api/v1/actionitems?id={0}";
        private const string GET_ACTIONITEM_FOR_RECORDID = "/api/v1/actionitems?recordId={0}";
        private const string GET_ACTIONITEM_FOR_RECORDIDENTIFIER = "/api/v1/actionitems?identifier={0}";
        private const string PUT_ACTIONITEM = "/api/v1/actionitems";
        
        private const string GET_PENDING_ACTIONITEMS_ALL = "/api/v1/actionitemspending?page={0}&pageSize={1}";
        private const string GET_PENDING_ACTIONITEMS_CONTAINING_RECORDTITLEORURI = "/api/v1/actionitemspending?recordTitleOrUri={0}&page={1}&pageSize={2}";
        private const string GET_PENDING_ACTIONITEMS_CONTAINING_RECORDURI = "/api/v1/actionitemspending?recordUri={0}&page={1}&pageSize={2}";
        private const string GET_PENDING_SYSTEM_ACTIONITEMS_CONTAINING_RECORDURI = "/api/v1/actionitemspending?system=true&recordUri={0}&page={1}&pageSize={2}";
        private const string PUT_PENDING_ACTIONITEM_COMPLETED = "/api/v1/actionitemspending?complete=true&id={0}";
        private const string PUT_PENDING_ACTIONITEM_FAILED = "/api/v1/actionitemspending?failed=true&id={0}";
        private const string PUT_PENDING_ACTIONITEM_UNSUPPORTED = "/api/v1/actionitemspending?unsupported=true&id={0}";
        
        private const string GET_INBOX_ACTIONITEMS_ALL = "/api/v1/actionitemsinbox?page={0}&pageSize={1}";
        private const string GET_INBOX_ACTIONITEMS_CONTAINING_TITLE = "/api/v1/actionitemsinbox?title={0}&page={1}&pageSize={2}";
        private const string GET_INBOX_ACTIONITEMS_CASE = "/api/v1/actionitemsinbox?inboxItemId={0}&page={1}&pageSize={2}";
        private const string PUT_INBOX_ACTIONITEM_APPROVE = "/api/v1/actionitemsinbox?approve=true&id={0}";
        private const string PUT_INBOX_ACTIONITEM_DISMISS = "/api/v1/actionitemsinbox?dismiss=true&id={0}";
        private const string PUT_INBOX_ACTIONITEM_RETRY = "/api/v1/actionitemsinbox?retry=true&id={0}";
        private const string PUT_INBOX_ACTIONITEM_COMPLETED = "/api/v1/actionitemsinbox?complete=true&id={0}";
        private const string PUT_INBOX_ACTIONITEMS_APPROVE_CASE = "/api/v1/actionitemsinbox?approve=true&inboxItemId={0}";
        private const string PUT_INBOX_ACTIONITEMS_DISMISS_CASE = "/api/v1/actionitemsinbox?dismiss=true&inboxItemId={0}";
        private const string PUT_INBOX_ACTIONITEMS_RETRY_CASE = "/api/v1/actionitemsinbox?retry=true&inboxItemId={0}";
        
        private const string GET_AUDIT_LASTEDIT = "/api/v1/audit?lastedit";
        private const string GET_AUDIT_ALL = "/api/v1/audit?page={0}&pageSize={1}";
        private const string GET_AUDIT_IN_RANGE = "/api/v1/audit?rangeStart={0}&rangeEnd={1}&page={2}&pageSize={3}";
        private const string GET_AUDIT_FOR_TARGET = "/api/v1/audit?target={0}&targetId={1}&page={2}&pageSize={3}";
        private const string GET_AUDIT_FOR_RECORDURI = "/api/v1/audit?recordUri={0}&page={1}&pageSize={2}";
        private const string POST_AUDIT = "/api/v1/audit";
        
        private const string POST_HEARTBEAT = "/api/v1/heartbeat?supportsFailover={0}";
        
        private const string GET_LEGALCASES_LASTEDIT = "/api/v1/legalcases?lastedit";
        private const string GET_LEGALCASES_ALL = "/api/v1/legalcases?page={0}&pageSize={1}";
        private const string GET_LEGALCASES_CONTAINING_TITLEORNUMBER = "/api/v1/legalcases?titleOrNumber={0}&page={1}&pageSize={2}";
        private const string GET_LEGALCASES_OPEN = "/api/v1/legalcases?openOnly&page={0}&pageSize={1}";
        private const string GET_LEGALCASES_WITH_ID = "/api/v1/legalcases/{0}";
        private const string POST_LEGALCASES = "/api/v1/legalcases";
        private const string PUT_LEGALCASES = "/api/v1/legalcases";
        private const string DELETE_LEGALCASES_WITH_ID = "/api/v1/legalcases/{0}";
        
        private const string GET_LEGALHOLDS_ALL = "/api/v1/legalholds?page={0}&pageSize={1}";
        private const string GET_LEGALHOLDS_OPEN = "/api/v1/legalholds?open=1&page={0}&pageSize={1}";
        private const string GET_LEGALHOLDS_OPEN_CONTAINING_URI = "/api/v1/legalholds?uri={0}&open=1&page={1}&pageSize={2}";
        private const string GET_LEGALHOLDS_CONTAINING_URI = "/api/v1/legalholds?uri={0}&page={1}&pageSize={2}";
        private const string GET_LEGALHOLDS_CONTAINING_LEGALCASEID = "/api/v1/legalholds?caseId={0}&page={1}&pageSize={2}";
        private const string GET_LEGALHOLD_WITH_ID = "/api/v1/legalholds/{0}";
        private const string POST_LEGALHOLD = "/api/v1/legalholds";
        private const string PUT_LEGALHOLD = "/api/v1/legalholds";
        private const string DELETE_LEGALHOLD_WITH_ID = "/api/v1/legalholds/{0}";
        private const string DELETE_LEGALHOLD_FOR_CASE_WITH_URI = "/api/v1/legalholds?legalCaseId={0}&uri={1}";

        private const string GET_RULESETS_LASTEDIT = "/api/v1/rulesets?lastedit";
        private const string GET_RULESETS_ALL = "/api/v1/rulesets?page={0}&pageSize={1}";
        private const string GET_RULESETS_CONTAINING_TITLE = "/api/v1/rulesets?title={0}&page={1}&pageSize={2}";
        private const string GET_RULESET_WITH_ID = "/api/v1/rulesets/{0}";
        private const string POST_RULESET = "/api/v1/rulesets";
        private const string PUT_RULESET = "/api/v1/rulesets";
        private const string DELETE_RULESET_WITH_ID = "/api/v1/rulesets/{0}";

        #endregion
    }
}