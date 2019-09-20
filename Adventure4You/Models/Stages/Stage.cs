using Adventure4You.Models.Points;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Stages
{
    [Table("Stages")]
    public class Stage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StageId { get; set; }

        [Required]
        public Guid RaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? MimimumPointsToCompleteStage { get; set; }

        public List<Point> Points { get; set; }

        public Stage()
        {

        }
    }
}
