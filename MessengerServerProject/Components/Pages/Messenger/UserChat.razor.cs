using Azure.Core;
using Azure.Messaging;
using MessengerServerProject.Dtos.Messenger;
using MessengerServerProject.EntitiesDto.MessengerDtos.UserChatDtos;
using MessengerServerProject.Migrations;
using MessengerServerProject.Models.Messenger;
using MessengerServerProject.Service;
using MessengerServerProject.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging.Console;

namespace MessengerServerProject.Components.Pages.Messenger
{
    public partial class UserChat : IDisposable
    {
        [Parameter]
        public string UserId { get; set; }

        private string OtherUserName;
        private string OtherUserId;
        private string OtherUserImg;

        public string CurrentUserId;
        private string CurrentUserImg;

        private int MessageCount;
        private bool IsOnline;

        //private HubConnection? hubConnection;
        private List<MessageDto> Messages = new List<MessageDto>();

        private string? userInput;
        private string? messageInput;

      //  [CascadingParameter]
     ///   private HttpContext HttpContext { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
           var chatDetails = await UserChatServices.GetChat(UserId);
            CurrentUserId = chatDetails.CurrentUserId;
            CurrentUserImg = chatDetails.CurrentUserImg;
            
            OtherUserId = chatDetails.OtherUserId;
            OtherUserName = chatDetails.OtherUserName;
            OtherUserImg = chatDetails.OtherUserImg;
            

            Messages = chatDetails.Messages;
            MessageCount = chatDetails.MessageCount;


        }
        protected async override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                await SignalRService.StartConnectionAsync();

                SignalRService._hubConnection.On<string, string, string, string >("ReceiveMessage", (senderId, senderName, senderImage, messageContent) =>
                {
                    Console.WriteLine(senderName);
                    Console.WriteLine(messageContent);
                    Messages.Add(new MessageDto(messageContent, false, DateTime.Now));
                    MessageCount++;
                    InvokeAsync(StateHasChanged);

                });

            }

            base.OnAfterRender(firstRender);
        }


        private async Task Send()
        {
            var sender = await UserService.GetCurrentAsync();
            if (!string.IsNullOrEmpty(messageInput) && SignalRService._hubConnection is not null)
            {

                await SignalRService._hubConnection.SendAsync("SendMessage", sender.Id, OtherUserId, messageInput);
                Messages.Add(new MessageDto(messageInput, true, DateTime.Now));
                MessageCount++;
                InvokeAsync(StateHasChanged);
                messageInput = string.Empty;
            }
        }


        public bool IsConnected =>
      SignalRService._hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (SignalRService._hubConnection is not null)
            {
                await SignalRService.StopConnectionAsync();
            }
        }


    }
}
