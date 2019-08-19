using System;

namespace Adventure4YouAPI.ViewModels.Points
{
    public class PointViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Value { get; }
        public string Coordinates { get; set; }
    }
}
