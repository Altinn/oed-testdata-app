using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using oed_testdata.Server.CloudEvents;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.Auth;
using oed_testdata.Server.Infrastructure.Maskinporten;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Oed;
using oed_testdata.Server.Services;
using oed_testdata.Server.Testdata.Bank;
using oed_testdata.Server.Testdata.Estate;
using oed_testdata.Server.Testdata.Svv;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
        options.AddPolicy("Dev", configurePolicy =>
        {
            // Allow multiple methods
            configurePolicy.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")
                .WithHeaders(
                    HeaderNames.Accept,
                    HeaderNames.ContentType,
                    HeaderNames.Authorization)
                .AllowCredentials()
                .SetIsOriginAllowed(origin =>
                {
                    if (string.IsNullOrWhiteSpace(origin)) return false;
                    if (origin.ToLower().StartsWith("https://localhost")) return true;
                    return false;
                });
        })
    );
}

builder.Services.AddBasicAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddBasicAuthorization(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddAltinnClient(builder.Configuration);
builder.Services.AddMaskinportenClient(builder.Configuration);
builder.Services.AddOedClient(builder.Configuration);
builder.Services.AddTestdataStore();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Dev");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapBasicAuthenticationEndpoints();
app.MapEstateEndpoints();
app.MapBankEndpoints();
app.MapSvvEndpoints();
app.MapOedInstanceEndpoints();
app.MapCloudEventEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/test", async ([FromServices] ITestService testService) =>
    {
        var instanceData = await testService.Test();
        return TypedResults.Ok(instanceData);
    }).WithName("Test").RequireAuthorization();
}


app.UseHttpsRedirection();

app.MapFallbackToFile("/index.html");

app.Run();