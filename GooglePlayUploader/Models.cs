namespace GooglePlayUploader
{
    public class Edit
    {
        public string Id { get; set; }
        public int ExpiryTimeSeconds { get; set; }
    }

    public class GoogleError
    {
        public GoogleErrorMessage error { get; set; }
    }

    public class GoogleErrorMessage
    {
        public int code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}