using MessengerServerProject.Data;
using MessengerServerProject.EntitiesDto.MessengerDtos.UserChatDtos;
using MessengerServerProject.Repository.Interfaces;
using MessengerServerProject.Service;
using MessengerServerProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.ResponseCompression;
using Azure.Messaging;
using System.Reflection;
using System.Net.Sockets;

namespace MessengerServerProject.Hubs
{
    public class MessengerHub : Hub
    {
        private readonly IConnectionUserRepository _connectionUserRepository;
        private readonly UserChatServices _userChatServices;
        private string userId { get; set; }

        public MessengerHub(IConnectionUserRepository connectionUserRepository, UserChatServices userChatServices)
        {
            _connectionUserRepository = connectionUserRepository;
            _userChatServices = userChatServices;
        }


               public async Task SendMessage(string senderIdDto, string receiverIdDto, string  messageContentDto)
                {
                    var sendMessageDto = new SendMessageDto(senderIdDto, receiverIdDto, messageContentDto);
                   // Save message to DB
                    var receiveMessageDto = await _userChatServices.SendUserMessageAsync(sendMessageDto);

                    var senderId = receiveMessageDto.SenderId;
                    var senderName = receiveMessageDto.SenderName;
                    var senderImage = receiveMessageDto.SenderImage;
                    var messageContent = receiveMessageDto.MessageContent;


            // Get all connection IDs of the receiver
            var connectionIds = await _connectionUserRepository.GetAllConnectionsIdAsync(sendMessageDto.ReceiverId);

                    if (connectionIds != null)
                    {
                        foreach (var connectionId in connectionIds)
                        {
                            await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, senderName, senderImage, messageContent);
                        }
                    }
                }
        


/*      public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }*/

        public override async Task OnConnectedAsync()
        {

            userId = Context.GetHttpContext().Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                var connectionId = Context.ConnectionId;
                await _connectionUserRepository.AddAsync(userId, connectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            userId = Context.GetHttpContext().Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                var connectionId = Context.ConnectionId;
                await _connectionUserRepository.RemoveAsync(userId, connectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
