using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using RevStack.Notification;
using RevStack.Mvc;

namespace RevStack.Admin
{
    public class AdminUserAlertMvcController<TKey> : Controller
    {
        private readonly IAdminAlertMessageService<TKey> _alertMessageService;
        public AdminUserAlertMvcController(IAdminAlertMessageService<TKey> alertMessageService)
        {
            _alertMessageService = alertMessageService;
        }

        public virtual async Task<ActionResult> Notification(TKey id, string key, string value, bool authenticated, string name)
        {
            var result = await NotificationActionAsync(id, key, value, authenticated, name);
            return result;
        }

        [NonAction]
        protected ActionResult NotificationAction(TKey id, string key, string value, bool authenticated, string name)
        {
            var entity = new NotifyAlert<TKey>
            {
                Id = id,
                Key = key,
                Value = value,
                Date = DateTime.Now,
                IsAuthenticated = authenticated,
                Name = name
            };

            var uri = new UriUtility(Request);
            var message = _alertMessageService.Get(entity, uri);
            return View(message);
        }

        [NonAction]
        protected Task<ActionResult> NotificationActionAsync(TKey id, string key, string value, bool authenticated, string name)
        {
            return Task.FromResult(NotificationAction(id, key, value, authenticated, name));
        }
    }
}
