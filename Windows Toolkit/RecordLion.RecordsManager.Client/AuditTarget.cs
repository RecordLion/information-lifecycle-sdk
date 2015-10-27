using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public enum AuditTarget
    {
        Record,
        RecordClass,
        Trigger,
        Retention,
        Lifecycle,
        EventOccurrence,
        LegalCase,
        LegalHold,
        Container,
        BarcodeScheme,
        Barcode
    }
}