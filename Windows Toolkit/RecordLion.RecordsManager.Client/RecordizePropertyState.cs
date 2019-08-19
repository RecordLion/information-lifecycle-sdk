using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordLion.RecordsManager.Client
{
    public enum RecordizePropertyState
    {
        Default,    //Overwritten with each Property Bag Update
        Static,     //Overwritten if provided in Property Bag, but never deleted
        Constant    //Never deleted or overwritten
    }
}
