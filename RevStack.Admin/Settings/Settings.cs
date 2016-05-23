using System;
using RevStack.Configuration;

namespace RevStack.Admin
{
    public static class Settings
    {
        public static string UserName
        {
            get
            {
                return Configuration.Admin.UserName;
            }
        }

        public static string Password
        {
            get
            {
                return Configuration.Admin.Password;
            }
        }
        public static string Name
        {
            get
            {
                return Configuration.Admin.Name;
            }
        }
        public static bool AllowOrderDelete
        {
            get
            {
                return Configuration.Admin.AllowOrderDelete;
            }
        }
        public static bool AllowUserDelete
        {
            get
            {
                return Configuration.Admin.AllowUserDelete;
            }
        }
        public static string CompanyName
        {
            get
            {
                return Company.Name;
            }
        }
        public static string CompanyEmail
        {
            get
            {
                return Company.NotificationEmail;
            }
        }
        public static string CompanySms
        {
            get
            {
                return Company.NotificationSms;
            }
        }
    }
}