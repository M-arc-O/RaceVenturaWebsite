using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("PointLinks")]
    public class PointLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PointId { get; set; }

        [Required]
        public int StageId { get; set; }

        public PointLink()
        {
        }
    }
}
