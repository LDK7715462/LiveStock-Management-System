using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal class Util // Validate Data & Connection
    {
        internal static readonly string BAD_STRING = string.Empty;
        internal static readonly int BAD_INT = Int32.MinValue;
        internal static readonly double BAD_DOUBLE = Double.MinValue;
        internal static int GetInt(object o)
        {
            if (o == null) return BAD_INT;
            int n;
            if (int.TryParse(o.ToString(), out n) == false)
                return BAD_INT;
            return n;
        }

        internal static double GetDouble(object o)
        {
            if (o == null) return BAD_DOUBLE;
            double n;
            if (double.TryParse(o.ToString(), out n) == false)
                return BAD_DOUBLE;
            return n;
        }

        internal static string GetString(object o)
        {
            return o?.ToString() ?? BAD_STRING;
        }

        internal static OdbcConnection GetConn() // Connection to the Database
        {
            string? dbstr = ConfigurationManager.AppSettings.Get("odbcString");
            string fpath = @"..\..\FarmData.accdb";
            string connstr = dbstr + fpath;
            var conn = new OdbcConnection(connstr);
            conn.Open();
            return conn;
        }
    }
}
