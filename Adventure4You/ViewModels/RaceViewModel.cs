using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4You.ViewModels
{
    public class RaceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CoordinatesCheckEnabled { get; set; }
        public bool SpecialTasksAreStage { get; set; }
    }
}
