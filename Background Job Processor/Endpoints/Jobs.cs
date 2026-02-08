using BackgroundJobs.Models;
using BackgroundJobs.Services;
// using Microsoft.AspNetCore.Authorization;

namespace BackgroundJobs.Endpoints;

public static class JobEndpoints
{
    public static IEndpointRouteBuilder MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/jobs");

        group.MapGet("/", async (IJobService jobs) =>
        {
            var result = await jobs.GetJobsAsync();
            return Results.Ok(result);
        });

        group.MapGet("/{id}", async (string id, IJobService jobs) =>
        {
            var result = await jobs.GetJobAsync(id);
            if (result is null)
            {return Results.NotFound();};
            return Results.Ok(result);
        });
        return app;
    }
}