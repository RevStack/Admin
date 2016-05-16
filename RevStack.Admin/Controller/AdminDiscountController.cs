using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Extensions;
using System.Threading.Tasks;
using RevStack.Mvc;
using RevStack.Pattern;
using RevStack.Commerce;

namespace RevStack.Admin
{
    public class AdminDiscountController : ApiController
    {
        private readonly IService<Discount, string> _service;
        public AdminDiscountController(IService<Discount, string> service)
        {
            _service = service;
        }

        public virtual async Task<IHttpActionResult> Get(ODataQueryOptions<Discount> options)
        {
            ODataQuerySettings settings = new ODataQuerySettings() { };
            var result = await _service.GetAsync();
            IQueryable query = options.ApplyTo(result.AsQueryable(), settings);
            var pagedResult = new PageResult<Discount>(query as IQueryable<Discount>, Request.ODataProperties().NextLink, Request.ODataProperties().TotalCount);
            return Content(HttpStatusCode.OK, pagedResult);
        }

        public virtual async Task<IHttpActionResult> Get(string id)
        {
            var query = await _service.FindAsync(x => x.Code.ToLower() == id.ToLower());
            var entity = query.FirstOrDefault();
            entity.Percentage = entity.Percentage.ToPercentageValue();
            return Ok(entity);
        }

        public virtual async Task<IHttpActionResult> Post(Discount entity)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new ModelErrorResult(Request, modelErrors);
            }
            var query = await _service.FindAsync(x => x.Code.ToLower() == entity.Code.ToLower());
            if(query.Any())
            {
                return new ContentErrorResult(Request,HttpStatusCode.BadRequest, "The promotion code already exists");
            }
            entity.Percentage = entity.Percentage.ToRatioValue();
            await _service.AddAsync(entity);
            return Ok(entity);
        }

        public virtual async Task<IHttpActionResult> Put(Discount entity)
        {
            await _service.UpdateAsync(entity);
            return Ok(entity);
        }

        public virtual async Task<IHttpActionResult> Delete(string id)
        {
            var query = await _service.FindAsync(x => x.Code.ToLower() == id.ToLower());
            var entity = query.FirstOrDefault();
            await _service.DeleteAsync(entity);
            return Ok(true);
        }

    }
}
