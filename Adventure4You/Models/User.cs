using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(255)]
        public string UserName { get; set; }

        [MaxLength(255)]
        public string UserEmail { get; set; }

        [MaxLength(64)]
        public string UserPassword { get; set; }

        public User()
        {
        }
    }
}
