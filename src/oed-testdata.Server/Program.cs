using Microsoft.AspNetCore.Mvc;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Oed;
using oed_testdata.Server.Services;
using oed_testdata.Server.Testdata.Estate;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddAltinnClient(builder.Configuration);
builder.Services.AddTestdataStore();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapEstateEndpoints();
app.MapOedInstanceEndpoints();

app.UseHttpsRedirection();


app.MapGet("/test", async ([FromServices] ITestService testService) =>
{
    var instanceData = await testService.Test();
    return TypedResults.Ok(instanceData);
}).WithName("Test");

app.MapFallbackToFile("/index.html");

app.Run();