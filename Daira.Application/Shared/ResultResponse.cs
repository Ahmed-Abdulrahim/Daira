namespace Daira.Application.Shared
{
    public class ResultResponse<T>
    {
        public bool Succeeded { get; set; }
        public T? Data { get; set; }
        public List<T>? ListOfData { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static ResultResponse<T> Success(T data, string message = "Operation completed successfully.")
        {
            return new ResultResponse<T>
            {
                Succeeded = true,
                Data = data,
                Message = message
            };
        }
        public static ResultResponse<T> Success(List<T> listOfData, string message = "Operation completed successfully.")
        {
            return new ResultResponse<T>
            {
                Succeeded = true,
                ListOfData = listOfData,
                Message = message
            };
        }

        public static ResultResponse<T> Success(string message = "Operation completed successfully.")
        {
            return new ResultResponse<T>
            {
                Succeeded = true,
                Message = message
            };
        }

        public static ResultResponse<T> Failure(string message)
        {
            return new ResultResponse<T>
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
        public static ResultResponse<T> Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new ResultResponse<T>
            {
                Succeeded = false,
                Message = errorList.FirstOrDefault() ?? "Operation failed.",
                Errors = errorList
            };
        }
    }
}
