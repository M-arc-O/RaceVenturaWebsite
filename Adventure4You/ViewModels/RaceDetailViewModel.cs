using Adventure4You.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.ViewModels
{
    public class RaceDetailViewModel: RaceViewModel
    {
        public bool CoordinatesCheckEnabled { get; set; }
        public bool SpecialTasksAreStage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public RaceDetailViewModel(Race race) : base(race)
        {
            CoordinatesCheckEnabled = race.CoordinatesCheckEnabled;
            SpecialTasksAreStage = race.SpecialTasksAreStage;
            StartTime = race.StartTime;
            EndTime = race.EndTime;
        }
    }
}
