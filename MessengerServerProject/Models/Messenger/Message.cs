namespace MessengerServerProject.Models.Messenger
{
    public class Message
    {
        public Message() { }
        public Message(string senderId, string receiverId, string messageContent)
        {
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
            this.MessageContent = messageContent;
            this.CreatedAt =  DateTime.Now;
        }
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string? MessageContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
