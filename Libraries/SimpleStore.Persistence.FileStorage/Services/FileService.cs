using System.Text.Json;
using SimpleStore.Persistence.FileStorage.Interfaces;

namespace SimpleStore.Persistence.FileStorage.Services;

public class FileService : IFileService
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true }; // Improves readability

    public bool WriteToJsonFile<T>(string filePath, T objectToWrite)
    {
        try
        {
            var json = JsonSerializer.Serialize(objectToWrite, Options);
            File.WriteAllText(filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error -> Unexpected Error: {ex.Message}");
        }

        return false;
    }

    public T? ReadFromJsonFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return default;

        try
        {
            var json = File.ReadAllText(filePath);
            return string.IsNullOrWhiteSpace(json)
                ? default
                : JsonSerializer.Deserialize<T>(json);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error -> File Not Found: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Error -> Unauthorized Access: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error -> Unexpected Error: {ex.Message}");
        }

        return default;
    }
}