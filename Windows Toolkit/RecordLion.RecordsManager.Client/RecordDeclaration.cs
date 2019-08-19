using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordDeclaration
    {
        public RecordDeclaration()
        {
            this.Record = RecordDeclarationState.None;
            this.Vital = RecordDeclarationState.None;
            this.Obsolete = RecordDeclarationState.None;
            this.Superseded = RecordDeclarationState.None;
        }


        public RecordDeclarationState Record { get; set; }

        public RecordDeclarationState Vital { get; set; }

        public RecordDeclarationState Obsolete { get; set; }

        public RecordDeclarationState Superseded { get; set; }
    }
}