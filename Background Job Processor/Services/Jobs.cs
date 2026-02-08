using MySqlConnector;
using BackgroundJobs.Models;

namespace BackgroundJobs.Services;

public interface IJobService
{
  Task<List<Job>>GetJobsAsync();
  Task<List<Job>>GetJobAsync(string id);  
  Task<object>CreateJobAsync(string type);
  Task <List<Job>>GetWaitingJobAsync(CancellationToken stopToken);
  Task<String>UpdateJobStatusAsync(int id, string status, string result);

};

public class JobService : IJobService
{
    public readonly string connectionString;

    public JobService(IConfiguration config)
    {
        connectionString = config.GetConnectionString("DefaultConnection")
        ?? throw new Exception("No Default Connection");
    }

    public async Task<List<Job>>GetJobsAsync() // Add Param for Failed?
    {
        var result = new List<Job>();
        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            var sql = @"Select * FROM jobs WHERE status !='complete'";
            using var command = new MySqlCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Job (
                    reader["id"].ToString() ?? string.Empty,
                    reader["type"].ToString() ?? string.Empty,
                    reader["status"].ToString() ?? string.Empty,
                    reader["result"].ToString() ?? string.Empty,
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                result.Add(row);
            };
        }
        return result;
    }

    public async Task<List<Job>>GetJobAsync(string id)
    {
        var result = new List<Job>();
        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            var sql = @"Select * FROM jobs where id = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);  
            var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }
            var row = new Job (
                reader["id"].ToString() ?? string.Empty,
                reader["type"].ToString() ?? string.Empty,
                reader["status"].ToString() ?? string.Empty,
                reader["result"].ToString() ?? string.Empty,
                reader.GetDateTime(reader.GetOrdinal("created_at")),
                reader.GetDateTime(reader.GetOrdinal("updated_at"))
            );
            result.Add(row); 
        };
        return result;
    }

    public async Task<object>CreateJobAsync(string type)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            var sql = "INSERT INTO jobs (type, status, payload, result) VALUES (@type, 'Incomplete', @payload, NULL); SELECT LAST_INSERT_ID();";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@payload", null);
            var id = Convert.ToInt32(command.ExecuteScalar());
            return id;
        };
    }

    public async Task<List<Job>>GetWaitingJobAsync(CancellationToken stopToken)
    {
        var result = new List<Job>();
        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            var sql = "SELECT * FROM jobs WHERE status = 'incomplete' Limit 1 ORDERBY ASC";
            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var row = new Job (
                reader["id"].ToString() ?? string.Empty,
                reader["type"].ToString() ?? string.Empty,
                reader["status"].ToString() ?? string.Empty,
                reader["result"].ToString() ?? string.Empty,
                reader.GetDateTime(reader.GetOrdinal("created_at")),
                reader.GetDateTime(reader.GetOrdinal("updated_at"))
            );
            result.Add(row);
        };
        return result;
    }

    public async Task<string>UpdateJobStatusAsync(int id, string status, string result)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            var sql = @"UPDATE jpbs SET status = @status, result = @result Where id = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@result", result);
            int reader = await command.ExecuteNonQueryAsync();
            if (reader == 0)
            {
                return "Failed";
            }
            return "Seuccess";
        }
    }
};