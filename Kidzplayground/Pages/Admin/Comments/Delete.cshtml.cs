using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kidzplayground.Data;
using Kidzplayground.Models;

namespace Kidzplayground.Pages.Admin.Comments
{
    public class DeleteModel : PageModel
    {
        private readonly Kidzplayground.Data.KidzplaygroundContext _context;

        public DeleteModel(Kidzplayground.Data.KidzplaygroundContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Comment Comment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }
            else
            {
                Comment = comment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                Comment = comment;
                _context.Comment.Remove(Comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
