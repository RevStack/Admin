using System;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Pattern;
using RevStack.Commerce;
using RevStack.Identity;

namespace RevStack.Admin
{
    public class AdminReportService<TOrder,TPayment,TUser,TKey> : IAdminReportService
        where TOrder : class, IOrder<TPayment,TKey>
        where TPayment : class, IPayment
        where TUser : class, IIdentityUser<TKey>
    {
        private readonly IRepository<TOrder, TKey> _orderRepository;
        protected IRepository<TUser, TKey> _userRepository;
        public AdminReportService(IRepository<TOrder, TKey> orderRepository, IRepository<TUser, TKey> userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public virtual AdminReportModel Get()
        {
            var report = new AdminReportModel
            {
                Visits = "N/A",
                Orders = _orderRepository.Get().Count().ToString(),
                Users = _userRepository.Get().Count().ToString(),
                Sales = _orderRepository.Get().Sum(x => x.Total).ToString("C")
            };

            return report;
        }

        public virtual Task<AdminReportModel> GetAsync()
        {
            return Task.FromResult(Get());
        }
    }
}