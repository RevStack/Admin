using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using RevStack.Mvc;

namespace RevStack.Admin
{
    
    public class AdminLoginController : ApiController
    {
        protected readonly IAdminLoginService _loginService;
        public AdminLoginController(IAdminLoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<IHttpActionResult> post(AdminLoginModel model)
        {
            if(!_loginService.Login(model))
            {
                return new ContentErrorResult(Request, HttpStatusCode.Forbidden, "Invalid Login Attempt");
            }
            else
            {
                var profile = await _loginService.GetProfileAsync();
                return Ok(profile);
            }
        }
    }
}
