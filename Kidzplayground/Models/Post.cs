using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kidzplayground.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string? Header { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
        public string? UserId { get; set; }
        public bool IsFlagged { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [BindProperty]
        public string? UserEmail { get; set; }
        public List<Comment> Comments { get; set; }
        public string? UserProfileImage { get; set; }
    }
}

