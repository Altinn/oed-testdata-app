using Microsoft.AspNetCore.Authorization;

namespace oed_testdata.Server.Infrastructure.Auth;

public class QueryParamRequirement(
    string queryParamName,
    string secret)
    : IAuthorizationRequirement
{
    public string Secret { get; } = secret;

    public string QueryParamName { get; } = queryParamName;
}