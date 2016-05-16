using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using RevStack.Mvc;
using RevStack.Commerce;
using RevStack.Pattern;

namespace RevStack.Admin
{
    public class AdminDiscountValidateController : ApiController
    {
        private readonly IService<Discount, string> _service;
        public AdminDiscountValidateController(IService<Discount, string> service)
        {
            _service = service;
        }

        public virtual async Task<IHttpActionResult> Get(string code)
        {
            var query = await _service.FindAsync(x => x.Code.ToLower() == code.ToLower());
            if (query.Any())
            {
                return new ContentErrorResult(Request, HttpStatusCode.BadRequest, "The promotion code already exists");
            }

            return Ok(true);
        }
    }
}
