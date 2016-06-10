using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordClass
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string Organization { get; set; }

        public string Notes { get; set; }

        public int Priority { get; set; }

        public bool ArchiveRecords { get; set; }

        public bool ArchiveRecordProperties { get; set; }
        
        public bool ArchiveRecordAudits { get; set; }

        public RecordClassRecordDeclarationRule RecordDeclarationRule { get; set; }

        public RecordClassVitalRule VitalRule { get; set; }

        public Rule[] ClassificationRules { get; set; }
        
        public DateTime? FirstUsedDate { get; set; }

        public DateTime? OriginatedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsSystem { get; set; }

        public bool HasChildren { get; set; }

        public int Count { get; set; }

        public int? ExpectedMonthlyVolume { get; set; }

        public bool? IsCaseBased { get; set; }

        public bool? IsCaseFile { get; set; }

        public string CaseFileRule { get; set; }
    }
}