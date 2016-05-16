using System;
using RevStack.Identity.Mvc;

namespace RevStack.Admin
{
    public class AdminUserModel<TKey> : ProfileBaseModel<TKey>,IAdminUserModel<TKey>
    {
        public int OrderCount { get; set; }

        public AdminUserModel() : base()
        {
            OrderCount = 0;
        }
    }

   

}