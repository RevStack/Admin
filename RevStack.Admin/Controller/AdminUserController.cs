﻿using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Extensions;
using System.Threading.Tasks;
using RevStack.Identity.Mvc;
using RevStack.Mvc;
using RevStack.Notification;

namespace RevStack.Admin
{

    public class AdminUserController<TUserModel, TProfile, TCreateProfile, TKey> : ApiController
        where TUserModel : class, IAdminUserModel<TKey>
        where TProfile : class, IProfileModel<TKey>
        where TCreateProfile : class, ICreateProfileModel<TKey>
    {
        protected IAdminUserService<TUserModel,TProfile,TCreateProfile,TKey> _service;
        protected INotifyTaskList<TKey> _notifyTaskList;
        protected Func<NotifyAlert<TKey>> _notifyAlertFactory;
        public AdminUserController(IAdminUserService<TUserModel,TProfile,TCreateProfile,TKey> service, INotifyTaskList<TKey> notifyTaskList,Func<NotifyAlert<TKey>> notifyAlertFactory)
        {
            _service = service;
            _notifyTaskList = notifyTaskList;
            _notifyAlertFactory = notifyAlertFactory;
        }

        public virtual async Task<IHttpActionResult> Get(ODataQueryOptions<TUserModel> options)
        {
            ODataQuerySettings settings = new ODataQuerySettings() { };
            var result = await _service.GetAsync();
            IQueryable query = options.ApplyTo(result.AsQueryable(), settings);
            var pagedResult = new PageResult<TUserModel>(query as IQueryable<TUserModel>, Request.ODataProperties().NextLink, Request.ODataProperties().TotalCount);
            return Content(HttpStatusCode.OK, pagedResult);
        }

        public virtual async Task<IHttpActionResult> Get(TKey id)
        {
            var entity = await _service.GetAsync(id);
            return Ok(entity);
        }

        public virtual async Task<IHttpActionResult> Post(TCreateProfile entity)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new ModelErrorResult(Request, modelErrors);
            }
            var isValid = await _service.ValidateAsync(entity);
            if(!isValid)
            {
                return new ContentErrorResult(Request, HttpStatusCode.Forbidden, "User email already exists");
            }
            var result = await _service.AddAsync(entity);
            if (!result.Item2)
            {
                return new ContentErrorResult(Request, HttpStatusCode.BadRequest, result.Item3);
            }
            entity.Id = result.Item1.Id;
            NotifyUser(entity);

            return Ok(result);
        }

        public virtual async Task<IHttpActionResult> Put(TProfile entity)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new ModelErrorResult(Request, modelErrors);
            }
            var result = await _service.UpdateAsync(entity);
            return Ok(result);
        }

        public virtual async Task<IHttpActionResult> Delete(TKey id)
        {
            if(!Settings.AllowUserDelete)
            {
                return new ContentErrorResult(Request, HttpStatusCode.Forbidden, "Delete user not allowed");
            }
            await _service.DeleteAsync(id);
            return Ok(true);
        }

        #region "Protected"
        protected virtual void NotifyUser(TCreateProfile entity)
        {
            string newLine = Environment.NewLine;
            string value = Settings.CompanyName + " user registration notice. " + newLine + newLine + "Username: " + entity.Email + " " + newLine + "Password: " + entity.Password; 

            var alert = _notifyAlertFactory();
            alert.Id = entity.Id;
            alert.Date = DateTime.Now;
            alert.Email = entity.Email;
            alert.IsAuthenticated = true;
            alert.Name = entity.Email;
            alert.PhoneNumber = entity.PhoneNumber;
            alert.Key = "User Registration";
            alert.Value = value;

            var tasks = _notifyTaskList.Tasks.Where(x => x.TaskType == NotifyTaskType.UserSignUp);
            tasks.ToList().ForEach(x => x.RunAsync(alert));
        }
        #endregion
    }
}
