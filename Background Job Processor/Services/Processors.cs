using MySqlConnector;
using BackgroundJobs.Models;
using System.Text;

namespace BackgroundJobs.Services;

public interface IProcessorService
{
    Task<string>ConvertToCSV<T>(string id, string type, IEnumerable<T> data);
    Task<string>GetJobProcessorAsync(Job job, CancellationToken stopToken);
    Task <string>DumpPlayersAsync (string id);
}

public class ProcessorService : IProcessorService
{
    public readonly string connectionString;

    public ProcessorService(IConfiguration config)
    {
        connectionString = config.GetConnectionString("DefaultConnection")
        ?? throw new Exception("No Default Connection");
    }

    public async Task<string>ConvertToCSV<T>(string id, string type, IEnumerable<T> data)
    {
         var sb = new StringBuilder();

        sb.AppendLine(""); // Columns
 
        // Loop data
        sb.AppendLine("");
 
        var filename = $"{type}_{id}_{DateTime.UtcNow:ddMMYYYYHHmmss}.csv";
        var folder = Path.Combine(AppContext.BaseDirectory, "exports");
        Directory.CreateDirectory(folder);

        var location = Path.Combine(filename, folder);
        await File.WriteAllTextAsync(location, sb.ToString());

        return $"/exports/{location}";   
    }

    public async Task<string>GetJobProcessorAsync(Job job, CancellationToken stopToken)
    {
        string result;
        try
        {
            switch (job.Type)
            {
                case "PlayersDump":
                    return await DumpPlayersAsync(job.Id);

                default:
                    return null;

            }
        }
        catch (Exception exception)
        {
            return null;
        }
        return result;
    }

    public async Task<string>DumpPlayersAsync (string id)
    {
        var result = new List<Player>();
        using (var connection = new MySqlConnection(connectionString))
        {
          await connection.OpenAsync();
          var sql = "Select uid, name, playerid, cash, bankacc, cartelCredits, adminLevel, copLevel, ionLevel, medicLevel, last_seen, insert_time FROM players";
          using var command = new MySqlCommand(sql, connection);
          using var reader = await command.ExecuteReaderAsync();
          while (await reader.ReadAsync())
            {
              var row = new Player (
                reader["uid"].ToString() ?? string.Empty,
                reader["name"].ToString() ?? string.Empty,
                reader["playerid"].ToString() ?? string.Empty,
                reader["cash"].ToString() ?? string.Empty,
                reader["bankacc"].ToString() ?? string.Empty,
                reader["cartelCredits"].ToString() ?? string.Empty,
                reader["adminLevel"].ToString() ?? string.Empty,
                reader["copLevel"].ToString() ?? string.Empty,
                reader["ionLevel"].ToString() ?? string.Empty,
                reader["medicLevel"].ToString() ?? string.Empty,
                reader.GetDateTime(reader.GetOrdinal("last_seen")),
                reader.GetDateTime(reader.GetOrdinal("insert_time"))
              );  
              result.Add(row);
            };  
        };
        var file = await ConvertToCSV(id, "playersDump", result);
        return "result";   // Will return File location for CSV
    }

}