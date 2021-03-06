﻿using RaceVenturaAPI.ViewModels.Validators;
using System;

namespace RaceVenturaAPI.ViewModels.Races
{
    public class VisitedPointViewModel
    {
        public Guid VisitedPointId { get; set; }

        [RequiredNotEmpty]
        public Guid PointId { get; set; }

        [RequiredNotEmpty]
        public Guid TeamId { get; set; }

        public DateTime Time { get; set; }
    }
}
