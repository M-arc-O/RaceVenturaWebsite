using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure4You.Models.Stages
{
    [Table("Stages")]
    public class Stage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid RaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? MimimumPointsToCompleteStage { get; set; }

        public Stage()
        {

        }
    }
}
