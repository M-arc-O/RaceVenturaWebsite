using System;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class PointViewModel
    {
        public Guid PointId { get; set; }
        public Guid StageId { get; set; }
        public string Name { get; set; }
        public PointTypeViewModel Type { get; set; }
        public int Value { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Answer { get; set; }
        public string Message { get; set; }
    }
}
