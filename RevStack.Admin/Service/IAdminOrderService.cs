using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStack.Commerce;

namespace RevStack.Admin
{
    public interface IAdminOrderService<TOrder,TPayment,TKey>
        where TOrder : class, IOrder<TPayment, TKey>
        where TPayment : class, IPayment
    {
        IEnumerable<TOrder> Get();
        Task<IEnumerable<TOrder>> GetAsync();
        TOrder Get(TKey id);
        Task<TOrder> GetAsync(TKey id);
        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id);
        bool UpdateStatus(TKey id, CurrentOrderStatus status, OrderNotification notification);
        Task<bool> UpdateStatusAsync(TKey id, CurrentOrderStatus status,OrderNotification notification);
        bool AddNotification(TKey id, OrderNotification notification);
        Task<bool> AddNotificationAsync(TKey id, OrderNotification notification);
    }
}