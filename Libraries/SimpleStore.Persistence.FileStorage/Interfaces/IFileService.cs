namespace SimpleStore.Persistence.FileStorage.Interfaces;

public interface IFileService
{
    bool WriteToJsonFile<T>(string filePath, T objectToWrite);
    T? ReadFromJsonFile<T>(string filePath);
}