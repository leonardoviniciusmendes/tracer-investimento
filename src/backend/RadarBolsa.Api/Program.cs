using RadarBolsa.Api.Configuration;
using RadarBolsa.Api.Endpoints;
using RadarBolsa.Application;
using RadarBolsa.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFrontendCors();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseCors(CorsPolicies.Frontend);

app.MapHealthEndpoints();
app.MapOpportunityEndpoints();
app.MapManualSignalEndpoints();
app.MapTrackedAssetEndpoints();

app.Run();
