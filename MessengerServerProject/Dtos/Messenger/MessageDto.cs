namespace MessengerServerProject.Dtos.Messenger
{
        public class MessageDto(string message, bool isSender, DateTime time)
        {
            public string Message { get; set; } = message;
            public DateTime Time { get; set; } = time;
            public bool IsSender { get; set; } = isSender;
        }

}
