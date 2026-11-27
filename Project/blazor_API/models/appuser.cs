using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation_engine
{
    [Table("users")]
    public class AppUser
    {
        [Key]
        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }

        [Column("profile_description")]
        public string? ProfileDescription { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("profile_name")]
        public string? ProfileName { get; set; }
    }
}
