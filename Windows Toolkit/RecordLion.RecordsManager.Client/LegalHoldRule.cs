namespace RecordLion.RecordsManager.Client
{
    public class LegalHoldRule
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public string Property { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }

        public string DataType { get; set; }

        public bool IsCaseSensitive { get; set; }

        public string Join { get; set; }

        public long LegalCaseId { get; set; }
    }
}