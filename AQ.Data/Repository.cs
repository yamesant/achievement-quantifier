using AQ.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AQ.Data;

public class Repository : IRepository
{
    private readonly ILogger<Repository> _logger;
    private readonly string _connectionString;

    public Repository(ILogger<Repository> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        string dataSource = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AQ",
            hostEnvironment.EnvironmentName,
            "data.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dataSource)!);
        _connectionString = $"Data Source={dataSource}";
        Setup();
    }

    private bool Setup()
    {
        try
        {
            using SqliteConnection connection = new(_connectionString);
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  BEGIN TRANSACTION;

                                  CREATE TABLE IF NOT EXISTS AchievementClass (
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Name TEXT NOT NULL UNIQUE,
                                      Unit TEXT NOT NULL
                                  );

                                  CREATE TABLE IF NOT EXISTS Achievement (
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      AchievementClassId INTEGER NOT NULL,
                                      CompletedDate TEXT NOT NULL,
                                      Quantity INTEGER NOT NULL,
                                      FOREIGN KEY (AchievementClassId) REFERENCES AchievementClass(Id)
                                  );

                                  COMMIT;
                                  """;
            connection.Open();
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
    }

    public async Task<AchievementClass?> Insert(AchievementClass achievementClass)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  INSERT INTO AchievementClass (Name, Unit) VALUES (@Name, @Unit);
                                  SELECT last_insert_rowid() AS Id;
                                  """;
            command.Parameters.AddWithValue("@Name", achievementClass.Name);
            command.Parameters.AddWithValue("@Unit", achievementClass.Unit);
            await connection.OpenAsync();
            object? output = await command.ExecuteScalarAsync();
            if (output is null) return null;
            achievementClass.Id = (long)output;
            return achievementClass;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
    
    public async Task<AchievementClass?> Update(AchievementClass achievementClass)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  UPDATE AchievementClass
                                  SET Name = @Name, Unit = @Unit
                                  WHERE Id = @Id;
                                  """;
            command.Parameters.AddWithValue("@Id", achievementClass.Id);
            command.Parameters.AddWithValue("@Name", achievementClass.Name);
            command.Parameters.AddWithValue("@Unit", achievementClass.Unit);
            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0 ? achievementClass : null;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
    
    public async Task<List<AchievementClass>> GetAllAchievementClasses()
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  SELECT Id, Name, Unit FROM AchievementClass;
                                  """;
            await connection.OpenAsync();
            SqliteDataReader reader = await command.ExecuteReaderAsync();
            List<AchievementClass> classes = new();
            while (await reader.ReadAsync())
            {
                classes.Add(new AchievementClass
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Unit = reader.GetString(reader.GetOrdinal("Unit"))
                });
            }
            return classes;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return [];
        }
    }

    public async Task<AchievementClass?> GetAchievementClassById(long id)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  SELECT Id, Name, Unit
                                  FROM AchievementClass
                                  WHERE Id = @Id;
                                  """;
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            SqliteDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AchievementClass
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Unit = reader.GetString(reader.GetOrdinal("Unit"))
                };
            }
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task<AchievementClass?> GetAchievementClassByName(string name)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  SELECT Id, Name, Unit
                                  FROM AchievementClass
                                  WHERE Name = @Name;
                                  """;
            command.Parameters.AddWithValue("@Name", name);
            await connection.OpenAsync();
            SqliteDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AchievementClass
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Unit = reader.GetString(reader.GetOrdinal("Unit"))
                };
            }
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
    
    public async Task<(int AchievementClassesDeleted, int AchievementsDeleted)> DeleteAchievementClass(long id)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  DELETE FROM Achievement WHERE AchievementClassId = @Id;
                                  SELECT changes();
                                  """;
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            int achievementsDeleted = await command.ExecuteNonQueryAsync();

            command.CommandText = """
                                  DELETE FROM AchievementClass WHERE Id = @Id;
                                  SELECT changes();
                                  """;
            int classesDeleted = await command.ExecuteNonQueryAsync();

            return (classesDeleted, achievementsDeleted);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return (0, 0);
        }
    }
}
