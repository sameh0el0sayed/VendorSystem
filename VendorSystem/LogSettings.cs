using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using System.Net.NetworkInformation;
using VendorSystem.Models.Model1;

namespace VendorSystem
{
    public static class LogSettings
    {

        public static void InsertInLog(string TableName, Operation Type, BayanEntities _context, decimal TransactionId = 0)
        {
            try
            {
                string TypeName = "";
                if (Type == Operation.Insert)
                {
                    TypeName = "Insert";
                }
                else if (Type == Operation.Update)
                {
                    TypeName = "Update";
                }
                else
                {
                    TypeName = "Delete";
                }
                _context.Database.ExecuteSqlCommand("Insert into Log." + TableName + " Values({0},{1},{2},{3},{4})", TypeName, TransactionId, 1, DateTime.Now, GetMacAddress());
                _context.SaveChanges();
            }
            catch
            {


            }
        }
        public static void InsertInLog(string TableName, Operation Type, BayanEntities _context, string TransactionId = "")
        {
            try
            {
                string TypeName = "";
                if (Type == Operation.Insert)
                {
                    TypeName = "Insert";
                }
                else if (Type == Operation.Update)
                {
                    TypeName = "Update";
                }
                else
                {
                    TypeName = "Delete";
                }
                _context.Database.ExecuteSqlCommand("Insert into Log." + TableName + " Values({0},{1},{2},{3},{4})", TypeName, TransactionId, 1, DateTime.Now, GetMacAddress());
                _context.SaveChanges();
            }
            catch
            {


            }
        }
        public static string GetMacAddress()
        {
            string Mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        Mac = nic.GetPhysicalAddress().ToString();
                    }
                }

            }

            return Mac;
        }
    }
    public enum Operation
    {
        Insert, Update, Delete
    }
}