namespace Kidzplayground.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TextContent { get; set; }
        public DateTime? Date { get; set; }
        public bool IsFlagged { get; set; }
        public int? PostId { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserProfileImage { get; set; }
    }
}
