using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.ViewModels.Points
{
    public class AddPointViewModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Coordinates { get; set; }
        public int StageId { get; internal set; }
    }
}
