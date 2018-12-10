using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.Models
{
    [Table("TeamStagesFinished")]
    public class TeamStageFinished
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StageId { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime StopTime { get; set; }
    }
}
