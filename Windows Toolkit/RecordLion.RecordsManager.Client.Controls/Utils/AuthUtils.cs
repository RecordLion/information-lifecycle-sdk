using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace RecordLion.RecordsManager.Client.Controls.Utils
{
    internal static class AuthUtils
    {
        private const Int32 InternetCookieHttponly = 0x2000;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(string url, string cookieName, StringBuilder cookieData, ref int size, Int32 dwFlags, IntPtr lpReserved);



        public static string GetSignInUrl(string recordsManagerUrl)
        {
            return string.Format("{0}?api=true", recordsManagerUrl);
        }

        public static bool IsSignedIn(string recordsManagerUrl, string currentUrl)
        {           
            var uri = new Uri(currentUrl, UriKind.Absolute);            

            return recordsManagerUrl.ToLower().Contains(uri.Host) &&
                   currentUrl.ToLower().Contains("auth/signincomplete");
        }

        public static bool IsSignedOut(string recordsManagerUrl, string currentUrl)
        {
            var uri = new Uri(currentUrl, UriKind.Absolute);

            return recordsManagerUrl.ToLower().Contains(uri.Host) &&
                   currentUrl.ToLower().Contains("auth/signout");
        }

        public static CookieContainer GetCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;

            // Determine the size of the cookie
            int datasize = 8192 * 16;

            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);

                if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }

            return cookies;
        }
    }
}
