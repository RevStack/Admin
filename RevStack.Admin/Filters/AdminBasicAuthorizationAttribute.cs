using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;

namespace RevStack.Admin
{
    public class AdminBasicAuthorizationAttribute : AuthorizationFilterAttribute
    {
        
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;
            if(authHeader !=null)
            {
                if(authHeader.Scheme.Equals("basic",StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                    var split = credentials.Split(':');
                    string username = split[0];
                    string password = split[1];
                    if(username.ToLower()==Settings.UserName.ToLower() && password==Settings.Password)
                    {
                        return;
                    }
                }
            }

            reject(actionContext);
        }

        private void reject(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("Unauthorized Request"));
        }
    }
}