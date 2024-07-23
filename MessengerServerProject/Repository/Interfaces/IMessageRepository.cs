using MessengerServerProject.Dtos.Messenger;

namespace MessengerServerProject.Repository.Interfaces
{
    public interface IMessageRepository
    {
        public Task<bool> AddAsync (string senderId, string receiverId, string message);
        public Task<List<MessageDto>> GetUsersChat(string currentUserId, string otherUserId);


    }
}
