using Adventure4You.Models;
using System;

namespace Adventure4You.ViewModels.Races
{
    public class RaceDetailViewModel: RaceViewModel
    {
        public bool CoordinatesCheckEnabled { get; set; }
        public bool SpecialTasksAreStage { get; set; }
        public int MaximumTeamSize { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public RaceDetailViewModel(Race race) : base(race)
        {
            CoordinatesCheckEnabled = race.CoordinatesCheckEnabled;
            SpecialTasksAreStage = race.SpecialTasksAreStage;
            MaximumTeamSize = race.MaximumTeamSize;
            StartTime = race.StartTime;
            EndTime = race.EndTime;
        }
    }
}
