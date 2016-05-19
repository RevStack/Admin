using System;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Pattern;
using RevStack.Identity;

namespace RevStack.Admin
{
    public class AdminUserReportService<TUser,TKey> : IAdminUserReportService
        where TUser : class, IIdentityUser<TKey>
    {
        protected IRepository<TUser, TKey> _userRepository;
        public AdminUserReportService(IRepository<TUser, TKey> userRepository)
        {
           
            _userRepository = userRepository;
        }

        public virtual AdminReportModel Get()
        {
            var report = new AdminReportModel
            {
                Visits = "N/A",
                Orders = "N/A",
                Users = _userRepository.Get().Count().ToString(),
                Sales = "N/A"
            };

            return report;
        }

        public virtual Task<AdminReportModel> GetAsync()
        {
            return Task.FromResult(Get());
        }
    }
}