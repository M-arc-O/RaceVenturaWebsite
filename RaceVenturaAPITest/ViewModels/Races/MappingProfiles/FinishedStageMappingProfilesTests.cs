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
    public class FinishedStageMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinishedStageMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void ModelToViewModelTest()
        {
            var model = new FinishedStage
            {
                TeamId = Guid.NewGuid(),
                FinishedStageId = Guid.NewGuid(),
                FinishTime = DateTime.Now,
                StageId = Guid.NewGuid()
            };

            var result = _Sut.Map<FinishedStageViewModel>(model);

            Assert.AreEqual(model.TeamId, result.TeamId);
            Assert.AreEqual(model.FinishedStageId, result.FinishedStageId);
            Assert.AreEqual(model.FinishTime, result.FinishTime);
            Assert.AreEqual(model.StageId, result.StageId);
        }

        [TestMethod]
        public void ViewModelToModelTest()
        {
            var viewModel = new FinishedStageViewModel
            {
                TeamId = Guid.NewGuid(),
                FinishedStageId = Guid.NewGuid(),
                FinishTime = DateTime.Now,
                StageId = Guid.NewGuid()
            };

            var result = _Sut.Map<FinishedStage>(viewModel);

            Assert.AreEqual(viewModel.TeamId, result.TeamId);
            Assert.AreEqual(viewModel.FinishedStageId, result.FinishedStageId);
            Assert.AreEqual(viewModel.FinishTime, result.FinishTime);
            Assert.AreEqual(viewModel.StageId, result.StageId);
        }
    }
}
