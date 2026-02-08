using MySqlConnector;
using BackgroundJobs.Models;

namespace BackgroundJobs.Services;

public interface IProcessorService
{
    Task<object>GetJobProcessorAsync(string type);
    Task <List<Player>>DumpPlayersAsync ();
}

public class ProcessorService : IProcessorService
{
    public readonly string connectionString;

    public ProcessorService(IConfiguration config)
    {
        connectionString = config.GetConnectionString("DefaultConnection")
        ?? throw new Exception("No Default Connection");
    }

    public async Task<Object>GetJobProcessorAsync(string type)
    {
        try
        {
            switch (type)
            {
                case "PlayerDump":
                    return DumpPlayersAsync();

                default:
                    return null;

            }
        }
        catch (Exception exception)
        {
            return null;
        }
    }

    public async Task<List<Player>>DumpPlayersAsync ()
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
        return result;   
    }

}