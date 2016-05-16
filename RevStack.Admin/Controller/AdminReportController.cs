using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace RevStack.Admin
{
    public class AdminReportController : ApiController
    {
        private readonly IAdminReportService _service;
        public AdminReportController(IAdminReportService service)
        {
            _service = service;
        }

        public virtual async Task<IHttpActionResult> Get()
        {
            var entity = await _service.GetAsync();
            return Ok(entity);
        }
    }
}
