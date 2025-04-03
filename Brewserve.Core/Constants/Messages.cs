namespace BrewServe.Core.Constants
{
    public static class Messages
    {
        public const string InValidInput = "Invalid input data";
        public const string InternalServerError = "An Internal server error occurred. Please try again";
        public static string RecordAlreadyExists(string recordName) => $"Record {recordName} already exists";
        public static string RecordNotFound(string recordName) => $"{recordName} not found";
        public static string RecordUpdated(string recordName, int id) => $"{recordName}Id {id} updated successfully";
    }
}
