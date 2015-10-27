using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace RecordLion.RecordsManager.Client
{
    [Serializable]
    public class UpdateException : Exception
    {
        #region Fields
        
        private const string ERRORSKEY = "UpdateException_Errors";
        private const string ERRORDETAILKEY = "UpdateException_ErrorDetail";
        
        #endregion
        
        #region Constructors
        
        public UpdateException() : this(null, null)
        {
        }

        
        public UpdateException(string message) : this(message, null)
        {
        }

        
        public UpdateException(string message, IList<UpdateError> errors) : base(message)
        {
            this.Errors = (errors != null) ? errors : new List<UpdateError>();
        }

        
        protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Errors = (IList<UpdateError>)info.GetValue(ERRORSKEY, typeof(IList<UpdateError>));
            
            this.ErrorDetail = info.GetString(ERRORDETAILKEY);
        }

        #endregion

        #region Properties

        public IList<UpdateError> Errors { get; private set; }

        public string ErrorDetail { get; set; }

        #endregion

        #region Overrides

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(ERRORSKEY, this.Errors);

            info.AddValue(ERRORDETAILKEY, this.ErrorDetail);
        }
        
        #endregion
    }
}