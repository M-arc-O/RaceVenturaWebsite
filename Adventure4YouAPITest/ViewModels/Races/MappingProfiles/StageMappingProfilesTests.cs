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
    public class StageMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new StageMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void ModelToViewModelTest()
        {
            var model = new Stage
            {
                Name = "c",
                StageId = Guid.NewGuid(),
                MimimumPointsToCompleteStage = 1,
                Number = 2,
                Points = new List<Point>(),
                RaceId = Guid.NewGuid()
            };

            var result = _Sut.Map<StageViewModel>(model);

            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.StageId, result.StageId);
            Assert.AreEqual(model.MimimumPointsToCompleteStage, result.MimimumPointsToCompleteStage);
            Assert.AreEqual(model.Number, result.Number);
            Assert.IsNotNull(result.Points);
            Assert.AreEqual(model.RaceId, result.RaceId);
        }

        [TestMethod]
        public void ViewModelToModelTest()
        {
            var viewModel = new StageViewModel
            {
                Name = "c",
                StageId = Guid.NewGuid(),
                MimimumPointsToCompleteStage = 1,
                Number = 2,
                Points = new List<PointViewModel>(),
                RaceId = Guid.NewGuid()
            };

            var result = _Sut.Map<Stage>(viewModel);

            Assert.AreEqual(viewModel.Name, result.Name);
            Assert.AreEqual(viewModel.StageId, result.StageId);
            Assert.AreEqual(viewModel.MimimumPointsToCompleteStage, result.MimimumPointsToCompleteStage);
            Assert.AreEqual(viewModel.Number, result.Number);
            Assert.IsNotNull(result.Points);
            Assert.AreEqual(viewModel.RaceId, result.RaceId);
        }
    }
}
