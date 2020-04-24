using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Races.MappingProfiles;
using Adventure4YouData.Models.Races;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Adventure4YouAPITest.ViewModels.Races.MappingProfiles
{
    [TestClass]
    public class TeamMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TeamMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void ModelToViewModelTest()
        {
            var model = new Team
            {
                Name = "c",
                Number = 2,
                RaceId = Guid.NewGuid(),
                FinishedStages = new List<FinishedStage>(),
                FinishTime = DateTime.Now,
                TeamId = Guid.NewGuid(),
                VisitedPoints = new List<VisitedPoint>()
            };

            var result = _Sut.Map<TeamViewModel>(model);

            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Number, result.Number);
            Assert.AreEqual(model.RaceId, result.RaceId);
            Assert.IsNotNull(result.FinishedStages);
            Assert.AreEqual(model.FinishTime, result.FinishTime);
            Assert.IsNotNull(result.VisitedPoints);
        }

        [TestMethod]
        public void ViewModelToModelTest()
        {
            var viewModel = new TeamViewModel
            {
                Name = "c",
                Number = 2,
                RaceId = Guid.NewGuid(),
                FinishedStages = new List<FinishedStageViewModel>(),
                FinishTime = DateTime.Now,
                TeamId = Guid.NewGuid(),
                VisitedPoints = new List<VisitedPointViewModel>()
            };

            var result = _Sut.Map<TeamViewModel>(viewModel);

            Assert.AreEqual(viewModel.Name, result.Name);
            Assert.AreEqual(viewModel.Number, result.Number);
            Assert.AreEqual(viewModel.RaceId, result.RaceId);
            Assert.IsNotNull(result.FinishedStages);
            Assert.AreEqual(viewModel.FinishTime, result.FinishTime);
            Assert.IsNotNull(result.VisitedPoints);
        }
    }
}
