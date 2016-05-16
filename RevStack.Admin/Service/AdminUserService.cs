using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Identity;
using RevStack.Identity.Mvc;
using RevStack.Pattern;
using RevStack.Mvc;

using AutoMapper;

namespace RevStack.Admin
{
   
    public class AdminUserService<TUser,TUserModel,TProfile,TCreateProfile,TUserManager,TKey> : IAdminUserService<TUserModel,TProfile,TCreateProfile,TKey>
        where TUser : class, IIdentityUser<TKey>
        where TUserModel : class, IAdminUserModel<TKey>
        where TProfile : class, IProfileModel<TKey>
        where TCreateProfile : class, ICreateProfileModel<TKey>
        where TUserManager : ApplicationUserManager<TUser, TKey>
        where TKey : IEquatable<TKey>, IConvertible
    {
        protected IRepository<TUser, TKey> _repository;
        protected Func<ApplicationUserManager<TUser, TKey>> _userManagerFactory;
        protected Func<TUser> _applicationUserFactory;
       
        public AdminUserService(IRepository<TUser, TKey> repository, 
            Func<ApplicationUserManager<TUser, TKey>> userManagerFactory,
            Func<TUser> applicationUserFactory
            )
        {
            _repository = repository;
            _userManagerFactory = userManagerFactory;
            _applicationUserFactory = applicationUserFactory;
        }

        public virtual IEnumerable<TUserModel> Get()
        {
            return _repository.Get().Select(x => Mapper.Map<TUserModel>(x));
        }

        public virtual Task<IEnumerable<TUserModel>> GetAsync()
        {
            return Task.FromResult(Get());
        }

        public virtual TUserModel Get(TKey id)
        {
            var result = _repository.Find(x => x.Compare(x.Id,id)).FirstOrDefault();
            return Mapper.Map<TUserModel>(result);
        }

        public virtual Task<TUserModel> GetAsync(TKey id)
        {
            return Task.FromResult(Get(id));
        }

        public virtual TProfile Update(TProfile entity)
        {
            var user = _repository.Find(x => x.Compare(x.Id,entity.Id));
            if(user.Any())
            {
                var updatedUser = user.FirstOrDefault();
                updatedUser = updatedUser.CopyPropertiesFrom(entity,true);
                _repository.Update(updatedUser);
            }
            return entity;
        }

        public virtual Task<TProfile> UpdateAsync(TProfile entity)
        {
            return Task.FromResult(Update(entity));
        }

        public virtual bool Delete(TKey id)
        {
            var result = _repository.Find(x => x.Compare(x.Id,id));
            if (result.Any())
            {
                var user = result.FirstOrDefault();
                _repository.Delete(user);
            }

            return true;
        }

        public virtual Task<bool> DeleteAsync(TKey id)
        {
            return Task.FromResult(Delete(id));
        }

       
        public async Task<Tuple<TCreateProfile,bool,string>> AddAsync(TCreateProfile entity)
        {
            
            var user = _applicationUserFactory();
            user = user.CopyPropertiesFrom(entity,true);
            var userManager = _userManagerFactory();
            var result=await userManager.CreateAsync(user, entity.Password);
            if(!result.Succeeded)
            {
                string errors = result.Errors.FirstOrDefault();
                return new Tuple<TCreateProfile, bool, string>(entity, false, errors);
            }
            return new Tuple<TCreateProfile, bool, string>(entity, true, null);
        }

        public bool Validate(TCreateProfile entity)
        {
            var existing = _repository.Find(x => x.Email == entity.Email);
            if(existing.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Task<bool> ValidateAsync(TCreateProfile entity)
        {
            return Task.FromResult(Validate(entity));
        }
    }
}