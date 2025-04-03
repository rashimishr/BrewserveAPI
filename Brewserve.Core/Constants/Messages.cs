namespace BrewServe.Core.Constants;
public static class Messages
{
    public static string RecordAlreadyExists(string recordName)
    {
        return $"Record {recordName} already exists";
    }
    public static string RecordNotFound(string recordName)
    {
        return $"{recordName} not found";
    }
    public static string RecordUpdated(string recordName, int id)
    {
        return $"{recordName}Id {id} updated successfully";
    }
}