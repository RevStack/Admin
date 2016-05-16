using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Extensions;
using System.Threading.Tasks;
using RevStack.Mvc;
using RevStack.Commerce;

namespace RevStack.Admin
{

    public class AdminOrderController<TOrder,TPayment,TKey> : ApiController
        where TOrder : class, IOrder<TPayment, TKey>
        where TPayment : class, IPayment
    {
        private IAdminOrderService<TOrder,TPayment,TKey> _service;
        public AdminOrderController(IAdminOrderService<TOrder, TPayment, TKey> service)
        {
            _service = service;
        }

        public virtual async Task<IHttpActionResult> Get(ODataQueryOptions<TOrder> options)
        {
            ODataQuerySettings settings = new ODataQuerySettings() { };
            var result = await _service.GetAsync();
            IQueryable query = options.ApplyTo(result.AsQueryable(), settings);
            var pagedResult = new PageResult<TOrder>(query as IQueryable<TOrder>, Request.ODataProperties().NextLink, Request.ODataProperties().TotalCount);
            return Content(HttpStatusCode.OK, pagedResult);
        }

        public virtual async Task<IHttpActionResult> Get(TKey id)
        {
            var entity = await _service.GetAsync(id);
            return Ok(entity);
        }

        public virtual async Task<IHttpActionResult> Delete(TKey id)
        {
            if (!Settings.AllowOrderDelete)
            {
                return new ContentErrorResult(Request, HttpStatusCode.Forbidden, "Delete order not allowed");
            }
            await _service.DeleteAsync(id);
            return Ok(true);
        }
    }
}
