namespace TodoApi.Errors
{
    public class UnathorizedException : Exception
    {
        public UnathorizedException(string message) : base(message) { }
    }
}
