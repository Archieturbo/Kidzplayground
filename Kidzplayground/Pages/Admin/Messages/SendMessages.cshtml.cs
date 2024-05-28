using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kidzplayground.Data;
using Kidzplayground.Models;
using Kidzplayground.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Kidzplayground.Pages.Admin.Messages
{
    public class SendMessagesModel : PageModel
    {
        private readonly KidzplaygroundContext _context;
        private readonly UserManager<KidzplaygroundUser> _userManager;
        private readonly ILogger<SendMessagesModel> _logger;

        public SendMessagesModel(KidzplaygroundContext context, UserManager<KidzplaygroundUser> userManager, ILogger<SendMessagesModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Message NewMessage { get; set; }
        public List<Message> Messages { get; set; }
        public List<KidzplaygroundUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogError("User ID is null");
                return;
            }

            var messages = await _context.Messages
                .Where(m => m.SendTo == userId)
                .ToListAsync();

            Messages = messages;

            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Users = await _context.Users.ToListAsync();
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewMessage.SendFrom = userId;
            NewMessage.SendDate = DateTime.Now;

            _context.Messages.Add(NewMessage);
            await _context.SaveChangesAsync();

            Messages = await _context.Messages
                .Where(m => m.SendTo == userId)
                .OrderByDescending(m => m.SendDate)
                .ToListAsync();

            return RedirectToPage();
        }

        public string GetUsername(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            return user?.UserName ?? "Unknown";
        }
    }
}
