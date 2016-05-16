using System;
using RevStack.Identity.Mvc;

namespace RevStack.Admin
{
    public interface IAdminUserModel<TKey> : IProfileModel<TKey>
    {
        int OrderCount { get; set; }
    }
}
