using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Adventure4You.Models.Teams
{
    [Table("TeamPhones")]
    public class TeamPhone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public Guid PhoneId { get; set; }
    }
}
