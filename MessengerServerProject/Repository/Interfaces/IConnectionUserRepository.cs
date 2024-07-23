using MessengerServerProject.Models.Messenger;

namespace MessengerServerProject.Repository.Interfaces
{
    public interface IConnectionUserRepository
    {
        public Task<bool> AddAsync(string userId, string ConnectionId);
        public Task<bool> RemoveAsync(string userId, string ConnectionId);
        public Task<List<string>> GetAllConnectionsIdAsync(string userId);
    }
}
