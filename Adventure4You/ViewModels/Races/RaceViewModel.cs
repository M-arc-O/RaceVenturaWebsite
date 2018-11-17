using Adventure4You.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Adventure4You.ViewModels.Races
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
