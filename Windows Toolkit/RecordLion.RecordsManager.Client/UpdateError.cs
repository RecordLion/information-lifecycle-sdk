using System;
using System.Diagnostics;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    [Serializable]
    [DebuggerDisplay("Member = \"{Member}\" Message = \'{Message}\"")]
    public class UpdateError
    {
        public UpdateError()
        {
        }


        public UpdateError(string message)
        {
            this.Message = message;
        }


        public UpdateError(string member, string message)
        {
            this.Member = member;
            this.Message = message;
        }


        public string Member { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Member))
            {
                return this.Message;
            }
            else
            {
                return string.Format("Member: {0} - Message: {1}", this.Member, this.Message ?? string.Empty);
            }
        }
    }
}