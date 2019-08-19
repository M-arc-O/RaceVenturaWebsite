using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Teams
{
    [Table("TeamStagesFinished")]
    public class TeamStageFinished
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid StageId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime StopTime { get; set; }
    }
}
