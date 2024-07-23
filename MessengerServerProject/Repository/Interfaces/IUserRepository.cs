using MessengerServerProject.Data;

namespace MessengerServerProject.Repository.Interfaces
{
    public interface IUserRepository 
    {
        public Task<List<ApplicationUser>> GetAllUsersAsync();
        public Task<List<ApplicationUser>> GetAllUsersExceptAsync();

        public Task <ApplicationUser> GetCurrentAsync();
        public Task<ApplicationUser> GetByIdAsync(string id);

    }
}
