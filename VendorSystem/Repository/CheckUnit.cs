using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace VendorSystem.Repository
{
    public static class CheckUnit
    {

        public static string RetriveCorrectMsg(string ArMsg, string EngMsg)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            if (Lang == "ar-SA")
            {
                return ArMsg;
            }
            return EngMsg;
        }

        public static string RetriveCorrectMsg(string ArMsg, string EngMsg, string Lang)
        {
            if (Lang == "ar-SA")
            {
                return ArMsg;
            }
            return EngMsg;
        }
    }
}