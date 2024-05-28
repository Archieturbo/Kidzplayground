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
    public class IndexModel : PageModel
    {
        private readonly Kidzplayground.Data.KidzplaygroundContext _context;

        public IndexModel(Kidzplayground.Data.KidzplaygroundContext context)
        {
            _context = context;
        }

        public IList<Comment> Comment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Comment = await _context.Comment.ToListAsync();
        }
    }
}
