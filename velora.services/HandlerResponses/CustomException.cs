namespace velora.services.HandlerResponses
{
    public class CustomException : ApiResponse<string> 
    {
        public CustomException(int statusCode, string? message = null, string? details = null)
           : base(null, true, statusCode, message)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
