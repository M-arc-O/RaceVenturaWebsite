using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adventure4You.Models;

namespace Adventure4You.ViewModels.Points
{
    public class PointViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; }
        public string Coordinates { get; set; }

        public PointViewModel(Point point)
        {
            Id = point.Id;
            Name = point.Name;
            Value = point.Value;
            Coordinates = point.Coordinates;
        }
    }
}
