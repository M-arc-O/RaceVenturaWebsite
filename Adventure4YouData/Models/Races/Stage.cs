using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4YouData.Models.Races
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
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int? MimimumPointsToCompleteStage { get; set; }

        public List<Point> Points { get; set; }

        public Stage()
        {

        }
    }
}
