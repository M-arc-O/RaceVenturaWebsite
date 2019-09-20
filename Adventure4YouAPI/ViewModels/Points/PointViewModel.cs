using System;

namespace Adventure4YouAPI.ViewModels.Points
{
    public class PointViewModel
    {
        public Guid PointId { get; set; }
        public Guid StageId { get; set; }
        public string Name { get; set; }
    }
}
