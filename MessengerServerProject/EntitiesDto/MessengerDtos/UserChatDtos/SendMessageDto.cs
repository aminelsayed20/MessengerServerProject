namespace MessengerServerProject.EntitiesDto.MessengerDtos.UserChatDtos
{
    public class SendMessageDto
    {

        public SendMessageDto(string senderId, string receiverId, string message)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageContent = message;
        }
        public string SenderId {  get; set; }
        public string ReceiverId { get; set; }
        public string MessageContent { get; set; }
    }
}
