using System;
using System.Threading.Tasks;


namespace RevStack.Admin
{
    public interface IAdminLoginService
    {
        bool Login(AdminLoginModel model);
        AdminProfileModel GetProfile();
        Task<AdminProfileModel> GetProfileAsync();
    }
}