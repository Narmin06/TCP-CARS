namespace CARS2;
public class Command
{
    public const string Get = "GET";
    public const string Post = "POST";
    public const string Put = "PUT";
    public const string Delete = "DELETE";
    public string? Text { get; set; }
    public Car Value { get; set; }
}
