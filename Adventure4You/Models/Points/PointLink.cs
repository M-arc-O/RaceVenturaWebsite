using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Points
{
    [Table("PointLinks")]
    public class PointLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid PointId { get; set; }

        [Required]
        public Guid StageId { get; set; }

        public PointLink()
        {
        }
    }
}
