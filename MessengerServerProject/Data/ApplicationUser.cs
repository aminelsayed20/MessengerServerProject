using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace MessengerServerProject.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImgPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; } = false;


    }

}
