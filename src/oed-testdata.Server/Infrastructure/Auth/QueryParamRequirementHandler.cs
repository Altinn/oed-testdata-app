using Microsoft.AspNetCore.Authorization;

namespace oed_testdata.Server.Infrastructure.Auth;

public class QueryParamRequirementHandler(
    IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<QueryParamRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        QueryParamRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is not null && httpContext.Request.Query.TryGetValue(requirement.QueryParamName, out var queryValue))
        {
            if (queryValue == requirement.Secret)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}