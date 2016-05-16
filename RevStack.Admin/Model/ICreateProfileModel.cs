using System;
using RevStack.Identity.Mvc;

namespace RevStack.Admin
{
    public interface ICreateProfileModel<TKey> : IProfileModel<TKey>
    {
        string Password { get; set; }
        string ConfirmPassword { get; set; }
    }
}
