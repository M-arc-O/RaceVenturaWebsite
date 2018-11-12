using Adventure4You.Models;
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

        public RaceViewModel(Race race)
        {
            Id = race.Id;
            Name = race.Name;
        }
    }
}
