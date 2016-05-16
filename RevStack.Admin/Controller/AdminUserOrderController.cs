using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Extensions;
using System.Threading.Tasks;
using RevStack.Commerce;

namespace RevStack.Admin
{
    public class AdminUserOrderController<TOrder,TPayment,TKey> : ApiController
        where TOrder : class, IOrder<TPayment,TKey>
        where TPayment : class, IPayment
    {
        protected IAdminUserOrderService<TOrder,TPayment,TKey> _service;
        public AdminUserOrderController(IAdminUserOrderService<TOrder,TPayment,TKey> service)
        {
            _service = service;
        }

        public virtual async Task<IHttpActionResult> Get(ODataQueryOptions<TOrder> options,TKey id)
        {
            ODataQuerySettings settings = new ODataQuerySettings() { };
            var result = await _service.GetAsync(id);
            IQueryable query = options.ApplyTo(result.AsQueryable(), settings);
            var pagedResult = new PageResult<TOrder>(query as IQueryable<TOrder>, Request.ODataProperties().NextLink, Request.ODataProperties().TotalCount);
            return Content(HttpStatusCode.OK, pagedResult);
        }

    }
}
