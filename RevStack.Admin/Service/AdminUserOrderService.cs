using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStack.Commerce;
using RevStack.Pattern;
using RevStack.Mvc;

namespace RevStack.Admin
{
    public class AdminUserOrderService<TOrder,TPayment,TKey> : IAdminUserOrderService<TOrder,TPayment,TKey>
        where TOrder : class, IOrder<TPayment, TKey>
        where TPayment : class, IPayment
    {
        private readonly IRepository<TOrder, TKey> _repository;
        public AdminUserOrderService(IRepository<TOrder, TKey> repository)
        {
            _repository = repository;
        }
        public IEnumerable<TOrder> Get(TKey id)
        {
            return _repository.Find(x => x.Compare(x.UserId,id));
        }

        public Task<IEnumerable<TOrder>> GetAsync(TKey id)
        {
            return Task.FromResult(Get(id));
        }
    }
}