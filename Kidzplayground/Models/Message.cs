using Kidzplayground.Areas.Identity.Data;

public class Message
{
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public string SendFrom { get; set; } = string.Empty;
    public string SendTo { get; set; }
    public DateTime SendDate { get; set; }
}



