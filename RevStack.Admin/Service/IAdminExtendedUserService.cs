using System;
using RevStack.Identity.Mvc;
using RevStack.Commerce;

namespace RevStack.Admin
{
    public interface IAdminExtendedUserService<TUserModel,TProfile,TCreateProfile, TOrder, TPayment, TKey> : IAdminUserService<TUserModel,TProfile,TCreateProfile,TKey>
        where TUserModel : class, IAdminUserModel<TKey>
        where TProfile : class, IProfileModel<TKey>
        where TCreateProfile : class, ICreateProfileModel<TKey>
        where TOrder : class, IOrder<TPayment,TKey>
        where TPayment : class, IPayment
    {

    }
}
