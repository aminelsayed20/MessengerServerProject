using MessengerServerProject.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace MessengerServerProject.Services
{
    public class SignalRService 
    {
        private readonly NavigationManager _navigation;
        private readonly UserService _userService;
        public HubConnection _hubConnection;
        private string _userId;

        public SignalRService(NavigationManager navigation, UserService userService)
        {
            _navigation = navigation;
            _userService = userService;
        }

        public async Task StartConnectionAsync()
        {
            var user = await _userService.GetCurrentAsync();
            _userId = user.Id;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigation.ToAbsoluteUri("/messengerhub") + "?userId=" + _userId)
                .Build();

            await _hubConnection.StartAsync();
        }

       public async Task StopConnectionAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }

/*        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                StopConnectionAsync();
            }
        }*/
    }
}
