using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CompProgEdu.Core.Features.Users;
using IdentityServer4.Extensions;

namespace CompProgEdu.Features
{
    public class AuthenticateApiAttribute : TypeFilterAttribute
    {
        public AuthenticateApiAttribute() : base(typeof(AuthenticationFilter))
        {
        }
    }

    public class AuthenticationFilter : IAsyncAuthorizationFilter
    {

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.IsAuthenticated())
                return;

            await Authenticate(context);
            user = context.HttpContext.User;

            if (user.IsAuthenticated())
                return;

            context.Result = new UnauthorizedResult();
        }

        private async Task Authenticate(ActionContext context)
        {
            var jwtAuth = await context.HttpContext
                .AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (jwtAuth.Succeeded)
            {
                var jwtPrincipal = jwtAuth.Principal;
                var userManager = context.HttpContext.RequestServices.GetService<UserManager<User>>();
                var userEntity = await userManager.GetUserAsync(jwtPrincipal);
                if (userEntity == null)
                {
                    context.HttpContext.User = jwtPrincipal;
                }
                else
                {
                    var userClaimsPrincipalFactory =
                        context.HttpContext.RequestServices.GetService<IUserClaimsPrincipalFactory<User>>();
                    context.HttpContext.User = await userClaimsPrincipalFactory.CreateAsync(userEntity);
                }
            }
        }
    }
}
