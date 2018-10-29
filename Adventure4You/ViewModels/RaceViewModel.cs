using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.ViewModels
{
    public class RaceViewModel
    {
        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public bool RaceCoordinatesCheckEnabled { get; set; }
    }
}
