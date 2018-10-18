using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models
{
    [Table("TeamLinks")]
    public class TeamLink
    {
        [Key]
        public int TeamLinkId { get; set; }

        public int TeamLinkTeamId { get; set; }

        public int TeamLinkRaceId { get; set; }

        public TeamLink()
        {
        }
    }
}
