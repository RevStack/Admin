using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStack.Identity.Mvc;

namespace RevStack.Admin
{
    public interface IAdminUserService<TUserModel,TProfile,TCreateProfile,TKey>
        where TUserModel : class, IAdminUserModel<TKey>
        where TProfile: class, IProfileModel<TKey>
        where TCreateProfile : class, ICreateProfileModel<TKey>
    {
        IEnumerable<TUserModel> Get();
        Task<IEnumerable<TUserModel>> GetAsync();
        TUserModel Get(TKey id);
        Task<TUserModel> GetAsync(TKey id);
        Task<Tuple<TCreateProfile,bool,string>> AddAsync(TCreateProfile entity);
        TProfile Update(TProfile entity);
        Task<TProfile> UpdateAsync(TProfile entity);
        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id);
        bool Validate(TCreateProfile entity);
        Task<bool> ValidateAsync(TCreateProfile entity);

    }
}