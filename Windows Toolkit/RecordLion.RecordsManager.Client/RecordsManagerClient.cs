using System;
using System.Collections.Generic;
using System.IO;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

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
        private GenericXmlSecurityToken token = null;

        public RecordsManagerClient() : this(null, new RecordsManagerCredentials(), new DefaultSecurityTokenRequestor())
        {
        }


        public RecordsManagerClient(string url) : this(url, new RecordsManagerCredentials(), new DefaultSecurityTokenRequestor())
        {
        }


        public RecordsManagerClient(string url, RecordsManagerCredentials credentials) : this(url, credentials, new DefaultSecurityTokenRequestor())
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
            using (var client = new WebClient())
            {
                client.BaseAddress = this.baseUrl;

                var response = client.DownloadString(new Uri(GET_SYSTEMINFO, UriKind.Relative));

                return JsonConvert.DeserializeObject<SystemInfo>(response);
            }
        }


        public string NewRMUID()
        {
            return this.Get<string>(GET_NEWRMUID);
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
                return client.DownloadString(new Uri(string.Format(GET_RECORDCLASSES_ALL, page, pageSize), UriKind.Relative));
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
                return client.DownloadString(new Uri(string.Format(GET_RECORDCLASSES, page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty), UriKind.Relative));
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
                return client.DownloadString(new Uri(string.Format(GET_RECORDCLASSES_ALL_OPEN, page, pageSize), UriKind.Relative));
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
                return client.DownloadString(new Uri(string.Format(GET_RECORDCLASSES_OPEN, page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty), UriKind.Relative));
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
            return this.Get<DateTime>(GET_RECORDCLASSES_LASTEDIT);
        }
        

        public IEnumerable<RecordClass> SearchRecordClasses(string titleOrCode)
        {
            var page = this.SearchRecordClasses(titleOrCode, 0, 0);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> SearchRecordClasses(string titleOrCode, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClass>>(string.Format(GET_RECORDCLASSES_CONTAINING_TITLEORCODE, titleOrCode, page, pageSize));
        }
        

        public IEnumerable<RecordClass> GetAllRecordClasses()
        {
            var page = this.GetAllRecordClasses(0, 0);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetAllRecordClasses(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClass>>(string.Format(GET_RECORDCLASSES_ALL, page, pageSize));
        }
        

        public IEnumerable<RecordClass> GetRecordClasses(long? parentId = null)
        {
            var page = this.GetRecordClasses(0, 0, parentId);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetRecordClasses(int page, int pageSize, long? parentId = null)
        {
            return this.Get<ClientPagedItems<RecordClass>>(string.Format(GET_RECORDCLASSES, page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }
        

        public IEnumerable<RecordClass> GetAllOpenRecordClasses()
        {
            var page = this.GetAllOpenRecordClasses(0, 0);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetAllOpenRecordClasses(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClass>>(string.Format(GET_RECORDCLASSES_ALL_OPEN, page, pageSize));
        }
        

        public IEnumerable<RecordClass> GetOpenRecordClasses(long? parentId = null)
        {
            var page = this.GetOpenRecordClasses(0, 0, parentId);
            
            return page.Items;
        }
        

        public IClientPagedItems<RecordClass> GetOpenRecordClasses(int page, int pageSize, long? parentId = null)
        {
            return this.Get<ClientPagedItems<RecordClass>>(string.Format(GET_RECORDCLASSES_OPEN, page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }
        

        public RecordClass GetRecordClass(long id)
        {
            return this.Get<RecordClass>(string.Format(GET_RECORDCLASS_WITH_ID, id));
        }
        

        public RecordClass GetRecordClass(string code)
        {
            return this.Get<RecordClass>(string.Format(GET_RECORDCLASS_WITH_CODE, code));
        }
        

        public RecordClass CreateRecordClass(RecordClass recordClass)
        {
            return this.Post<RecordClass>(POST_RECORDCLASS, recordClass);
        }


        public RecordClass CreateRecordClassCaseFile(RecordClass recordClass)
        {
            recordClass.IsCaseFile = true;

            return this.CreateRecordClass(recordClass);
        }
        

        public RecordClass UpdateRecordClass(RecordClass recordClass)
        {
            return this.Put<RecordClass>(PUT_RECORDCLASS, recordClass);
        }
        

        public void DeleteRecordClass(long id)
        {
            this.Delete(string.Format(DELETE_RECORDCLASS_WITH_ID, id));
        }
        
        #endregion

        
        #region Records

        public DateTime GetRecordsLastEdit()
        {
            return this.Get<DateTime>(GET_RECORDS_LASTEDIT);
        }
            
        
        public string GetRecordsAsJson()
        {
            return this.GetRecordsAsJson(0, 0);
        }
            
        
        public string GetRecordsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(string.Format(GET_RECORDS_ALL, page, pageSize), UriKind.Relative));
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
                return client.DownloadString(new Uri(string.Format(GET_RECORDS_FOR_RECORDCLASS, recordClassId, page, pageSize), UriKind.Relative));
            }
        }


        public string GetRecordsForContainerAsJson(long containerId)
        {
            return this.GetRecordsForContainerAsJson(containerId, 0, 0);
        }
            
        
        public string GetRecordsForContainerAsJson(long containerId, int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(string.Format(GET_RECORDS_FOR_CONTAINER, containerId, page, pageSize), UriKind.Relative));
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
            return this.Get<ClientPagedItems<Record>>(string.Format(GET_RECORDS_CONTAINING_TITLEORURI, titleOrUri, page, pageSize));
        }                 
            
        
        public IEnumerable<Record> GetRecords()
        {
            var page = this.GetRecords(0, 0);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<Record> GetRecords(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Record>>(string.Format(GET_RECORDS_ALL, page, pageSize));
        }


        public IEnumerable<Record> GetRecordsForRecordClass(long recordClassId)
        {
            var page = this.GetRecordsForRecordClass(recordClassId, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<Record> GetRecordsForRecordClass(long recordClassId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Record>>(string.Format(GET_RECORDS_FOR_RECORDCLASS, recordClassId, page, pageSize));
        }


        public IEnumerable<Record> GetRecordsForContainer(long containerId)
        {
            var page = this.GetRecordsForContainer(containerId, 0, 0);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<Record> GetRecordsForContainer(long containerId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Record>>(string.Format(GET_RECORDS_FOR_CONTAINER, containerId, page, pageSize));
        }
            
        
        public Record GetRecord(long id)
        {
            return this.Get<Record>(string.Format(GET_RECORD_WITH_ID, id));
        }
            
        
        public Record GetRecordByIdentifier(string identifier)
        {
            return this.Get<Record>(string.Format(GET_RECORD_WITH_IDENTIFIER, identifier));
        }
            
        
        public Record GetRecordByUri(string uri)
        {
            return this.Get<Record>(string.Format(GET_RECORD_WITH_URI, uri));
        }
            
        
        public void DeclareRecord(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION, uri), declaration);
        }
            
        
        public void UndeclareRecord(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION, uri), declaration);
        }
            
        
        public void DeclareVital(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION, uri), declaration);
        }
            
        
        public void UndeclareVital(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION, uri), declaration);
        }
            
        
        public void DeclareObsolete(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION, uri), declaration);
        }
            
        
        public void UndeclareObsolete(string uri)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION, uri), declaration);
        }
            
        
        public void DeclareRecordById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_ID, id), declaration);
        }
            
        
        public void UndeclareRecordById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_ID, id), declaration);
        }
            
        
        public void DeclareVitalById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_ID, id), declaration);
        }
            
        
        public void UndeclareVitalById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_ID, id), declaration);
        }
            
        
        public void DeclareObsoleteById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_ID, id), declaration);
        }
            
        
        public void UndeclareObsoleteById(long id)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_ID, id), declaration);
        }
            
        
        public void DeclareRecordByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_IDENTIFIER, identifier), declaration);
        }
            
        
        public void UndeclareRecordByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Record = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_IDENTIFIER, identifier), declaration);
        }
            
        
        public void DeclareVitalByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_IDENTIFIER, identifier), declaration);
        }
            
        
        public void UndeclareVitalByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Vital = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_IDENTIFIER, identifier), declaration);
        }
            
        
        public void DeclareObsoleteByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Declare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_IDENTIFIER, identifier), declaration);
        }
            
        
        public void UndeclareObsoleteByIdentifier(string identifier)
        {
            var declaration = new RecordDeclaration()
            {
                Obsolete = RecordDeclarationState.Undeclare
            };
                
            this.Put(string.Format(PUT_RECORD_DECLARATION_WITH_IDENTIFIER, identifier), declaration);
        }
            
        #endregion


        #region Containers

        public string GetAllContainersAsJson()
        {
            return this.GetAllContainersAsJson(0, 0);
        }
        
            
        public string GetAllContainersAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(string.Format(GET_CONTAINERS_ALL, page, pageSize), UriKind.Relative));
            }
        }
                
            
        public string GetContainersAsJson(long? parentId = null)
        {
            return this.GetContainersAsJson(0, 0, parentId);
        }
        
            
        public string GetContainersAsJson(int page, int pageSize, long? parentId = null)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(string.Format(GET_CONTAINERS, page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty), UriKind.Relative));
            }
        }
                
            
        public IEnumerable<Container> GetContainersFromJson(string json)
        {
            var page = this.GetContainersWithPageDataFromJson(json);
        
            return page.Items;
        }

            
        public IClientPagedItems<Container> GetContainersWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<Container>>(json);
        }
        
            
        public DateTime GetContainerLastEdit()
        {
            return this.Get<DateTime>(GET_CONTAINERS_LASTEDIT);
        }
        
            
        public IEnumerable<Container> SearchContainers(string title)
        {
            var page = this.SearchContainers(title, 0, 0);
        
            return page.Items;
        }

            
        public IClientPagedItems<Container> SearchContainers(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Container>>(string.Format(GET_CONTAINERS_CONTAINING_TITLE, title, page, pageSize));
        }
        
            
        public IEnumerable<Container> GetAllContainers()
        {
            var page = this.GetAllContainers(0, 0);
        
            return page.Items;
        }

            
        public IClientPagedItems<Container> GetAllContainers(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Container>>(string.Format(GET_CONTAINERS_ALL, page, pageSize));
        }
        
            
        public IEnumerable<Container> GetContainers(long? parentId = null)
        {
            var page = this.GetContainers(0, 0, parentId);
        
            return page.Items;
        }

            
        public IClientPagedItems<Container> GetContainers(int page, int pageSize, long? parentId = null)
        {
            return this.Get<ClientPagedItems<Container>>(string.Format(GET_CONTAINERS, page, pageSize, (parentId.HasValue) ? parentId.Value.ToString() : string.Empty));
        }
        
            
        public Container GetContainer(long id)
        {
            return this.Get<Container>(string.Format(GET_CONTAINER_WITH_ID, id));
        }
        
            
        public Container GetContainer(string barcode)
        {
            return this.Get<Container>(string.Format(GET_CONTAINER_WITH_BARCODE, barcode));
        }
        
            
        public Container CreateContainer(Container container)
        {
            return this.Post<Container>(POST_CONTAINER, container);
        }
        
            
        public Container UpdateContainer(Container container)
        {
            return this.Put<Container>(PUT_CONTAINER, container);
        }
        
            
        public void DeleteContainer(long id)
        {
            this.Delete(string.Format(DELETE_CONTAINER_WITH_ID, id));
        }
        
        #endregion
        

        #region Barcodes
        
        public string GenerateBarcode(long barcodeSchemeId)
        {
            return this.Get<string>(string.Format(GET_BARCODES_GENERATE, barcodeSchemeId));
        }
        
        
        public DateTime GetBarcodeSchemesLastEdit()
        {
            return this.Get<DateTime>(GET_BARCODES_LASTEDIT);
        }
        
        
        public IEnumerable<BarcodeScheme> SearchBarcodeSchemes(string title)
        {
            var page = this.SearchBarcodeSchemes(title, 0, 0);

            return page.Items;
        }
            

        public IClientPagedItems<BarcodeScheme> SearchBarcodeSchemes(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<BarcodeScheme>>(string.Format(GET_BARCODES_WITH_TITLE, title, page, pageSize));
        }             
        
        
        public IEnumerable<BarcodeScheme> GetBarcodeSchemes()
        {
            var paged = this.GetBarcodeSchemes(0, 0);

            return paged.Items;
        }
            

        public IClientPagedItems<BarcodeScheme> GetBarcodeSchemes(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<BarcodeScheme>>(string.Format(GET_BARCODES_ALL, page, pageSize));
        }
        
        
        public BarcodeScheme GetBarcodeScheme(long id)
        {
            return this.Get<BarcodeScheme>(string.Format(GET_BARCODES_WITH_ID, id));
        }
        
        
        public BarcodeScheme CreateBarcodeScheme(BarcodeScheme scheme)
        {
            return this.Post<BarcodeScheme>(POST_BARCODE, scheme);
        }
        
        
        public BarcodeScheme UpdateBarcodeScheme(BarcodeScheme scheme)
        {
            return this.Put<BarcodeScheme>(PUT_BARCODE, scheme);
        }
        
        
        public void DeleteBarcodeScheme(long id)
        {
            this.Delete(string.Format(DELETE_BARCODE, id));
        }
        
        #endregion
            
        
        #region Recordization

        public IEnumerable<Record> ProcessRecordization(IEnumerable<Recordize> recordizers)
        {
            return this.Post<IEnumerable<Record>>(POST_RECORDIZERS, recordizers.ToArray());
        }

        
        public void DeleteRecordization(string recordizedUrl, bool deleteAll)
        {
            this.Delete(string.Format(DELETE_RECORDIZERS, recordizedUrl, deleteAll));
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
                return client.DownloadString(new Uri(string.Format(GET_TRIGGERS_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_TRIGGERS_LASTEDIT);
        }


        public IEnumerable<RetentionTrigger> SearchTriggers(string title)
        {
            var page = this.SearchTriggers(title, 0, 0);
        
            return page.Items;
        }
        
        
        public IClientPagedItems<RetentionTrigger> SearchTriggers(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RetentionTrigger>>(string.Format(GET_TRIGGERS_CONTAINING_TITLE, title, page, pageSize));
        }


        public IEnumerable<RetentionTrigger> GetTriggers()
        {
            var page = this.GetTriggers(0, 0);
        
            return page.Items;
        }
        
        
        public IClientPagedItems<RetentionTrigger> GetTriggers(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RetentionTrigger>>(string.Format(GET_TRIGGERS_ALL, page, pageSize));
        }


        public RetentionTrigger GetTrigger(long id)
        {
            return this.Get<RetentionTrigger>(string.Format(GET_TRIGGER_WITH_ID, id));
        }


        public RetentionTrigger CreateTrigger(RetentionTrigger trigger)
        {
            return this.Post<RetentionTrigger>(POST_TRIGGER, trigger);
        }


        public RetentionTrigger UpdateTrigger(RetentionTrigger trigger)
        {
            return this.Put<RetentionTrigger>(PUT_TRIGGER, trigger);
        }


        public void DeleteTrigger(long id)
        {
            this.Delete(string.Format(DELETE_TRIGGER_WITH_ID, id));
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
                return client.DownloadString(new Uri(string.Format(GET_RETENTIONS_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_RETENTIONS_LASTEDIT);
        }
        

        public IEnumerable<Retention> GetRetentions()
        {
            var page = this.GetRetentions(0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<Retention> GetRetentions(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Retention>>(string.Format(GET_RETENTIONS_ALL, page, pageSize));
        }
        

        public IEnumerable<Retention> SearchRetentions(string titleOrAuthority)
        {
            var page = this.SearchRetentions(titleOrAuthority, 0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<Retention> SearchRetentions(string titleOrAuthority, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Retention>>(string.Format(GET_RETENTIONS_CONTAINING_TITLEORAUTHORITY, titleOrAuthority, page, pageSize));
        }
        

        public Retention GetRetention(long id)
        {
            return this.Get<Retention>(string.Format(GET_RETENTION_WITH_ID, id));
        }


        public Retention CreateRetention(Retention retention)
        {
            return this.Post<Retention>(POST_RETENTION, retention);
        }


        public Retention UpdateRetention(Retention retention)
        {
            return this.Put<Retention>(PUT_RETENTION, retention);
        }
        

        public void DeleteRetention(long id)
        {
            this.Delete(string.Format(DELETE_RETENTION_WITH_ID, id));
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
                return client.DownloadString(new Uri(string.Format(GET_LIFECYCLES_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_LIFECYCLES_LASTEDIT);
        }
            
        
        public IEnumerable<Lifecycle> SearchLifecycles(string title)
        {
            var page = this.SearchLifecycles(title, 0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<Lifecycle> SearchLifecycles(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Lifecycle>>(string.Format(GET_LIFECYCLES_CONTAINING_TITLE, title, page, pageSize));
        }
            
        
        public IEnumerable<Lifecycle> GetLifecycles()
        {
            var page = this.GetLifecycles(0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<Lifecycle> GetLifecycles(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<Lifecycle>>(string.Format(GET_LIFECYCLES_ALL, page, pageSize));
        }
            
        
        public Lifecycle GetLifecycle(long id)
        {
            return this.Get<Lifecycle>(string.Format(GET_LIFECYCLE_WITH_ID, id));
        }


        public List<LifecyclePhaseSummary> GetLifecyclesSummary(long id)
        {
            return this.Get<List<LifecyclePhaseSummary>>(string.Format(GET_LIFECYCLE_SUMMARY_WITH_ID, id));
        }
            
        
        public Lifecycle CreateLifecycle(Lifecycle lifecycle)
        {
            return this.Post<Lifecycle>(POST_LIFECYCLE, lifecycle);
        }
            
        
        public Lifecycle UpdateLifecycle(Lifecycle lifecycle)
        {
            return this.Put<Lifecycle>(PUT_LIFECYCLE, lifecycle);
        }
            
        
        public void DeleteLifecycle(long id)
        {
            this.Delete(string.Format(DELETE_LIFECYCLE_WITH_ID, id));
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
                return client.DownloadString(new Uri(string.Format(GET_RECORDCLASSLIFECYCLES_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_RECORDCLASSLIFECYCLES_LASTEDIT);
        }
        
            
        public IEnumerable<RecordClassLifecycle> SearchRecordClassLifecycles(string title)
        {
            var page = this.SearchRecordClassLifecycles(title, 0, 0);
        
            return page.Items;
        }
        

        public IClientPagedItems<RecordClassLifecycle> SearchRecordClassLifecycles(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClassLifecycle>>(string.Format(GET_RECORDCLASSLIFECYCLES_RECORDCLASSTITLEORLIFECYCLETITLE, title, page, pageSize));
        }

            
        public IEnumerable<RecordClassLifecycle> GetRecordClassLifecycles()
        {
            var page = this.GetRecordClassLifecycles(0, 0);
        
            return page.Items;
        }
        

        public IClientPagedItems<RecordClassLifecycle> GetRecordClassLifecycles(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordClassLifecycle>>(string.Format(GET_RECORDCLASSLIFECYCLES_ALL, page, pageSize));
        }

            
        public RecordClassLifecycle GetRecordClassLifecycle(long id)
        {
            return this.Get<RecordClassLifecycle>(string.Format(GET_RECORDCLASSLIFECYCLE_WITH_ID, id));
        }
        
            
        public RecordClassLifecycle CreateRecordClassLifecycle(RecordClassLifecycle lifecycle)
        {
            return this.Post<RecordClassLifecycle>(POST_RECORDCLASSLIFECYCLE, lifecycle);
        }
        
            
        public RecordClassLifecycle UpdateRecordClassLifecycle(RecordClassLifecycle lifecycle)
        {
            return this.Put<RecordClassLifecycle>(PUT_RECORDCLASSLIFECYCLE, lifecycle);
        }
        
            
        public void DeleteRecordClassLifecycle(long id)
        {
            this.Delete(string.Format(DELETE_RECORDCLASSLIFECYCLE_WITH_ID, id));
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
                return client.DownloadString(new Uri(string.Format(GET_EVENTOCCURRENCES_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_EVENTOCCURRENCES_LASTEDIT);
        }
        
        
        public IEnumerable<EventOccurrence> SearchEventOccurrences(string eventTitle)
        {
            var page = this.SearchEventOccurrences(eventTitle, 0, 0);

            return page.Items;
        }
            
        
        public IClientPagedItems<EventOccurrence> SearchEventOccurrences(string eventTitle, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<EventOccurrence>>(string.Format(GET_EVENTOCCURRENCES_CONTAINING_EVENTTITLE, eventTitle, page, pageSize));
        }
            

        public IEnumerable<EventOccurrence> GetEventOccurrences()
        {
            var page = this.GetEventOccurrences(0, 0);

            return page.Items;
        }
            
        
        public IClientPagedItems<EventOccurrence> GetEventOccurrences(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<EventOccurrence>>(string.Format(GET_EVENTOCCURRENCES_ALL, page, pageSize));
        }
            

        public EventOccurrence GetEventOccurrences(long id)
        {
            return this.Get<EventOccurrence>(string.Format(GET_EVENTOCCURRENCE_WITH_ID, id));
        }
        
        
        public EventOccurrence CreateEventOccurrence(EventOccurrence eventOccurrence)
        {
            return this.Post<EventOccurrence>(POST_EVENTOCCURRENCE, eventOccurrence);
        }
        
        
        public void DeleteEventOccurrence(long id)
        {
            this.Delete(string.Format(DELETE_EVENTOCCURRENCE_WITH_ID, id));
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
                return client.DownloadString(new Uri(string.Format(GET_AUDIT_ALL, page, pageSize), UriKind.Relative));
            }
        }
            
        
        public IEnumerable<AuditEntry> GetAuditsFromJson(string json)
        {
            var page = this.GetAuditsWithPageDataFromJson(json);
        
            return page.Items;
        }
                
            
        public IClientPagedItems<AuditEntry> GetAuditsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IClientPagedItems<AuditEntry>>(json);
        }
        
            
        public DateTime GetAuditsLastEdit()
        {
            return this.Get<DateTime>(GET_AUDIT_LASTEDIT);
        }

        
        public IEnumerable<AuditEntry> SearchAudits(DateTime rangeStart, DateTime rangeEnd)
        {
            var page = this.SearchAudits(rangeStart, rangeEnd, 0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> SearchAudits(DateTime rangeStart, DateTime rangeEnd, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(string.Format(GET_AUDIT_IN_RANGE, rangeStart, rangeEnd, page, pageSize));
        }
        
            
        public IEnumerable<AuditEntry> GetAudits()
        {
            var page = this.GetAudits(0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> GetAudits(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(string.Format(GET_AUDIT_ALL, page, pageSize));
        }
        
            
        public IEnumerable<AuditEntry> GetAudits(AuditTarget target, long targetId)
        {
            var page = this.GetAudits(target, targetId, 0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> GetAudits(AuditTarget target, long targetId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(string.Format(GET_AUDIT_FOR_TARGET, target, targetId, page, pageSize));
        }
        
            
        public IEnumerable<AuditEntry> GetAuditsForRecord(string recordUri)
        {
            var page = this.GetAuditsForRecord(recordUri, 0, 0);

            return page.Items;
        }
        
            
        public IClientPagedItems<AuditEntry> GetAuditsForRecord(string recordUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<AuditEntry>>(string.Format(GET_AUDIT_FOR_RECORDURI, recordUri, page, pageSize));
        }
        
            
        public AuditEntry CreateAudit(NewAuditEntry auditEntry)
        {
            return this.Post<AuditEntry>(POST_AUDIT, auditEntry);
        }

        #endregion
        
            
        #region Heartbeat

        public HeartbeatResult Heartbeat(Heartbeat heartbeat)
        {
            return this.Heartbeat(heartbeat, false);
        }
        

        public HeartbeatResult Heartbeat(Heartbeat heartbeat, bool supportsFailover)
        {
            return this.Post<HeartbeatResult>(string.Format(POST_HEARTBEAT, supportsFailover), heartbeat);
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
                return client.DownloadString(new Uri(string.Format(GET_LEGALCASES_ALL, page, pageSize), UriKind.Relative));
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
                return client.DownloadString(new Uri(string.Format(GET_LEGALCASES_OPEN, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_LEGALCASES_LASTEDIT);
        }
        

        public IEnumerable<LegalCase> SearchLegalCases(string titleOrNumber)
        {
            var page = this.SearchLegalCases(titleOrNumber, 0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<LegalCase> SearchLegalCases(string titleOrNumber, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalCase>>(string.Format(GET_LEGALCASES_CONTAINING_TITLEORNUMBER, titleOrNumber, page, pageSize));
        }

        
        public IEnumerable<LegalCase> GetLegalCases()
        {
            var page = this.GetLegalCases(0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<LegalCase> GetLegalCases(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalCase>>(string.Format(GET_LEGALCASES_ALL, page, pageSize));
        }

        
        public IEnumerable<LegalCase> GetOpenLegalCases()
        {
            var page = this.GetOpenLegalCases(0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<LegalCase> GetOpenLegalCases(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalCase>>(string.Format(GET_LEGALCASES_OPEN, page, pageSize));
        }

        
        public LegalCase GetLegalCase(long id)
        {
            return this.Get<LegalCase>(string.Format(GET_LEGALCASES_WITH_ID, id));
        }
        

        public LegalCase CreateLegalCase(LegalCase legalCase)
        {
            return this.Post<LegalCase>(POST_LEGALCASES, legalCase);
        }
        

        public LegalCase UpdateLegalCase(LegalCase legalCase)
        {
            return this.Put<LegalCase>(PUT_LEGALCASES, legalCase);
        }
        

        public void DeleteLegalCase(long id)
        {
            this.Delete(string.Format(DELETE_LEGALCASES_WITH_ID, id));
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
                return client.DownloadString(new Uri(string.Format(GET_LEGALHOLDS_ALL, page, pageSize), UriKind.Relative));
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
                return client.DownloadString(new Uri(string.Format(GET_LEGALHOLDS_OPEN, page, pageSize), UriKind.Relative));
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
            return this.Get<ClientPagedItems<LegalHold>>(string.Format(GET_LEGALHOLDS_ALL, page, pageSize));
        }


        public IEnumerable<LegalHold> GetOpenLegalHolds()
        {
            var page = this.GetOpenLegalHolds(0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> GetOpenLegalHolds(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(string.Format(GET_LEGALHOLDS_OPEN, page, pageSize));
        }


        public IEnumerable<LegalHold> SearchLegalHolds(string uri)
        {
            var page = this.SearchLegalHolds(uri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> SearchLegalHolds(string uri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(string.Format(GET_LEGALHOLDS_CONTAINING_URI, uri, page, pageSize));
        }


        public IEnumerable<LegalHold> SearchOpenLegalHolds(string uri)
        {
            var page = this.SearchOpenLegalHolds(uri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> SearchOpenLegalHolds(string uri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(string.Format(GET_LEGALHOLDS_OPEN_CONTAINING_URI, uri, page, pageSize));
        }


        public IEnumerable<LegalHold> SearchLegalHolds(long legalCaseId)
        {
            var page = this.SearchLegalHolds(legalCaseId, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<LegalHold> SearchLegalHolds(long legalCaseId, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<LegalHold>>(string.Format(GET_LEGALHOLDS_CONTAINING_LEGALCASEID, legalCaseId, page, pageSize));
        }


        public LegalHold GetLegalHoldById(long id)
        {
            return this.Get<LegalHold>(string.Format(GET_LEGALHOLD_WITH_ID, id));
        }
            
        
        public LegalHold CreateLegalHold(LegalHold legalHold)
        {
            return this.Post<LegalHold>(POST_LEGALHOLD, legalHold);
        }
            
        
        public LegalHold UpdateLegalHold(LegalHold legalHold)
        {
            return this.Put<LegalHold>(PUT_LEGALHOLD, legalHold);
        }
            
        
        public void DeleteLegalHold(long legalCaseId, string uri)
        {
            this.Delete(string.Format(DELETE_LEGALHOLD_FOR_CASE_WITH_URI, legalCaseId, uri));
        }
            
        
        public void DeleteLegalHold(long id)
        {
            this.Delete(string.Format(DELETE_LEGALHOLD_WITH_ID, id));
        }
            
        #endregion


        #region Action Items
        
        public DateTime GetActionItemsLastEdit()
        {
            return this.Get<DateTime>(GET_ACTIONITEMS_LASTEDIT);
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
            return this.Get<ActionItem>(string.Format(GET_ACTIONITEM_WITH_ID, id));
        }

            
        public ActionItem GetActionItemForRecord(long recordId)
        {
            return this.Get<ActionItem>(string.Format(GET_ACTIONITEM_FOR_RECORDID, recordId));
        }
        
            
        public ActionItem GetActionItemForRecord(string identifier)
        {
            return this.Get<ActionItem>(string.Format(GET_ACTIONITEM_FOR_RECORDIDENTIFIER, identifier));
        }
        
            
        public ActionItem UpdateActionItem(ActionItem actionItem)
        {
            return this.Put<ActionItem>(PUT_ACTIONITEM, actionItem);
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
                return client.DownloadString(new Uri(string.Format(GET_PENDING_ACTIONITEMS_ALL, page, pageSize), UriKind.Relative));
            }
        }

        
        public IEnumerable<ActionItem> SearchPendingActionItemsByTitleOrUri(string recordTitleOrUri)
        {
            var page = this.SearchPendingActionItemsByTitleOrUri(recordTitleOrUri, 0, 0);
            
            return page.Items;
        }

        
        public IClientPagedItems<ActionItem> SearchPendingActionItemsByTitleOrUri(string recordTitleOrUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(string.Format(GET_PENDING_ACTIONITEMS_CONTAINING_RECORDTITLEORURI, recordTitleOrUri, page, pageSize));
        }


        public IEnumerable<ActionItem> SearchPendingActionItemsByUri(string recordUri)
        {
            var page = this.SearchPendingActionItemsByUri(recordUri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<ActionItem> SearchPendingActionItemsByUri(string recordUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(string.Format(GET_PENDING_ACTIONITEMS_CONTAINING_RECORDURI, recordUri, page, pageSize));
        }


        public IEnumerable<ActionItem> SearchPendingSystemActionItemsByUri(string recordUri)
        {
            var page = this.SearchPendingSystemActionItemsByUri(recordUri, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<ActionItem> SearchPendingSystemActionItemsByUri(string recordUri, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(string.Format(GET_PENDING_SYSTEM_ACTIONITEMS_CONTAINING_RECORDURI, recordUri, page, pageSize));
        }

        
        public IEnumerable<ActionItem> GetPendingActionItems()
        {
            var page = this.GetPendingActionItems(0, 0);
        
            return page.Items;
        }
            
        
        public IClientPagedItems<ActionItem> GetPendingActionItems(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ActionItem>>(string.Format(GET_PENDING_ACTIONITEMS_ALL, page, pageSize));
        }
            
        
        public ActionItem UpdatePendingActionItemCompleted(long id)
        {
            return this.Put<ActionItem>(PUT_PENDING_ACTIONITEM_COMPLETED, id);
        }
            

        public ActionItem UpdatePendingActionItemFailed(long id)
        {
            return this.Put<ActionItem>(PUT_PENDING_ACTIONITEM_FAILED, id);
        }
        
        
        public ActionItem UpdatePendingActionItemUnsupported(long id)
        {
            return this.Put<ActionItem>(PUT_PENDING_ACTIONITEM_UNSUPPORTED, id);
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
                return client.DownloadString(new Uri(string.Format(GET_INBOX_ACTIONITEMS_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<ClientPagedItems<InboxActionItem>>(string.Format(GET_INBOX_ACTIONITEMS_CONTAINING_TITLE, title, page, pageSize));
        }


        public IEnumerable<InboxActionItem> GetInboxActionItems()
        {
            var page = this.GetInboxActionItems(0, 0);
        
            return page.Items;
        }


        public IClientPagedItems<InboxActionItem> GetInboxActionItems(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<InboxActionItem>>(string.Format(GET_INBOX_ACTIONITEMS_ALL, page, pageSize));
        }

        public IClientPagedItems<ActionItem> GetInboxActionItemsForCase(InboxActionItem caseItem, int page, int pageSize)
        {
            if (!caseItem.IsCaseFile)
                return null;

            return this.Get<ClientPagedItems<ActionItem>>(string.Format(GET_INBOX_ACTIONITEMS_CASE, caseItem.CaseFileId, caseItem.RetentionExpirationDate, caseItem.PhaseOrder, caseItem.PhaseAction, caseItem.IsApproved, caseItem.AutomationFailed, caseItem.AutomationUnsupported, page, pageSize));
        }
            
        public void UpdateInboxActionItemApprove(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_APPROVE, id);
        }


        public void UpdateInboxActionItemDismiss(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_DISMISS, id);
        }


        public void UpdateInboxActionItemRetry(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_RETRY, id);
        }


        public void UpdateInboxActionItemCompleted(long id)
        {
            this.Put(PUT_INBOX_ACTIONITEM_COMPLETED, id);
        }


        public void UpdateInboxCaseApprove(InboxActionItem caseItem)
        {
            this.Put(string.Format(PUT_INBOX_ACTIONITEMS_APPROVE_CASE, caseItem.CaseFileId, caseItem.RetentionExpirationDate, caseItem.PhaseOrder, caseItem.PhaseAction, caseItem.IsApproved, caseItem.AutomationFailed, caseItem.AutomationUnsupported), null);
        }


        public void UpdateInboxCaseDismiss(InboxActionItem caseItem)
        {
            this.Put(string.Format(PUT_INBOX_ACTIONITEMS_DISMISS_CASE, caseItem.CaseFileId, caseItem.RetentionExpirationDate, caseItem.PhaseOrder, caseItem.PhaseAction, caseItem.IsApproved, caseItem.AutomationFailed, caseItem.AutomationUnsupported), null);
        }


        public void UpdateInboxCaseRetry(InboxActionItem caseItem)
        {
            this.Put(string.Format(PUT_INBOX_ACTIONITEMS_RETRY_CASE, caseItem.CaseFileId, caseItem.RetentionExpirationDate, caseItem.PhaseOrder, caseItem.PhaseAction, caseItem.IsApproved, caseItem.AutomationFailed, caseItem.AutomationUnsupported), null);
        }


        #endregion
        
        #endregion


        #region Record Requests

        public string GetRecordRequestsAsJson()
        {
            return this.GetRecordRequestsAsJson(0, 0);
        }


        public string GetRecordRequestsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(string.Format(GET_RECORDREQUESTS_ALL, page, pageSize), UriKind.Relative));
            }
        }


        public IEnumerable<RecordRequest> GetRecordRequestsFromJson(string json)
        {
            var page = this.GetRecordRequestsWithPageDataFromJson(json);

            return page.Items;
        }


        public IClientPagedItems<RecordRequest> GetRecordRequestsWithPageDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ClientPagedItems<RecordRequest>>(json);
        }


        public DateTime GetRecordRequestsLastEdit()
        {
            return this.Get<DateTime>(GET_RECORDREQUESTS_LASTEDIT);
        }


        public IEnumerable<RecordRequest> GetRecordRequests()
        {
            var page = this.GetRecordRequests(0, 0);

            return page.Items;
        }


        public IClientPagedItems<RecordRequest> GetRecordRequests(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordRequest>>(string.Format(GET_RECORDREQUESTS_ALL, page, pageSize));
        }


        public IEnumerable<RecordRequest> SearchRecordRequests(string titleOrAuthority)
        {
            var page = this.SearchRecordRequests(titleOrAuthority, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<RecordRequest> SearchRecordRequests(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordRequest>>(string.Format(GET_RECORDREQUESTS_CONTAINING_TITLE, title, page, pageSize));
        }


        public RecordRequest GetRecordRequest(long id)
        {
            return this.Get<RecordRequest>(string.Format(GET_RECORDREQUEST_WITH_ID, id));
        }


        public RecordRequest CreateRecordRequest(RecordRequest request)
        {
            return this.Post<RecordRequest>(POST_RECORDREQUEST, request);
        }


        public RecordRequest UpdateRecordRequest(RecordRequest request)
        {
            return this.Put<RecordRequest>(PUT_RECORDREQUEST, request);
        }


        public void DeleteRecordRequest(long id)
        {
            this.Delete(string.Format(DELETE_RECORDREQUEST_WITH_ID, id));
        }


        #region Inbox Record Requests

        public string GetInboxRecordRequestsAsJson()
        {
            return this.GetInboxRecordRequestsAsJson(0, 0);
        }


        public string GetInboxRecordRequestsAsJson(int page, int pageSize)
        {
            using (var client = this.GetClient())
            {
                return client.DownloadString(new Uri(string.Format(GET_INBOX_RECORDREQUESTS_ALL, page, pageSize), UriKind.Relative));
            }
        }


        public IEnumerable<RecordRequest> SearchInboxRecordRequests(string searchTerms)
        {
            var page = this.SearchInboxRecordRequests(searchTerms, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<RecordRequest> SearchInboxRecordRequests(string title, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordRequest>>(string.Format(GET_INBOX_RECORDREQUESTS_CONTAINING_TITLE, title, page, pageSize));
        }


        public IEnumerable<RecordRequest> GetInboxRecordRequests()
        {
            var page = this.GetInboxRecordRequests(0, 0);

            return page.Items;
        }


        public IClientPagedItems<RecordRequest> GetInboxRecordRequests(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<RecordRequest>>(string.Format(GET_INBOX_RECORDREQUESTS_ALL, page, pageSize));
        }

        public RecordRequest GetInboxRecordRequest(long id)
        {
            return this.Get<RecordRequest>(string.Format(GET_INBOX_RECORDREQUEST_WITH_ID, id));
        }


        public RecordRequest CloseRecordRequest(long id)
        {
            return this.Put<RecordRequest>(string.Format(PUT_INBOX_RECORDREQUEST_WITH_ID, id), false);
        }


        public RecordRequest CloseAndFulfillRecordRequest(long id)
        {
            return this.Put<RecordRequest>(string.Format(PUT_INBOX_RECORDREQUEST_WITH_ID, id), true);
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
                return client.DownloadString(new Uri(string.Format(GET_MANAGEDPROPERTIES_ALL, page, pageSize), UriKind.Relative));
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
            return this.Get<DateTime>(GET_MANAGEDPROPERTIES_LASTEDIT);
        }


        public IEnumerable<ManagedProperty> SearchManagedProperties(string name)
        {
            var page = this.SearchManagedProperties(name, 0, 0);

            return page.Items;
        }


        public IClientPagedItems<ManagedProperty> SearchManagedProperties(string name, int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ManagedProperty>>(string.Format(GET_MANAGEDPROPERTIES_CONTAINING_NAME, name, page, pageSize));
        }


        public IEnumerable<ManagedProperty> GetManagedProperties()
        {
            var page = this.GetManagedProperties(0, 0);

            return page.Items;
        }


        public IClientPagedItems<ManagedProperty> GetManagedProperties(int page, int pageSize)
        {
            return this.Get<ClientPagedItems<ManagedProperty>>(string.Format(GET_MANAGEDPROPERTIES_ALL, page, pageSize));
        }


        public ManagedProperty GetManagedProperty(long id)
        {
            return this.Get<ManagedProperty>(string.Format(GET_MANAGEDPROPERTY_WITH_ID, id));
        }


        public ManagedProperty CreateManagedProperty(ManagedProperty property)
        {
            return this.Post<ManagedProperty>(POST_MANAGEDPROPERTY, property);
        }


        public ManagedProperty UpdateManagedProperty(ManagedProperty property)
        {
            return this.Put<ManagedProperty>(PUT_MANAGEDPROPERTY, property);
        }


        public void DeleteManagedProperty(long id)
        {
            this.Delete(string.Format(DELETE_MANAGEDPROPERTY_WITH_ID, id));
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
        

        private T Get<T>(string resourceUrl)
        {
            using (var client = this.GetClient())
            {
                var response = client.DownloadString(new Uri(resourceUrl, UriKind.Relative));
                return JsonConvert.DeserializeObject<T>(response, incomingJsonSettings);
            }
        }
        
            
        private T Post<T>(string resourceUrl, object data)
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
                    
        
        private T Put<T>(string resourceUrl, object data)
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
                
                
        private void Delete(string resourceUrl)
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
                var buffer = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", this.credentials.Username, this.credentials.Password));
                return string.Format("Basic {0}", Convert.ToBase64String(buffer));
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
            
            if (this.token == null || this.token.ValidTo < DateTime.Now)
                this.token = this.securityTokenRequestor.RequestToken(this.issuer, this.baseUrl, this.credentials);
        
            if (this.token != null)
            {
                var buffer = Encoding.ASCII.GetBytes(token.TokenXml.OuterXml);
                return string.Format("Saml {0}", Convert.ToBase64String(buffer));
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
        
        private const string GET_RECORDS_LASTEDIT = "/api/v1/records?lastedit";
        private const string GET_RECORDS_ALL = "/api/v1/records?page={0}&pageSize={1}";        
        private const string GET_RECORDS_CONTAINING_TITLEORURI = "/api/v1/records?titleOrUri={0}&page={1}&pageSize={2}";
        private const string GET_RECORDS_FOR_CONTAINER = "/api/v1/records?container=true&containerId={0}page={1}&pageSize={2}";
        private const string GET_RECORDS_FOR_RECORDCLASS = "/api/v1/records?classId={0}&page={1}&pageSize={2}";
        private const string GET_RECORD_WITH_ID = "/api/v1/records/{0}";
        private const string GET_RECORD_WITH_IDENTIFIER = "/api/v1/records?identifier={0}";
        private const string GET_RECORD_WITH_URI = "/api/v1/records?uri={0}";
        private const string PUT_RECORD_DECLARATION = "/api/v1/records?uri={0}";
        private const string PUT_RECORD_DECLARATION_WITH_ID = "/api/v1/records?id={0}";
        private const string PUT_RECORD_DECLARATION_WITH_IDENTIFIER = "/api/v1/records?identifier={0}";
        
        private const string GET_CONTAINERS_LASTEDIT = "/api/v1/containers?lastedit";
        private const string GET_CONTAINERS = "/api/v1/containers?page={0}&pageSize={1}&parentId={2}";
        private const string GET_CONTAINERS_ALL = "api/v1/containers?all=true&page={0}&pageSize={1}";
        private const string GET_CONTAINERS_CONTAINING_TITLE = "/api/v1/containers?title={0}&page={1}&pageSize={2}";
        private const string GET_CONTAINER_WITH_ID = "/api/v1/containers/{0}";
        private const string GET_CONTAINER_WITH_BARCODE = "/api/v1/containers?barcode={0}";
        private const string POST_CONTAINER = "/api/v1/containers";
        private const string PUT_CONTAINER = "/api/v1/containers";
        private const string DELETE_CONTAINER_WITH_ID = "/api/v1/containers/{0}";
        
        private const string GET_BARCODES_LASTEDIT = "/api/v1/barcodes?lastedit";
        private const string GET_BARCODES_GENERATE = "/api/v1/barcodes/{0}?generate";
        private const string GET_BARCODES_ALL = "/api/v1/barcodes?page={0}&pageSize={1}";
        private const string GET_BARCODES_WITH_ID = "/api/v1/barcodes/{0}";
        private const string GET_BARCODES_WITH_TITLE = "/api/v1/barcodes?title={0}&page={1}&pageSize={2}";
        private const string POST_BARCODE = "/api/v1/barcodes";
        private const string PUT_BARCODE = "/api/v1/barcodes";
        private const string DELETE_BARCODE = "/api/v1/barcodes/{0}";
        
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
        private const string GET_INBOX_ACTIONITEMS_CASE = "/api/v1/actionitemsinbox?caseFileId={0}&expiringOnOrBefore={1}&phaseOrder={2}&phaseAction={3}&isApproved={4}&automationFailed={5}&automationUnsupported={6}&page={7}&pageSize={8}";
        private const string PUT_INBOX_ACTIONITEM_APPROVE = "/api/v1/actionitemsinbox?approve=true&id={0}";
        private const string PUT_INBOX_ACTIONITEM_DISMISS = "/api/v1/actionitemsinbox?dismiss=true&id={0}";
        private const string PUT_INBOX_ACTIONITEM_RETRY = "/api/v1/actionitemsinbox?retry=true&id={0}";
        private const string PUT_INBOX_ACTIONITEM_COMPLETED = "/api/v1/actionitemsinbox?complete=true&id={0}";
        private const string PUT_INBOX_ACTIONITEMS_APPROVE_CASE = "/api/v1/actionitemsinbox?approve=true&caseFileId={0}&expiringOnOrBefore={1}&phaseOrder={2}&phaseAction={3}&isApproved={4}&automationFailed={5}&automationUnsupported={6}";
        private const string PUT_INBOX_ACTIONITEMS_DISMISS_CASE = "/api/v1/actionitemsinbox?dismiss=true&caseFileId={0}&expiringOnOrBefore={1}&phaseOrder={2}&phaseAction={3}&isApproved={4}&automationFailed={5}&automationUnsupported={6}";
        private const string PUT_INBOX_ACTIONITEMS_RETRY_CASE = "/api/v1/actionitemsinbox?retry=true&caseFileId={0}&expiringOnOrBefore={1}&phaseOrder={2}&phaseAction={3}&isApproved={4}&automationFailed={5}&automationUnsupported={6}";
        
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

        private const string GET_RECORDREQUESTS_LASTEDIT = "/api/v1/requests?lastedit";
        private const string GET_RECORDREQUESTS_ALL = "/api/v1/requests?page={0}&pageSize={1}";
        private const string GET_RECORDREQUESTS_CONTAINING_TITLE = "/api/v1/requests?title={0}&page={1}&pageSize={2}";
        private const string GET_RECORDREQUEST_WITH_ID = "/api/v1/requests/{0}";
        private const string POST_RECORDREQUEST = "/api/v1/requests";
        private const string PUT_RECORDREQUEST = "/api/v1/requests";
        private const string DELETE_RECORDREQUEST_WITH_ID = "/api/v1/requests/{0}";

        private const string GET_INBOX_RECORDREQUESTS_ALL = "/api/v1/requestsinbox?page={0}&pageSize={1}";
        private const string GET_INBOX_RECORDREQUESTS_CONTAINING_TITLE = "/api/v1/requestsinbox?title={0}&page={1}&pageSize={2}";
        private const string GET_INBOX_RECORDREQUEST_WITH_ID = "/api/v1/requestsinbox/{0}";
        private const string PUT_INBOX_RECORDREQUEST_WITH_ID = "/api/v1/requestsinbox?id={0}";

        #endregion
    }
}