using System;
using System.Configuration;

namespace RevStack.Admin
{
    public static class Settings
    {
        public static string UserName
        {
            get
            {
                string username = ConfigurationManager.AppSettings["Admin.Username"];
                if (!string.IsNullOrEmpty(username)) return username;
                else return "admin@revstack.io";
            }
        }

        public static string Password
        {
            get
            {
                string password = ConfigurationManager.AppSettings["Admin.Password"];
                if (!string.IsNullOrEmpty(password)) return password;
                else return "admin!";
            }
        }
        public static string Name
        {
            get
            {
                string name = ConfigurationManager.AppSettings["Admin.Name"];
                if (!string.IsNullOrEmpty(name)) return name;
                else return "Admin";
            }
        }
        public static bool AllowOrderDelete
        {
            get
            {
                string allow = ConfigurationManager.AppSettings["Admin.Order.Delete"];
                if (!string.IsNullOrEmpty(allow)) return Convert.ToBoolean(allow);
                else return true;
            }
        }
        public static bool AllowUserDelete
        {
            get
            {
                string allow = ConfigurationManager.AppSettings["Admin.User.Delete"];
                if (!string.IsNullOrEmpty(allow)) return Convert.ToBoolean(allow);
                else return true;
            }
        }
        public static string CompanyName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["Admin.Company.Notification.Name"];
                if (!string.IsNullOrEmpty(result)) return result;
                else return "XYZ, Inc";
            }
        }
        public static string CompanyEmail
        {
            get
            {
                string result = ConfigurationManager.AppSettings["Admin.Company.Notification.Email"];
                if (!string.IsNullOrEmpty(result)) return result;
                else return "dev.null@localhost";
            }
        }
        public static string CompanySms
        {
            get
            {
                string result = ConfigurationManager.AppSettings["Admin.Company.Notification.Sms"];
                if (!string.IsNullOrEmpty(result)) return result;
                else return "0000000000";
            }
        }
    }
}