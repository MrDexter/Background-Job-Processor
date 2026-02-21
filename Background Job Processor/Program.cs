using Microsoft.Extensions.Hosting;
using BackgroundJobs.Endpoints;
using BackgroundJobs.Services;
using BackgroundJobs.Background;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IProcessorService, ProcessorService>();
// For the Azure App Service test platform, Disable this and do a manual trigger on job creation to save on resources
// builder.Services.AddHostedService<JobWorker>();
builder.Services.AddScoped<IJobWorker, JobWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{}

app.UseHttpsRedirection();
app.MapJobEndpoints();
app.MapPlayerEndpoints();
app.MapMiscEndpoints();

app.MapGet("/", () => Results.Ok(new { 
    Service = "DecsPage Backend Worker", 
    Status = "Online", 
    Version = "1.0.0",
    // Documentation = "/swagger" // Soon / Next...
}));

app.Run();