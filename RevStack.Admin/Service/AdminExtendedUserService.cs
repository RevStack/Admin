using System;
using System.Collections.Generic;
using System.Linq;
using RevStack.Identity;
using RevStack.Identity.Mvc;
using RevStack.Pattern;
using RevStack.Commerce;
using RevStack.Mvc;
using AutoMapper;

namespace RevStack.Admin
{
    public class AdminExtendedUserService<TUser, TUserModel, TProfile, TCreateProfile, TUserManager,TOrder,TPayment, TKey> : AdminUserService<TUser,TUserModel,TProfile,TCreateProfile,TUserManager,TKey>
        where TUser : class, IIdentityUser<TKey>
        where TUserModel : class, IAdminUserModel<TKey>
        where TProfile : class, IProfileModel<TKey>
        where TCreateProfile : class, ICreateProfileModel<TKey>
        where TUserManager : ApplicationUserManager<TUser, TKey>
        where TOrder : class, IOrder<TPayment,TKey>
        where TPayment : class, IPayment
        where TKey : IEquatable<TKey>, IConvertible
    {
        private readonly IRepository<TOrder, TKey> _orderRepository;
        private readonly Func<TUserModel> _userModelFactory;
        public AdminExtendedUserService(IRepository<TUser, TKey> repository, 
            Func<ApplicationUserManager<TUser, TKey>> userManagerFactory, 
            Func<TUser> applicationUserFactory, 
            IRepository<TOrder, TKey> orderRepository,
            Func<TUserModel> userModelFactory) : base(repository,userManagerFactory, applicationUserFactory)
        {
            _orderRepository = orderRepository;
            _userModelFactory = userModelFactory;
        }

        public override IEnumerable<TUserModel> Get()
        {
            var result = _repository.Get().Select(x => Mapper.Map<TUserModel>(x)).ToList();
            result.ForEach(y=>y.OrderCount=_orderRepository.Find(c=>c.Compare(c.UserId,y.Id)).Count());
            return result;
        }

        public override TUserModel Get(TKey id)
        {
            var result = _repository.Find(x => x.Compare(x.Id,id)).FirstOrDefault();
            TUserModel model;
            if(result==null)
            {
                model = _userModelFactory();
                model.OrderCount = 1;
                return model;
            }
            model= Mapper.Map<TUserModel>(result);
            model.OrderCount = _orderRepository.Find(x => x.Compare(x.UserId,id)).Count();
            return model;
        }
    }
}