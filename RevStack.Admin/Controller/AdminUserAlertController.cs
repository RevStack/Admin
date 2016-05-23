using System;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using RevStack.Notification;

namespace RevStack.Admin
{
    public class AdminUserAlertController<TKey> : ApiController
    {
        private readonly INotifyTaskList<TKey> _taskList;
        public AdminUserAlertController(INotifyTaskList<TKey> taskList)
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
