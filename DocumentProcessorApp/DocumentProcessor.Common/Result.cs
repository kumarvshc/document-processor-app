namespace DocumentProcessor.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public string Error { get; }
        public int StatusCode { get; }

        private Result(bool isSuccess, T? value, string error, int statusCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, int statusCode = 200)
        {
            return new Result<T>(true, value, string.Empty, statusCode);
        }

        public static Result<T> Failure(string error, int statusCode = 400)
        {
            return new Result<T>(false, default, error, statusCode);
        }

        public static Result<T> NotFound(string error = "Resource not found")
        {
            return new Result<T>(false, default, error, 404);
        }

        public static Result<T> UnprocessableEntity(string error)
        {
            return new Result<T>(false, default, error, 422);
        }

        public static Result<T> InternalError(string error = "An internal error occurred")
        {
            return new Result<T>(false, default, error, 500);
        }

        public static Result<T> TooManyRequest(string error = "Too many request")
        {
            return new Result<T>(false, default, error, 429);
        }
    }
}
