namespace Net7Practice.Models
{
    public class ServiceResponse<T>
    {
        public object Data { get; set; }
        public bool Issuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
