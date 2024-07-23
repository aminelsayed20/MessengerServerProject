using MessengerServerProject.Data;
using MessengerServerProject.Models.Messenger;
using MessengerServerProject.Repository.Interfaces;
using MessengerServerProject.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerServerProject.Repository
{
    public class ConnectionUserRepository : IConnectionUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        public ConnectionUserRepository(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public async Task<bool> AddAsync(string userId, string connectionId)
        {
            
            // Add the new ConnectionUser record
            if (userId is not "null" && connectionId is not null)
            _context.ConnectionUsers.Add(new ConnectionUser(userId, connectionId));
            _context.SaveChanges();
            // Save changes to the database
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(string userId, string connectionId)
        {

            var currentUser =  await _userService.GetByIdAsync(userId);  // Retrieve the user 

            if (currentUser is null || connectionId is null) // cheeck if user exist
            {
                return false;
            }

            // remove all connections if there duplicated 
            var connectionUser = _context.ConnectionUsers
            .Where(cu => cu.ConnectionId == connectionId);

            _context.ConnectionUsers.RemoveRange(connectionUser);


            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> GetAllConnectionsIdAsync(string userId)
        {
            var connectionIds = _context.ConnectionUsers.Where(c => c.UserId == userId);
            if (connectionIds.Any())
            {
                return await connectionIds.Select(c=>c.ConnectionId).ToListAsync();
            }

            else return new List<string>();
        }
    }
}
