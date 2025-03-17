
namespace Extensions.Logging.Prime.Model
{
    internal class PrimeApiResult
    {
        public PrimeApiResult() { }

        public PrimeApiResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; } = 200;

        public string? Message { get; set; }

        public static PrimeApiResult Success(string message = "")
        {
            return new PrimeApiResult(200, message);
        }

        public static PrimeApiResult Error(string message = "")
        {
            return new PrimeApiResult(500, message);
        }
    }

    internal class PrimeApiResult<T> : PrimeApiResult
    {
        public PrimeApiResult(int code, string message) : base(code, message) { }

        public PrimeApiResult(T data)
        {
            Data = data;
        }

        public T? Data { get; set; }

        public static PrimeApiResult<T> Success(T data)
        {
            return new PrimeApiResult<T>(data);
        }
    }
}
