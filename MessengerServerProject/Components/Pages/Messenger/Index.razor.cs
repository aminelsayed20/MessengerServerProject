using MessengerServerProject.Data;
using Microsoft.AspNetCore.Components;

namespace MessengerServerProject.Components.Pages.Messenger
{
    public partial class Index
    {
        private List<ApplicationUser> users;
        private List<ApplicationUser> filteredUsers;

        protected override async Task OnInitializedAsync()
        {
            users =  await UserService.GetAllUsersExceptAsync();
            filteredUsers = users;
        }

        private void FilterUsers(ChangeEventArgs e)
        {
            var searchValue = e.Value.ToString().ToLower();
            filteredUsers = users.Where(u => u.UserName.ToLower().Contains(searchValue)).ToList();
        }
        private void OpenChat(string userId)
        {
            NavigationManager.NavigateTo($"/Messenger/UserChat/{userId}");
        }

    }
}
