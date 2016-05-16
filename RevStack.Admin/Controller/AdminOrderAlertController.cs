using System;
using System.Net;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Commerce;
using RevStack.Mvc;
using RevStack.Notification;

namespace RevStack.Admin
{
    public class AdminOrderAlertController<TOrder,TPayment,TKey> : ApiController
        where TOrder : class, IOrder<TPayment, TKey>
        where TPayment : class, IPayment
    {
        private readonly INotifyTaskList<TKey> _taskList;
        private IAdminOrderService<TOrder,TPayment,TKey> _service;
        public AdminOrderAlertController(IAdminOrderService<TOrder, TPayment, TKey> service, INotifyTaskList<TKey> taskList)
        {
            _service = service;
            _taskList = taskList;
        }

        public async Task<IHttpActionResult> post(NotifyAlert<TKey> alert)
        {
            var id = alert.Id;
            var order = await _service.GetAsync(id);
            if(order==null)
            {
                return new ContentErrorResult(Request, HttpStatusCode.NotFound, "Error: No Order found for the notification request");
            }
            alert.PhoneNumber = order.BillingAddress.PhoneNumber;
            alert.Email = order.Email;
            alert.TrackingUrl = order.TrackingUrl;

            var orderAlertTasks= _taskList.Tasks.Where(x => x.TaskType == NotifyTaskType.OrderAlert);

            //run tasks
            orderAlertTasks.ToList().ForEach(x => x.RunAsync(alert));

            //save notification to order
            await _service.AddNotificationAsync(id, new OrderNotification
            {
                Key=alert.Key,
                Value=alert.Value,
                Date=DateTime.Now
            });

            return Ok(alert);
        }
    }
}
