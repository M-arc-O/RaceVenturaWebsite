using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4YouData.Models.Races
{
    [Table("FinishedStages")]
    public class FinishedStage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FinishedStageId { get; set; }

        [Required]
        public Guid StageId { get; set; }

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public DateTime FinishTime { get; set; }
    }
}
