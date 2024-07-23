using MessengerServerProject.Data;
using MessengerServerProject.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerServerProject.Service
{
	public class UserService
	{
         private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
            _userRepository = userRepository;
		}

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersExceptAsync()
        {
           return await _userRepository.GetAllUsersExceptAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<ApplicationUser> GetCurrentAsync()
        {
            return await _userRepository.GetCurrentAsync();
        }

    }
}
