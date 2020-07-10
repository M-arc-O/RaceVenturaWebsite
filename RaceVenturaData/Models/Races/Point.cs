using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceVenturaData.Models.Races
{
    [Table("Points")]
    public class Point
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PointId { get; set; }

        [Required]
        public Guid StageId { get; set; }

        [Required]
        public PointType Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Value { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Answer { get; set; }

        public string Message { get; set; }

        public Point()
        {
        }
    }
}
