namespace GenAccERP.Services
{
    public enum ServiceStatus { Ok, NotFound, BadRequest, Conflict }

    public class ServiceResult<T>
    {
        public ServiceStatus Status { get; private init; }
        public T? Value { get; private init; }
        public string? Error { get; private init; }
        public bool IsSuccess => Status == ServiceStatus.Ok;

        public static ServiceResult<T> Success(T value) => new() { Status = ServiceStatus.Ok, Value = value };
        public static ServiceResult<T> NotFound() => new() { Status = ServiceStatus.NotFound };
        public static ServiceResult<T> BadRequest(string error) => new() { Status = ServiceStatus.BadRequest, Error = error };
        public static ServiceResult<T> Conflict(string error) => new() { Status = ServiceStatus.Conflict, Error = error };
    }
}
