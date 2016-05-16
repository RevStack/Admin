using System;
using System.Threading.Tasks;

namespace RevStack.Admin
{
    
    public class AdminLoginService : IAdminLoginService
    {
       
        public bool Login(AdminLoginModel model)
        {
            if (model.Username.ToLower() == Settings.UserName.ToLower() && model.Password == Settings.Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AdminProfileModel GetProfile()
        {
            var profile = new AdminProfileModel
            {
                Name = Settings.Name
            };

            return profile;
        }

        public Task<AdminProfileModel> GetProfileAsync()
        {
            return Task.FromResult(GetProfile());
        }
    }
}