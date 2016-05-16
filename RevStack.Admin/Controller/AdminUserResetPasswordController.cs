using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using RevStack.Identity.Mvc;
using RevStack.Identity;
using RevStack.Mvc;
using RevStack.Notification;

namespace RevStack.Admin
{
    public class AdminUserResetPasswordController<TUser,TKey> : ApiController
        where TUser : class, IIdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        protected Func<ApplicationUserManager<TUser, TKey>> _userManagerFactory;
        protected INotifyTaskList<TKey> _notifyTaskList;
        protected Func<NotifyAlert<TKey>> _notifyAlertFactory;
        public AdminUserResetPasswordController(Func<ApplicationUserManager<TUser, TKey>> userManagerFactory, INotifyTaskList<TKey> notifyTaskList,Func<NotifyAlert<TKey>> notifyAlertFactory)
        {
            _userManagerFactory = userManagerFactory;
            _notifyTaskList = notifyTaskList;
            _notifyAlertFactory = notifyAlertFactory;
        }

        public virtual async Task<IHttpActionResult> Post(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new ModelErrorResult(Request, modelErrors);
            }

            var userManager = _userManagerFactory();
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user==null)
            {
                return new ContentErrorResult(Request, HttpStatusCode.NotFound, "Error: No user associated with the posted email address");
            }
            //generate the code token
            var code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            //use the generated code token
            var result = await userManager.ResetPasswordAsync(user.Id, code, model.Password);
            if(!result.Succeeded)
            {
                return new ContentErrorResult(Request, HttpStatusCode.BadRequest, result.Errors.FirstOrDefault());
            }

            NotifyUser(model,user.PhoneNumber);

            return Ok(model);
        }

        protected virtual void NotifyUser(ResetPasswordModel model,string phoneNumber)
        {
            string newLine = Environment.NewLine;
            string value = Settings.CompanyName + " user password reset notice. " + newLine + newLine + 
                "Your password has been reset by a website administrator. " + newLine + "Your new password: " + model.Password;

            var alert = _notifyAlertFactory();
            alert.Email = model.Email;
            alert.IsAuthenticated = true;
            alert.Name = model.Email;
            alert.PhoneNumber = phoneNumber;
            alert.Key = "User Password Reset";
            alert.Value = value;

            var tasks = _notifyTaskList.Tasks.Where(x => x.TaskType == NotifyTaskType.UserAlert);
            tasks.ToList().ForEach(x => x.RunAsync(alert));
        }

       
    }
}