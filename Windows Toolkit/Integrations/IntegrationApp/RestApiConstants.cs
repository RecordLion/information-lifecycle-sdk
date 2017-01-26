namespace IntegrationApp
{
    public static class RestApiV1Constants
    {
        public const string HEADER_CONTENTTYPE = "application/json";

        //Record Class Endpoints...
        public const string GET_RECORDCLASSES_LASTEDIT = "/api/v1/recordclasses?lastedit";

        //Event Occurrence Endpoints...
        public const string GET_EVENTOCCURRENCES_CONTAINING_EVENTTITLE = "/api/v1/eventoccurrences?eventTitle={0}&page={1}&pageSize={2}";
        public const string POST_EVENTOCCURRENCE = "/api/v1/eventoccurrences";

        //Event Trigger Endpoints...
        public const string GET_TRIGGERS_CONTAINING_TITLE = "/api/v1/triggers?title={0}&page={1}&pageSize={2}";
        public const string GET_TRIGGERS_ALL = "/api/v1/triggers?page={0}&pageSize={1}";
        public const string GET_TRIGGER_WITH_ID = "/api/v1/triggers/{0}";
    }
}
