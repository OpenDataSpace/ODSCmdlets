using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace OpenDataSpace.Commands
{
    static internal class Utility
    {
        public static SecureString StringToSecureString(string str)
        {
            var ss = new SecureString();
            foreach (char c in str.ToCharArray())
            {
                ss.AppendChar(c);
            }
            ss.MakeReadOnly();
            return ss;
        }

        public static string SecureStringToString(SecureString secureStr)
        {
            var bstr = Marshal.SecureStringToBSTR(secureStr);
            try
            {
                string str = Marshal.PtrToStringBSTR(bstr);
                return str;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
        }
    }
}
