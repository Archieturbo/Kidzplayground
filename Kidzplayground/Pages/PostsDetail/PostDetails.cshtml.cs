using Kidzplayground.Data;
using Kidzplayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kidzplayground.Pages.Posts
{
    public class PostDetailsModel : PageModel
    {
        private readonly KidzplaygroundContext _context;

        public PostDetailsModel(KidzplaygroundContext context)
        {
            _context = context;
        }

        public Post Post { get; set; }
        [BindProperty]
        public List<Comment> Comments { get; set; }
        [BindProperty]
        public Comment NewComment { get; set; } = new Comment();

        public async Task<IActionResult> OnGetAsync(int id, int? deleteComment)
        {
            if (deleteComment.HasValue)
            {
                Models.Comment commentToBeDeleted = await _context.Comment.FindAsync(deleteComment.Value);
                if (commentToBeDeleted != null)
                {
                    _context.Comment.Remove(commentToBeDeleted);
                    await _context.SaveChangesAsync();
                }
            }

            Post = await _context.Post.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
            if (Post == null)
            {
                return NotFound();
            }

            var userpost = await _context.Users.FirstOrDefaultAsync(u => u.Id == Post.UserId);
            if (userpost != null)
            {
                Post.UserEmail = userpost.Email;
                Post.UserProfileImage = userpost.ProfileImage;

            }

            foreach (var comment in Post.Comments)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId);
                if (user != null)
                {
                    comment.UserEmail = user.Email;
                    comment.UserProfileImage = user.ProfileImage;
                }
            }

            Comments = Post?.Comments ?? new List<Comment>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Post = await _context.Post.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == NewComment.PostId);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewComment.UserId = userId;
            NewComment.Date = DateTime.Now;

            _context.Comment.Add(NewComment);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id = NewComment.PostId });
        }

        public async Task<IActionResult> OnPostFlagPostAsync(int postId)
        {
            var post = await _context.Post.FindAsync(postId); 
            if (post == null)
            {
                return NotFound();
            }
            post.IsFlagged = true;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Inlägget har blivit skickat till en Admin för inspektion.";
            return RedirectToPage(); 
        }

        public async Task<IActionResult> OnPostFlagCommentAsync(int commentId)
        {
            var comment = await _context.Comment.FindAsync(commentId); 
            if (comment == null)
            {
                return NotFound();
            }
            comment.IsFlagged = true;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Kommentaren har blivit skickad till en Admin för inspektion.";
            return RedirectToPage(); 
        }
    }
}
