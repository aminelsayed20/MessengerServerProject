namespace MessengerServerProject.Models.Messenger
{
    public class ConnectionUser
    {
        public ConnectionUser() { }
        public ConnectionUser(string userId, string connectionid)
        {
            UserId = userId;
            ConnectionId = connectionid;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? ConnectionId { get; set; }
    }
}
