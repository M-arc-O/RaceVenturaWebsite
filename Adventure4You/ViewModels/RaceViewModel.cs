using System;
namespace Adventure4You.ViewModels
{
    public class RaceViewModel
    {
        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public string RaceGuid { get; set; }
        public bool RaceCoordinatesCheckEnabled { get; set; }

        public RaceViewModel()
        {
        }
    }
}
