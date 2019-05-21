using Adventure4YouAPI.Models;
using System;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class RaceDetailViewModel: RaceViewModel
    {
        public bool CoordinatesCheckEnabled { get; set; }
        public bool SpecialTasksAreStage { get; set; }
        public int MaximumTeamSize { get; set; }
        public int MinimumPointsToCompleteStage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public RaceDetailViewModel()
        {

        }

        public RaceDetailViewModel(Race race) : base(race)
        {
            CoordinatesCheckEnabled = race.CoordinatesCheckEnabled;
            SpecialTasksAreStage = race.SpecialTasksAreStage;
            MaximumTeamSize = race.MaximumTeamSize;
            MinimumPointsToCompleteStage = race.MinimumPointsToCompleteStage;
            StartTime = race.StartTime;
            EndTime = race.EndTime;
        }

        public virtual Race ToRaceModel()
        {
            return new Race
            {
                Id = this.Id,
                Name = this.Name,
                CoordinatesCheckEnabled = this.CoordinatesCheckEnabled,
                SpecialTasksAreStage = this.SpecialTasksAreStage,
                MaximumTeamSize = this.MaximumTeamSize,
                MinimumPointsToCompleteStage = this.MinimumPointsToCompleteStage,
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };
        }
    }
}
