using Kidzplayground.Areas.Identity.Data;
using Kidzplayground.DAL;
using Kidzplayground.Data;
using Kidzplayground.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kidzplayground.Pages
{
    public class IndexModel : PageModel
    {
        private readonly KidzplaygroundContext _context;
        private readonly UserManager<KidzplaygroundUser> _userManager;
        private readonly SignInManager<KidzplaygroundUser> _signInManager;

        public IndexModel(KidzplaygroundContext context, UserManager<KidzplaygroundUser> userManager, SignInManager<KidzplaygroundUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public Models.Post Post { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        [BindProperty]
        public Comment NewComment { get; set; }

        public List<Models.Post> Posts { get; set; }

        public IList<Category> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }
        public string SelectedCategoryName { get; set; }

        public async Task OnGetAsync(int? deleteId)
        {
            Categories = await DataManagerAPI.GetCategories(); //Hämtar kategorier via API

            if (deleteId.HasValue) //Raderar inlägg om det finns
            {
                Models.Post postToBeDeleted = await _context.Post.FindAsync(deleteId.Value);

                if (postToBeDeleted != null)
                {
                    if (System.IO.File.Exists("./wwwroot/userImages/" + postToBeDeleted.Image))
                    {
                        System.IO.File.Delete("./wwwroot/userImages/" + postToBeDeleted.Image);
                    }
                    _context.Post.Remove(postToBeDeleted);
                    await _context.SaveChangesAsync();
                }
            }

            if (CategoryId.HasValue) //Hämta inlägg baserat på kategori
            {
                if (CategoryId.Value == 0)
                {
                    SelectedCategoryName = "Alla inlägg";
                    Posts = await _context.Post.OrderByDescending(p => p.Date).ToListAsync();
                }
                else
                {
                    var selectedCategory = await _context.Categories.FindAsync(CategoryId.Value);
                    SelectedCategoryName = selectedCategory?.Name ?? "Inlägg";
                    Posts = await _context.Post.Where(p => p.CategoryId == CategoryId.Value).ToListAsync();
                }
            }
            else
            {
                SelectedCategoryName = "Nya inlägg";
                Posts = await _context.Post.OrderByDescending(p => p.Date).Take(4).ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var image = UploadedImage;
            string fileName = "";

            if (image != null) //Spara uppladdad bild om den finns tillgänglig
            {
                Random rnd = new Random();
                fileName = rnd.Next(0, 100000).ToString() + image.FileName;
                using (var fileStream = new FileStream("./wwwroot/userImages/" + fileName, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            Post.Date = DateTime.Now;
            Post.Image = fileName;
            Post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Post.Add(Post);

            await _context.SaveChangesAsync();//Sparar det nya inlägget i databasen

            return RedirectToPage();
        }
    }
}
