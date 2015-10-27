using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public enum LifecyclePhaseAction
    {
        None,
        Transfer,
        Workflow,
        DeclareRecord,
        UndeclareRecord,
        DisposeDelete,
        DisposeTransfer,
        Permanent,
        DisposeRecycle
    }
}