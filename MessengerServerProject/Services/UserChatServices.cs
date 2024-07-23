using MessengerServerProject.Dtos.Messenger;
using MessengerServerProject.EntitiesDto.MessengerDtos.UserChatDtos;
using MessengerServerProject.Models.Messenger;
using MessengerServerProject.Repository;
using MessengerServerProject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MessengerServerProject.Services
{
    public class UserChatServices
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConnectionUserRepository _connectionUserRepository;
        private readonly IUserRepository _userRepository;
        public UserChatServices(IMessageRepository messageRepository, IConnectionUserRepository connectionUserRepository, 
            IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _connectionUserRepository = connectionUserRepository;
            _userRepository = userRepository;
        }
        public async Task<ReceiveMessageDto> SendUserMessageAsync (SendMessageDto sendMessageDto)
        {
            // save message to db
            var addmessageResult = await _messageRepository.AddAsync(sendMessageDto.SenderId, sendMessageDto.ReceiverId, sendMessageDto.MessageContent);
             if (!addmessageResult)
              {
                Console.WriteLine("message doesn't saved in database");
              }

            var senderUser =  _userRepository.GetByIdAsync(sendMessageDto.SenderId).Result;

            var senderId = senderUser.Id;
            var senderName = senderUser.FirstName + " " + senderUser.LastName;
            var senderImagePath = senderUser.ImgPath;
            var message = sendMessageDto.MessageContent;

            var receiveMessageDto =  new ReceiveMessageDto(senderId, senderName, senderImagePath, message);

            return  receiveMessageDto;

        }

        public async Task<ChatUserDto> GetChat (string otherUserId)
        {
            var currentUser = await _userRepository.GetCurrentAsync();
            var otherUser = await _userRepository.GetByIdAsync(otherUserId);

            var messages = await _messageRepository.GetUsersChat(currentUser.Id, otherUser.Id);

            var chatDto = new ChatUserDto
            {
                CurrentUserId = currentUser.Id,
                CurrentUserName = currentUser.FirstName + " " + currentUser.LastName,
                CurrentUserImg = currentUser.ImgPath,

                OtherUserId = otherUser.Id,
                OtherUserName = otherUser.FirstName + " " + otherUser.LastName,
                OtherUserImg = otherUser.ImgPath,
                OtherUserIsActive = otherUser.IsActive ?? false,

                Messages = messages,
                MessageCount = messages.Count()
            };
            return  chatDto;

        }
    }
}
