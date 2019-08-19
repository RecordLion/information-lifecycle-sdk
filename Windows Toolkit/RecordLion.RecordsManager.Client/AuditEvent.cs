using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public enum AuditEvent
    {
        Other,
        Create,
        Update,
        Delete,
        View,
        Action,
        Custody
    }
}