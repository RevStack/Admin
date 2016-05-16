using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStack.Commerce;


namespace RevStack.Admin
{
    public interface IAdminUserOrderService<TOrder,TPayment,TKey>
        where TOrder : class, IOrder<TPayment,TKey>
        where TPayment : class, IPayment
    {
        IEnumerable<TOrder> Get(TKey id);
        Task<IEnumerable<TOrder>> GetAsync(TKey id);
    }
}