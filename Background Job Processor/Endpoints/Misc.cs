
namespace BackgroundJobs.Endpoints;

public static class MiscEndpoints
{
    public static IEndpointRouteBuilder MapMiscEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", async () =>
        {
            Results.Ok("Ok");   
        });

        app.MapGet("/debug/config", (IConfiguration cfg) =>
        {
            var hasSql = !string.IsNullOrWhiteSpace(cfg.GetConnectionString("DefaultConnection"));
            var hasBlob = !string.IsNullOrWhiteSpace(cfg["Storage:ConnectionString"]);
            var container = cfg["Storage:Container"] ?? "(null)";

            return Results.Ok(new { hasSql, hasBlob, container });
        });


        return app;
    }
};