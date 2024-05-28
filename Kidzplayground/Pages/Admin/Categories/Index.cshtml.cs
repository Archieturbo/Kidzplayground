using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kidzplayground.Data;
using Kidzplayground.Models;
using Microsoft.AspNetCore.Authorization;
using Kidzplayground.DAL;

namespace Kidzplayground.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Kidzplayground.Data.KidzplaygroundContext _context;

        public IndexModel(Kidzplayground.Data.KidzplaygroundContext context)
        {
            _context = context;
        }

        public List<Category> Category { get;set; } = default!;

        [BindProperty]
        public Category NewCategory { get;set; }

        public async Task OnGetAsync() //Hämtar kategorierna från API.
        {
            Category = await DataManagerAPI.GetCategories();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categories.Add(NewCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

           return RedirectToPage();
        }

    }
}
