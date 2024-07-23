using MessengerServerProject.Data;
using MessengerServerProject.Dtos.Messenger;
using MessengerServerProject.Models.Messenger;
using MessengerServerProject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace MessengerServerProject.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(string senderId, string receiverId, string message)
        {
            if (senderId.IsNullOrEmpty() || receiverId.IsNullOrEmpty())
            {
                return  false;
            }

            _context.Messages.Add(new Message(senderId, receiverId, message));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<MessageDto>> GetUsersChat(string currentUserId, string otherUserId)
        {
            var chat = _context.Messages
                .Where(um => (um.SenderId == currentUserId && um.ReceiverId == otherUserId)
                             || (um.ReceiverId == currentUserId && um.SenderId == otherUserId))
                .OrderBy(um => um.CreatedAt);

            var messages = await chat
               .Select(ch => new MessageDto(ch.MessageContent, ch.SenderId == currentUserId, ch.CreatedAt))
                .ToListAsync();

            return messages;
        }
    }
}
