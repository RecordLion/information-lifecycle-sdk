using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.Client
{
    internal static class Constants
    {
        public static string AZ_URL_RLIL = "https://app.recordlion.net";
        public static string AZ_URL_RLIL_TEST = "https://localhost:44307";

        public static string WAAD_TENANT = "8dd9cc07-835d-40e8-8efa-dd4b73313022";
        public static string WAAD_CLIENTID = "e81d929a-6776-4a82-b849-7ba55c5131a4";
        public static string WAAD_AUTHORITY = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}", WAAD_TENANT);
    }
}
