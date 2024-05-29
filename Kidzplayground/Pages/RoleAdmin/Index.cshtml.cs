using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kidzplayground.Pages.RoleAdmin
{
   
    public class IndexModel : PageModel
    {
        public List<Areas.Identity.Data.KidzplaygroundUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RoleName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AddUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RemoveUserId { get; set; }
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }

        public readonly UserManager<Areas.Identity.Data.KidzplaygroundUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<Areas.Identity.Data.KidzplaygroundUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync(); //Hämtar alla roller och användare från databasen
            Users = await _userManager.Users.ToListAsync();

            if(AddUserId !=null)
            {
               
                var alterUser = await _userManager.FindByIdAsync(AddUserId);
                await _userManager.AddToRoleAsync(alterUser, RoleName);
                await UpdateIsAdminAsync(alterUser);
            }
            if (RemoveUserId !=null)
            {
                var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
                await _userManager.RemoveFromRoleAsync(alterUser, RoleName);
                await UpdateIsAdminAsync(alterUser);
            }

            var currentUser = await _userManager.GetUserAsync(User); //Kolla om den aktuella användaren är vanlig användare eller admin
            if(currentUser != null)
            {
                IsUser = await _userManager.IsInRoleAsync(currentUser, "User");
                IsAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
                
            }
        }
        private async Task UpdateIsAdminAsync(Areas.Identity.Data.KidzplaygroundUser user) //Metod för att uppdatera om användaren är Admin baserat på deras roll
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            user.IsAdmin = isAdmin;
            await _userManager.UpdateAsync(user);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (RoleName != null)
            {
                await CreateRole(RoleName);
            }
            return RedirectToPage("./Index");
        }

        public async Task CreateRole(string roleName) //Metod för att skapa en ny roll om den inte redan finns
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var Role = new IdentityRole
                { 
                    Name = roleName 
                };
                await _roleManager.CreateAsync(Role);
            }
        }
    }
}
