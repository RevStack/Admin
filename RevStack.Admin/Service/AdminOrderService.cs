using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Commerce;
using RevStack.Pattern;
using RevStack.Mvc;

namespace RevStack.Admin
{
    public class AdminOrderService<TOrder,TPayment,TKey> : IAdminOrderService<TOrder,TPayment,TKey>
         where TOrder : class, IOrder<TPayment, TKey>
        where TPayment : class, IPayment
    {
        private readonly IRepository<TOrder,TKey> _repository; 
        public AdminOrderService(IRepository<TOrder,TKey> repository)
        {
            _repository = repository;
        }

        public IEnumerable<TOrder> Get()
        {
            return _repository.Get();
        }

        public Task<IEnumerable<TOrder>> GetAsync()
        {
            return Task.FromResult(Get());
        }

        public TOrder Get(TKey id)
        {
            return _repository.Find(x => x.Compare(x.Id,id)).FirstOrDefault();
        }

        public Task<TOrder> GetAsync(TKey id)
        {
            return Task.FromResult(Get(id));
        }

        public bool Delete(TKey id)
        {
            var results = _repository.Find(x => x.Compare(x.Id,id));
            if (results.Any())
            {
                var entity = results.FirstOrDefault();
                _repository.Delete(entity);
            }
            return true;
        }

        public Task<bool> DeleteAsync(TKey id)
        {
            return Task.FromResult(Delete(id));
        }

        public bool UpdateStatus(TKey id, CurrentOrderStatus status, OrderNotification notification)
        {
            var order=_repository.Find(x => x.Compare(x.Id,id)).FirstOrDefault();
            if (order == null)
            {
                return false;
            }
            order.OrderStatus.Status = status;
            order.OrderStatus.Notifications.Add(notification);
            _repository.Update(order);

            return true;
        }

        public Task<bool> UpdateStatusAsync(TKey id, CurrentOrderStatus status, OrderNotification notification)
        {
            return Task.FromResult(UpdateStatus(id, status, notification));
        }

        public bool AddNotification(TKey id, OrderNotification notification)
        {
            var order = _repository.Find(x => x.Compare(x.Id,id)).FirstOrDefault();
            if (order == null)
            {
                return false;
            }
            order.OrderStatus.Notifications.Add(notification);
            _repository.Update(order);

            return true;
        }

        public Task<bool> AddNotificationAsync(TKey id, OrderNotification notification)
        {
            return Task.FromResult(AddNotification(id, notification));
        }
    }


}