using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace CompProgEdu.Features.Auth
{
    public class RoleAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string[] _roleList { get; set; }
        private List<string> allRoles = CompProgEdu.Core.Security.Roles.List;
        public RoleAuthorizationAttribute(params string[] roleList)
        {
            Roles = string.Join(",", allRoles);
            _roleList = roleList;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            //Validate if any permissions are passed when using attribute at controller or action level
            if (!_roleList.Any())
            {
                //Validation cannot take place without any permissions so returning unauthorized
                filterContext.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = $"Status Code: {401} - Unauthorized",
                    ContentType = "text/plain",
                };
                return;
            }

            var rolesFromToken = filterContext.HttpContext.User.Claims.Select(x => x.Value).Where(x => allRoles.Contains(x));

            foreach (var role in _roleList)
            {
                if (rolesFromToken.Contains(role) || rolesFromToken.Contains("Global Admin"))
                {
                    return;
                }
            }

            filterContext.Result = new ContentResult
            {
                StatusCode = 401,
                Content = $"Status Code: {401} - Unauthorized",
                ContentType = "text/plain",
            };

            return;
        }
    }
}
