namespace CompanySystem.Business.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }

        private ServiceResult(bool isSuccess, string message, T? data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static ServiceResult<T> Success(T data, string message = "Operation completed successfully")
        {
            return new ServiceResult<T>(true, message, data);
        }

        public static ServiceResult<T> Failure(string message)
        {
            return new ServiceResult<T>(false, message);
        }
    }
}
