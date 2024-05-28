using Kidzplayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kidzplayground.Data;
using Kidzplayground.Models;

namespace Kidzplayground.Pages.Admin
{
    public class ReviewModel : PageModel
    {
        private readonly KidzplaygroundContext _context;

        public ReviewModel(KidzplaygroundContext context)
        {
            _context = context;
        }

        public IList<Post> FlaggedPosts { get; set; }
        public IList<Comment> FlaggedComments { get; set; }

        public async Task OnGetAsync()
        {
            FlaggedPosts = await _context.Post
                .Where(p => p.IsFlagged)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            FlaggedComments = await _context.Comment
                .Where(c => c.IsFlagged)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostRemoveFlagAsync(int postId)
        {
            var post = await _context.Post.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            post.IsFlagged = false;
            await _context.SaveChangesAsync();
         
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveCommentFlagAsync(int commentId)
        {
            var comment = await _context.Comment.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            comment.IsFlagged = false;
            await _context.SaveChangesAsync();
          
            return RedirectToPage();
        }
    }
}
