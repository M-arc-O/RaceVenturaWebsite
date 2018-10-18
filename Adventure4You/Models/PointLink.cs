using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("PointLinks")]
    public class PointLink
    {
        [Key]
        public int PointLinkId { get; set; }

        [Required]
        public int PointLinkPointId { get; set; }

        public int PointLinkVisitedTeamId { get; set; }

        public PointLink()
        {
        }
    }
}
