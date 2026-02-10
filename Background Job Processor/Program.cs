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
builder.Services.AddHostedService<JobWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{}

app.UseHttpsRedirection();
app.MapJobEndpoints();
app.MapPlayerEndpoints();

app.Run();
