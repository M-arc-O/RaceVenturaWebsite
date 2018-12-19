using Adventure4You.Models;
using System;

namespace Adventure4You.ViewModels.Races
{
    public class AddRaceViewModel
    {
        public string Name { get; set; }
        public bool CoordinatesCheckEnabled { get; set; }
        public bool SpecialTasksAreStage { get; set; }
        public int MaximumTeamSize { get; set; }
        public int MinimumPointsToCompleteStage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual Race ToRaceModel()
        {
            return new Race
            {
                Name = this.Name.Trim(),
                CoordinatesCheckEnabled = this.CoordinatesCheckEnabled,
                SpecialTasksAreStage = this.SpecialTasksAreStage,
                MaximumTeamSize = this.MaximumTeamSize,
                MimimumPointsToCompleteStage = this.MinimumPointsToCompleteStage,
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };
        }
    }
}
