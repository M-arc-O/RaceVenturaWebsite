using Adventure4You.Models.Stages;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4YouAPI.ViewModels.Stages
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Stage, StageViewModel>();
            CreateMap<StageViewModel, Stage>();
            
            CreateMap<Stage, AddStageViewModel>();
            CreateMap<AddStageViewModel, Stage>();
        }
    }
}
