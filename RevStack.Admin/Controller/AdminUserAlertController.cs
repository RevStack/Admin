using System;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Commerce;
using RevStack.Notification;

namespace RevStack.Admin
{
    public class AdminUserAlertController<TOrder,TPayment,TKey> : ApiController
        where TOrder : class, IOrder<TPayment,TKey>
        where TPayment : class, IPayment
    {
        private readonly INotifyTaskList<TKey> _taskList;
        public AdminUserAlertController(IAdminOrderService<TOrder,TPayment,TKey> service, INotifyTaskList<TKey> taskList)
        {
            _taskList = taskList;
        }

        public async Task<IHttpActionResult> post(NotifyAlert<TKey> alert)
        {
            var userAlertTasks = _taskList.Tasks.Where(x => x.TaskType == NotifyTaskType.UserAlert);
            if(userAlertTasks.Any())
            {
                foreach(var task in userAlertTasks)
                {
                    await task.RunAsync(alert);
                }
            }
            return Ok(alert);
        }
    }
}
