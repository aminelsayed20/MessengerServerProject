
using Azure.Identity;
using MessengerServerProject.Components.Pages.Messenger;
using MessengerServerProject.Data;
using MessengerServerProject.Repository.Interfaces;
using MessengerServerProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MessengerServerProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersExceptAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            
            if (user == null)
            {
                return await _context.Users.ToListAsync();
            }

            return await _context.Users.Where(u => u.Id != user.Id).ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? new ApplicationUser();
        }

        public async Task<ApplicationUser> GetCurrentAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user != null)
            return user;

            return new ApplicationUser() { Id = "null"};

        }
    }
}
