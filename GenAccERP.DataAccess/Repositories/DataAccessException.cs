namespace GenAccERP.DataAccess.Repositories
{
    /// <summary>Raised when the database rejects a write (constraint, trigger, etc.).</summary>
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception? inner = null) : base(message, inner) { }
    }
}
