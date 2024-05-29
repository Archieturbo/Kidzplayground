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
            if (deleteComment.HasValue)     //Radera kommentarer
            {
                Models.Comment commentToBeDeleted = await _context.Comment.FindAsync(deleteComment.Value);
                if (commentToBeDeleted != null)
                {
                    _context.Comment.Remove(commentToBeDeleted);
                    await _context.SaveChangesAsync();
                }
            }

            Post = await _context.Post.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id); //H�mtar detaljer om inl�gget inklusive kommentarer
            if (Post == null)
            {
                return NotFound();
            }

            var userpost = await _context.Users.FirstOrDefaultAsync(u => u.Id == Post.UserId); //H�mtar information om inl�ggets f�rfattare
            if (userpost != null)
            {
                Post.UserEmail = userpost.Email;
                Post.UserProfileImage = userpost.ProfileImage;

            }

            foreach (var comment in Post.Comments)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId); //H�mtar information om kommentaren
                if (user != null)
                {
                    comment.UserEmail = user.Email;
                    comment.UserProfileImage = user.ProfileImage;
                }
            }

            Comments = Post?.Comments ?? new List<Comment>(); //H�mtar kommentarer eller en tom lista
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

            _context.Comment.Add(NewComment); //L�gg till nya kommentaren i databasen
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id = NewComment.PostId }); //Returnerar till uppdaterade sidan med nya kommentaren
        }

        public async Task<IActionResult> OnPostFlagPostAsync(int postId) //Hanterar funktionen att flagga inl�gg
        {
            var post = await _context.Post.FindAsync(postId); 
            if (post == null)
            {
                return NotFound();
            }
            post.IsFlagged = true;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Inl�gget har blivit skickat till en Admin f�r inspektion.";
            return RedirectToPage(); 
        }

        public async Task<IActionResult> OnPostFlagCommentAsync(int commentId) //Hanterar funktionen att flagga kommentarer
        {
            var comment = await _context.Comment.FindAsync(commentId); 
            if (comment == null)
            {
                return NotFound();
            }
            comment.IsFlagged = true;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Kommentaren har blivit skickad till en Admin f�r inspektion.";
            return RedirectToPage(); 
        }
    }
}
