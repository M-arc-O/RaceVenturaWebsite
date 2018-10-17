using System;
using System.ComponentModel.DataAnnotations;

namespace Adventure4You.Models
{
    public class RaceModel
    {
        [Key]
        public int RaceId { get; set; }

        public string RaceName { get; set; }
        public string RaceGuid { get; set; }
        public bool RaceCoordinatesCheckEnabled { get; set; }

        public RaceModel()
        {
        }
    }
}
