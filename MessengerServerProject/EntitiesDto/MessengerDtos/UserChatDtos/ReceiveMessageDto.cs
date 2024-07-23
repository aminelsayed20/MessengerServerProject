using MessengerServerProject.Models.Messenger;

namespace MessengerServerProject.EntitiesDto.MessengerDtos.UserChatDtos
{
    public class ReceiveMessageDto
    {
        public ReceiveMessageDto()
        {
        }

        public ReceiveMessageDto(string senderId, string senderName, string senderImage, string message)
        {
            SenderId = senderId;
            SenderName = senderName;
            SenderImage = senderImage;
            MessageContent = message;
        }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderImage { get; set; }
        public string MessageContent { get; set; }
    
    }
}
