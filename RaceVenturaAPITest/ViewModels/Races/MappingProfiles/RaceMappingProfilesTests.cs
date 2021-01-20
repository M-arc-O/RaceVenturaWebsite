using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels.Races.MappingProfiles;
using RaceVenturaData.Models.Races;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RaceVenturaAPITest.ViewModels.Races.MappingProfiles
{
    [TestClass]
    public class RaceMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RaceMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void ModelToViewModelTest()
        {
            var model = new Race
            {
                CoordinatesCheckEnabled = true,
                MaxDuration = null,
                MaximumTeamSize = 1,
                MinimumPointsToCompleteStage = 1,
                Name = "a",
                PenaltyPerMinuteLate = 1,
                RaceId = Guid.NewGuid(),
                SpecialTasksAreStage = true,
                Stages = new List<Stage>(),
                StartTime = DateTime.Now,
                Teams = new List<Team>(),
            };

            var result = _Sut.Map<RaceViewModel>(model);

            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.RaceId, result.RaceId);
        }

        [TestMethod]
        public void ViewModelToModelTest()
        {
            var viewModel = new RaceViewModel
            {
                Name = "a",
                RaceId = Guid.NewGuid()
            };

            var result = _Sut.Map<Race>(viewModel);

            Assert.AreEqual(viewModel.Name, result.Name);
            Assert.AreEqual(viewModel.RaceId, result.RaceId);
        }

        [TestMethod]
        public void ModelToDetailViewModelTest()
        {
            var model = new Race
            {
                CoordinatesCheckEnabled = true,
                MaxDuration = new TimeSpan(),
                MaximumTeamSize = 1,
                MinimumPointsToCompleteStage = 1,
                Name = "a",
                PenaltyPerMinuteLate = 1,
                RaceId = Guid.NewGuid(),
                SpecialTasksAreStage = true,
                Stages = new List<Stage>(),
                StartTime = DateTime.Now,
                Teams = new List<Team>(),
            };

            var result = _Sut.Map<RaceDetailViewModel>(model);

            Assert.AreEqual(model.CoordinatesCheckEnabled, result.CoordinatesCheckEnabled);
            Assert.AreEqual(model.MaxDuration, result.MaxDuration);
            Assert.AreEqual(model.MaximumTeamSize, result.MaximumTeamSize);
            Assert.AreEqual(model.MinimumPointsToCompleteStage, result.MinimumPointsToCompleteStage);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.PenaltyPerMinuteLate, result.PenaltyPerMinuteLate);
            Assert.AreEqual(model.RaceId, result.RaceId);
            Assert.AreEqual(model.SpecialTasksAreStage, result.SpecialTasksAreStage);
            Assert.IsNotNull(result.Stages);
            Assert.AreEqual(model.StartTime, result.StartTime);
            Assert.IsNotNull(result.Teams);
        }

        [TestMethod]
        public void DetailViewModelToModelTest()
        {
            var viewModel = new RaceDetailViewModel
            {
                CoordinatesCheckEnabled = true,
                MaxDuration = new TimeSpan(),
                MaximumTeamSize = 1,
                MinimumPointsToCompleteStage = 1,
                Name = "a",
                PenaltyPerMinuteLate = 1,
                RaceId = Guid.NewGuid(),
                SpecialTasksAreStage = true,
                Stages = new List<StageViewModel>(),
                StartTime = DateTime.Now,
                Teams = new List<TeamViewModel>(),
            };

            var result = _Sut.Map<Race>(viewModel);

            Assert.AreEqual(viewModel.CoordinatesCheckEnabled, result.CoordinatesCheckEnabled);
            Assert.AreEqual(viewModel.MaxDuration, result.MaxDuration);
            Assert.AreEqual(viewModel.MaximumTeamSize, result.MaximumTeamSize);
            Assert.AreEqual(viewModel.MinimumPointsToCompleteStage, result.MinimumPointsToCompleteStage);
            Assert.AreEqual(viewModel.Name, result.Name);
            Assert.AreEqual(viewModel.PenaltyPerMinuteLate, result.PenaltyPerMinuteLate);
            Assert.AreEqual(viewModel.RaceId, result.RaceId);
            Assert.AreEqual(viewModel.SpecialTasksAreStage, result.SpecialTasksAreStage);
            Assert.IsNotNull(result.Stages);
            Assert.AreEqual(viewModel.StartTime, result.StartTime);
            Assert.IsNotNull(result.Teams);
        }
    }
}
