using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Stages
{
    [Table("StageLinks")]
    public class StageLink
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StageId { get; set; }

        [Required]
        public int RaceId { get; set; }

        public StageLink()
        {

        }
    }
}
