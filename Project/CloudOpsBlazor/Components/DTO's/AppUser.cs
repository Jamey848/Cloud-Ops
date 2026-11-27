using System.ComponentModel.DataAnnotations;
using CloudOpsBlazor.Components;

namespace CloudOpsBlazor.Components
{
    public class AppUser
    {
        public string UserId { get; set; }

        public string? ProfilePicture { get; set; }

        public string? ProfileDescription { get; set; }

        public string? Email { get; set; }

        public string? ProfileName { get; set; }
    }
}
