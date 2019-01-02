using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordLion.RecordsManager.Client
{
    public static class Extensions
    {
        public static string FormatResourceUrl(this string str, params object[] args)
        {
            string[] encoded = null;

            if (args != null)
            {
                encoded = new string[args.Length];

                for(int i = 0; i < args.Length; i++)
                {
                    encoded[i] = (args[i] != null) ? Uri.EscapeDataString(args[i].ToString()) : null;
                }
            }

            return string.Format(str, encoded);
        }
    }
}
