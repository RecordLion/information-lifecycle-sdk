using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RecordLion.RecordsManager.Client
{
    public interface IRecordsManagerClient
    {
        string Url { get; set; }

        int Timeout { get; set; }

        RecordsManagerCredentials Credentials { get; set; }

        CookieContainer CookieContainers { get; set; }

        ISecurityTokenRequestor SecurityTokenRequestor { get; set; }

        string Issuer { get; }

        SystemInfo GetSystemInfo();


        string NewRMUID();

        #region Record Classes
        
        string GetAllRecordClassesAsJson();
        

        string GetAllRecordClassesAsJson(int page, int pageSize);
        

        string GetRecordClassesAsJson(long? parentId = null);
        

        string GetRecordClassesAsJson(int page, int pageSize, long? parentId = null);
        

        string GetAllOpenRecordClassesAsJson();
        

        string GetAllOpenRecordClassesAsJson(int page, int pageSize);
        

        string GetOpenRecordClassesAsJson(long? parentId = null);
        

        string GetOpenRecordClassesAsJson(int page, int pageSize, long? parentId = null);
        

        IEnumerable<RecordClass> GetRecordClassesFromJson(string json);        
        

        IClientPagedItems<RecordClass> GetRecordClassesWithPageDataFromJson(string json);
        

        DateTime GetRecordClassesLastEdit();
        

        IEnumerable<RecordClass> SearchRecordClasses(string titleOrCode);
        

        IClientPagedItems<RecordClass> SearchRecordClasses(string titleOrCode, int page, int pageSize);
        

        IEnumerable<RecordClass> GetAllRecordClasses();
        

        IClientPagedItems<RecordClass> GetAllRecordClasses(int page, int pageSize);
        

        IEnumerable<RecordClass> GetRecordClasses(long? parentId = null);
        

        IClientPagedItems<RecordClass> GetRecordClasses(int page, int pageSize, long? parentId = null);
        

        IEnumerable<RecordClass> GetAllOpenRecordClasses();
        

        IClientPagedItems<RecordClass> GetAllOpenRecordClasses(int page, int pageSize);
        

        IEnumerable<RecordClass> GetOpenRecordClasses(long? parentId = null);
        

        IClientPagedItems<RecordClass> GetOpenRecordClasses(int page, int pageSize, long? parentId = null);
        

        RecordClass GetRecordClass(long id);
        

        RecordClass GetRecordClass(string code);
        

        RecordClass CreateRecordClass(RecordClass recordClass);


        RecordClass CreateRecordClassCaseFile(RecordClass recordClass);
        

        RecordClass UpdateRecordClass(RecordClass recordClass);
        

        void DeleteRecordClass(long id);
        
        #endregion

        
        #region Records

        DateTime GetRecordsLastEdit();

        
        string GetRecordsAsJson();

        
        string GetRecordsAsJson(int page, int pageSize);


        string GetRecordsForRecordClassAsJson(long recordClassId);


        string GetRecordsForRecordClassAsJson(long recordClassId, int page, int pageSize);


        string GetRecordsForContainerAsJson(long containerId);

        
        string GetRecordsForContainerAsJson(long containerId, int page, int pageSize);

        
        IEnumerable<Record> GetRecordsFromJson(string json);

        
        IClientPagedItems<Record> GetRecordsWithPageDataFromJson(string json);

        
        IEnumerable<Record> SearchRecords(string titleOrUri);

        
        IClientPagedItems<Record> SearchRecords(string titleOrUri, int page, int pageSize);      

        
        IEnumerable<Record> GetRecords();

        
        IClientPagedItems<Record> GetRecords(int page, int pageSize);


        IEnumerable<Record> GetRecordsForRecordClass(long recordClassId);


        IClientPagedItems<Record> GetRecordsForRecordClass(long recordClassId, int page, int pageSize);


        IEnumerable<Record> GetRecordsForContainer(long containerId);

        
        IClientPagedItems<Record> GetRecordsForContainer(long containerId, int page, int pageSize);

        
        Record GetRecord(long id);

        
        Record GetRecordByIdentifier(string identifier);

        
        Record GetRecordByUri(string uri);

        
        void DeclareRecord(string uri);

        
        void UndeclareRecord(string uri);

        
        void DeclareVital(string uri);

        
        void UndeclareVital(string uri);

        
        void DeclareObsolete(string uri);

        
        void UndeclareObsolete(string uri);

        
        void DeclareRecordById(long id);

        
        void UndeclareRecordById(long id);

        
        void DeclareVitalById(long id);

        
        void UndeclareVitalById(long id);

        
        void DeclareObsoleteById(long id);

        
        void UndeclareObsoleteById(long id);

        
        void DeclareRecordByIdentifier(string identifier);

        
        void UndeclareRecordByIdentifier(string identifier);

        
        void DeclareVitalByIdentifier(string identifier);

        
        void UndeclareVitalByIdentifier(string identifier);

        
        void DeclareObsoleteByIdentifier(string identifier);

        
        void UndeclareObsoleteByIdentifier(string identifier);

        #endregion


        #region Containers

        string GetAllContainersAsJson();
        

        string GetAllContainersAsJson(int page, int pageSize);


        string GetContainersAsJson(long? parentId = null);


        string GetContainersAsJson(int page, int pageSize, long? parentId = null);


        IEnumerable<Container> GetContainersFromJson(string json);


        IClientPagedItems<Container> GetContainersWithPageDataFromJson(string json);


        DateTime GetContainerLastEdit();


        IEnumerable<Container> SearchContainers(string title);


        IClientPagedItems<Container> SearchContainers(string title, int page, int pageSize);


        IEnumerable<Container> GetAllContainers();


        IClientPagedItems<Container> GetAllContainers(int page, int pageSize);


        IEnumerable<Container> GetContainers(long? parentId = null);


        IClientPagedItems<Container> GetContainers(int page, int pageSize, long? parentId = null);


        Container GetContainer(long id);


        Container GetContainer(string barcode);


        Container CreateContainer(Container container);


        Container UpdateContainer(Container container);


        void DeleteContainer(long id);        

        #endregion
        

        #region Barcodes
        
        string GenerateBarcode(long barcodeSchemeId);

        
        DateTime GetBarcodeSchemesLastEdit();
        

        IEnumerable<BarcodeScheme> SearchBarcodeSchemes(string title);
        

        IClientPagedItems<BarcodeScheme> SearchBarcodeSchemes(string title, int page, int pageSize);
        

        IEnumerable<BarcodeScheme> GetBarcodeSchemes();
        

        IClientPagedItems<BarcodeScheme> GetBarcodeSchemes(int page, int pageSize);
        

        BarcodeScheme GetBarcodeScheme(long id);
        

        BarcodeScheme CreateBarcodeScheme(BarcodeScheme scheme);
        

        BarcodeScheme UpdateBarcodeScheme(BarcodeScheme scheme);
        

        void DeleteBarcodeScheme(long id);
        
        #endregion

        
        #region Recordization

        IEnumerable<Record> ProcessRecordization(IEnumerable<Recordize> recordizers);


        void DeleteRecordization(string recordizedUrl, bool deleteAll);

        #endregion


        #region Triggers

        string GetTriggersAsJson();
        

        string GetTriggersAsJson(int page, int pageSize);
        

        IEnumerable<RetentionTrigger> GetTriggersFromJson(string json);


        IClientPagedItems<RetentionTrigger> GetTriggersWithPageDataFromJson(string json);


        DateTime GetTriggersLastEdit();


        IEnumerable<RetentionTrigger> SearchTriggers(string title);


        IClientPagedItems<RetentionTrigger> SearchTriggers(string title, int page, int pageSize);


        IEnumerable<RetentionTrigger> GetTriggers();


        IClientPagedItems<RetentionTrigger> GetTriggers(int page, int pageSize);


        RetentionTrigger GetTrigger(long id);


        RetentionTrigger CreateTrigger(RetentionTrigger trigger);


        RetentionTrigger UpdateTrigger(RetentionTrigger trigger);


        void DeleteTrigger(long id);

        #endregion


        #region Retentions

        string GetRetentionsAsJson();

        
        string GetRetentionsAsJson(int page, int pageSize);

        
        IEnumerable<Retention> GetRetentionsFromJson(string json);
        

        IClientPagedItems<Retention> GetRetentionsWithPageDataFromJson(string json);
        

        DateTime GetRetentionsLastEdit();
        

        IEnumerable<Retention> GetRetentions();
        

        IClientPagedItems<Retention> GetRetentions(int page, int pageSize);
        

        IEnumerable<Retention> SearchRetentions(string titleOrAuthority);
        

        IClientPagedItems<Retention> SearchRetentions(string titleOrAuthority, int page, int pageSize);
        

        Retention GetRetention(long id);
        

        Retention CreateRetention(Retention retention);


        Retention UpdateRetention(Retention retention);
        

        void DeleteRetention(long id);
        
        #endregion

        
        #region Lifecyles

        string GetLifecylesAsJson();


        string GetLifecylesAsJson(int page, int pageSize);


        IEnumerable<Lifecycle> GetLifecylesFromJson(string json);


        IClientPagedItems<Lifecycle> GetLifecyclesWithPageDataFromJson(string json);

        
        DateTime GetLifecyclesLastEdit();

        
        IEnumerable<Lifecycle> SearchLifecycles(string title);

        
        IClientPagedItems<Lifecycle> SearchLifecycles(string title, int page, int pageSize);

        
        IEnumerable<Lifecycle> GetLifecycles();

        
        IClientPagedItems<Lifecycle> GetLifecycles(int page, int pageSize);

        
        Lifecycle GetLifecycle(long id);

        
        List<LifecyclePhaseSummary> GetLifecyclesSummary(long id);

        
        Lifecycle CreateLifecycle(Lifecycle lifecycle);


        Lifecycle UpdateLifecycle(Lifecycle lifecycle);

        
        void DeleteLifecycle(long id);

        #endregion


        #region RecordClass Lifecyles

        string GetRecordClassLifecyclesAsJson();
        

        string GetRecordClassLifecyclesAsJson(int page, int pageSize);
        

        IEnumerable<RecordClassLifecycle> GetRecordClassLifecyclesFromJson(string json);


        IClientPagedItems<RecordClassLifecycle> GetRecordClassLifecyclesWithPageDataFromJson(string json);


        DateTime GetRecordClassLifecyclesLastEdit();


        IEnumerable<RecordClassLifecycle> SearchRecordClassLifecycles(string title);


        IClientPagedItems<RecordClassLifecycle> SearchRecordClassLifecycles(string title, int page, int pageSize);


        IEnumerable<RecordClassLifecycle> GetRecordClassLifecycles();


        IClientPagedItems<RecordClassLifecycle> GetRecordClassLifecycles(int page, int pageSize);


        RecordClassLifecycle GetRecordClassLifecycle(long id);


        RecordClassLifecycle CreateRecordClassLifecycle(RecordClassLifecycle recordClassLifecycle);


        RecordClassLifecycle UpdateRecordClassLifecycle(RecordClassLifecycle recordClassLifecycle);


        void DeleteRecordClassLifecycle(long id);

        #endregion
        

        #region Event Occurrences
        
        string GetEventOccurrencesAsJson();

        
        string GetEventOccurrencesAsJson(int page, int pageSize);

        
        IEnumerable<EventOccurrence> GetEventOccurrencesFromJson(string json);

        
        IClientPagedItems<EventOccurrence> GetEventOccurrencesWithPageDataFromJson(string json);
        

        DateTime GetEventOccurrencesLastEdit();
        

        IEnumerable<EventOccurrence> SearchEventOccurrences(string eventTitle);
        

        IClientPagedItems<EventOccurrence> SearchEventOccurrences(string eventTitle, int page, int pageSize);
        

        IEnumerable<EventOccurrence> GetEventOccurrences();
        

        IClientPagedItems<EventOccurrence> GetEventOccurrences(int page, int pageSize);
        

        EventOccurrence GetEventOccurrences(long id);
        

        EventOccurrence CreateEventOccurrence(EventOccurrence eventOccurrence);
        

        void DeleteEventOccurrence(long id);
        
        #endregion

        
        #region Audit

        string GetAuditsAsJson();


        string GetAuditsAsJson(int page, int pageSize);


        IEnumerable<AuditEntry> GetAuditsFromJson(string json);


        IClientPagedItems<AuditEntry> GetAuditsWithPageDataFromJson(string json);

        
        DateTime GetAuditsLastEdit();

        
        IEnumerable<AuditEntry> SearchAudits(DateTime rangeStart, DateTime rangeEnd);

        
        IClientPagedItems<AuditEntry> SearchAudits(DateTime rangeStart, DateTime rangeEnd, int page, int pageSize);

        
        IEnumerable<AuditEntry> GetAudits();

        
        IClientPagedItems<AuditEntry> GetAudits(int page, int pageSize);

        
        IEnumerable<AuditEntry> GetAudits(AuditTarget target, long targetId);

        
        IClientPagedItems<AuditEntry> GetAudits(AuditTarget target, long targetId, int page, int pageSize);

        
        IEnumerable<AuditEntry> GetAuditsForRecord(string recordUri);

        
        IClientPagedItems<AuditEntry> GetAuditsForRecord(string recordUri, int page, int pageSize);

        
        AuditEntry CreateAudit(NewAuditEntry auditEntry);

        #endregion


        #region Heartbeat

        HeartbeatResult Heartbeat(Heartbeat heartbeat);
        

        HeartbeatResult Heartbeat(Heartbeat heartbeat, bool handlePrimary);
        
        #endregion

        
        #region Legal Cases

        string GetLegalCasesAsJson();

        
        string GetLegalCasesAsJson(int page, int pageSize);

        
        string GetOpenLegalCasesAsJson();

        
        string GetOpenLegalCasesAsJson(int page, int pageSize);

        
        IEnumerable<LegalCase> GetLegalCasesFromJson(string json);
        

        IClientPagedItems<LegalCase> GetLegalCasesWithPageDataFromJson(string json);
        

        DateTime GetLegalCasesLastEdit();
        

        IEnumerable<LegalCase> SearchLegalCases(string titleOrNumber);
        

        IClientPagedItems<LegalCase> SearchLegalCases(string titleOrNumber, int page, int pageSize);
        

        IEnumerable<LegalCase> GetLegalCases();
        

        IClientPagedItems<LegalCase> GetLegalCases(int page, int pageSize);
        

        IEnumerable<LegalCase> GetOpenLegalCases();
        

        IClientPagedItems<LegalCase> GetOpenLegalCases(int page, int pageSize);
        

        LegalCase GetLegalCase(long id);
        

        LegalCase CreateLegalCase(LegalCase legalCase);
        

        LegalCase UpdateLegalCase(LegalCase legalCase);
        

        void DeleteLegalCase(long id);
        
        #endregion

        
        #region Legal Holds

        string GetLegalHoldsAsJson();


        string GetOpenLegalHoldsAsJson();


        IEnumerable<LegalHold> GetLegalHoldsFromJson(string json);


        IClientPagedItems<LegalHold> GetLegalHoldsWithPageDataFromJson(string json);


        IEnumerable<LegalHold> GetLegalHolds();

        
        IClientPagedItems<LegalHold> GetLegalHolds(int page, int pageSize);

        
        IEnumerable<LegalHold> GetOpenLegalHolds();

        
        IClientPagedItems<LegalHold> GetOpenLegalHolds(int page, int pageSize);

        
        IEnumerable<LegalHold> SearchOpenLegalHolds(string uri);

        
        IClientPagedItems<LegalHold> SearchOpenLegalHolds(string uri, int page, int pageSize);

        
        IEnumerable<LegalHold> SearchLegalHolds(string uri);

        
        IClientPagedItems<LegalHold> SearchLegalHolds(string uri, int page, int pageSize);

        
        IEnumerable<LegalHold> SearchLegalHolds(long legalCaseId);

        
        IClientPagedItems<LegalHold> SearchLegalHolds(long legalCaseId, int page, int pageSize);

        
        LegalHold GetLegalHoldById(long id);

        
        LegalHold CreateLegalHold(LegalHold legalHold);

        
        LegalHold UpdateLegalHold(LegalHold legalHold);

        
        void DeleteLegalHold(long legalCaseId, string uri);

        
        void DeleteLegalHold(long id);

        #endregion


        #region Action Items

        IEnumerable<ActionItem> GetActionItemsFromJson(string json);
        

        IClientPagedItems<ActionItem> GetActionItemsWithPageDataFromJson(string json);
        

        DateTime GetActionItemsLastEdit();
        

        ActionItem GetActionItem(long id);
        

        ActionItem GetActionItemForRecord(long recordId);
        

        ActionItem GetActionItemForRecord(string identifier);
        

        ActionItem UpdateActionItem(ActionItem actionItem);

        #region Pending Action Items
        
        string GetPendingActionItemsAsJson();

        
        string GetPendingActionItemsAsJson(int page, int pageSize);

        
        IEnumerable<ActionItem> SearchPendingActionItemsByTitleOrUri(string recordTitleOrUri);

        
        IClientPagedItems<ActionItem> SearchPendingActionItemsByTitleOrUri(string recordTitleOrUri, int page, int pageSize);


        IEnumerable<ActionItem> SearchPendingActionItemsByUri(string recordUri);


        IClientPagedItems<ActionItem> SearchPendingActionItemsByUri(string recordUri, int page, int pageSize);

        
        IEnumerable<ActionItem> SearchPendingSystemActionItemsByUri(string recordUri);


        IClientPagedItems<ActionItem> SearchPendingSystemActionItemsByUri(string recordUri, int page, int pageSize);

        
        IEnumerable<ActionItem> GetPendingActionItems();

        
        IClientPagedItems<ActionItem> GetPendingActionItems(int page, int pageSize);
        
        
        ActionItem UpdatePendingActionItemCompleted(long id);
        
        
        ActionItem UpdatePendingActionItemFailed(long id);
        
        
        ActionItem UpdatePendingActionItemUnsupported(long id);
        
        #endregion

        
        #region Inbox Action Items

        string GetInboxActionItemsAsJson();
        

        string GetInboxActionItemsAsJson(int page, int pageSize);


        IEnumerable<InboxActionItem> GetInboxActionItemsFromJson(string json);


        IClientPagedItems<InboxActionItem> GetInboxActionItemsWithPageDataFromJson(string json);
        

        IEnumerable<InboxActionItem> SearchInboxActionItems(string title);


        IClientPagedItems<InboxActionItem> SearchInboxActionItems(string title, int page, int pageSize);


        IEnumerable<InboxActionItem> GetInboxActionItems();


        IClientPagedItems<InboxActionItem> GetInboxActionItems(int page, int pageSize);


        IClientPagedItems<ActionItem> GetInboxActionItemsForCase(InboxActionItem caseItem, int page, int pageSize);


        void UpdateInboxActionItemApprove(long id);


        void UpdateInboxActionItemDismiss(long id);


        void UpdateInboxActionItemRetry(long id);


        void UpdateInboxActionItemCompleted(long id);


        void UpdateInboxCaseApprove(InboxActionItem caseItem);


        void UpdateInboxCaseDismiss(InboxActionItem caseItem);


        void UpdateInboxCaseRetry(InboxActionItem caseItem);


        #endregion

        #endregion


        #region Record Requests

        string GetRecordRequestsAsJson();


        string GetRecordRequestsAsJson(int page, int pageSize);


        IEnumerable<RecordRequest> GetRecordRequestsFromJson(string json);


        IClientPagedItems<RecordRequest> GetRecordRequestsWithPageDataFromJson(string json);


        DateTime GetRecordRequestsLastEdit();


        IEnumerable<RecordRequest> GetRecordRequests();


        IClientPagedItems<RecordRequest> GetRecordRequests(int page, int pageSize);


        IEnumerable<RecordRequest> SearchRecordRequests(string title);


        IClientPagedItems<RecordRequest> SearchRecordRequests(string title, int page, int pageSize);


        RecordRequest GetRecordRequest(long id);


        RecordRequest CreateRecordRequest(RecordRequest request);


        RecordRequest UpdateRecordRequest(RecordRequest request);


        void DeleteRecordRequest(long id);


        #region Inbox Record Requests

        string GetInboxRecordRequestsAsJson();


        string GetInboxRecordRequestsAsJson(int page, int pageSize);


        IEnumerable<RecordRequest> SearchInboxRecordRequests(string title);


        IClientPagedItems<RecordRequest> SearchInboxRecordRequests(string title, int page, int pageSize);


        IEnumerable<RecordRequest> GetInboxRecordRequests();


        IClientPagedItems<RecordRequest> GetInboxRecordRequests(int page, int pageSize);


        RecordRequest CloseRecordRequest(long id);


        RecordRequest CloseAndFulfillRecordRequest(long id);


        #endregion

        #endregion


        #region Managed Properties

        string GetManagedPropertiesAsJson();


        string GetManagedPropertiesAsJson(int page, int pageSize);


        IEnumerable<ManagedProperty> GetManagedPropertiesFromJson(string json);


        IClientPagedItems<ManagedProperty> GetManagedPropertiesWithPageDataFromJson(string json);


        DateTime GetManagedPropertiesLastEdit();


        IEnumerable<ManagedProperty> SearchManagedProperties(string name);


        IClientPagedItems<ManagedProperty> SearchManagedProperties(string name, int page, int pageSize);


        IEnumerable<ManagedProperty> GetManagedProperties();


        IClientPagedItems<ManagedProperty> GetManagedProperties(int page, int pageSize);


        ManagedProperty GetManagedProperty(long id);


        ManagedProperty CreateManagedProperty(ManagedProperty property);


        ManagedProperty UpdateManagedProperty(ManagedProperty property);


        void DeleteManagedProperty(long id);

        #endregion
    }
}