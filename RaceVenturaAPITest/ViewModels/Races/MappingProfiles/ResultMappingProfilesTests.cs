using RaceVentura.Models.Results;
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
    public class ResultMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ResultMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void TeamModelToViewModelTest()
        {
            var model = new TeamResult
            {
                EndTime = DateTime.Now,
                NumberOfPoints = 1,
                NumberOfStages = 2,
                StageResults = new List<StageResult>(),
                TeamName = "a",
                TeamNumber = 3,
                TotalValue = 4
            };

            var result = _Sut.Map<TeamResultViewModel>(model);

            Assert.AreEqual(model.EndTime, result.EndTime);
            Assert.AreEqual(model.NumberOfPoints, result.NumberOfPoints);
            Assert.AreEqual(model.NumberOfStages, result.NumberOfStages);
            Assert.IsNotNull(result.StageResults);
            Assert.AreEqual(model.TeamName, result.TeamName);
            Assert.AreEqual(model.TeamNumber, result.TeamNumber);
            Assert.AreEqual(model.TotalValue, result.TotalValue);
        }

        [TestMethod]
        public void StageResultModelToViewModelTest()
        {
            var model = new StageResult
            {
                TotalValue = 4,
                PointResults = new List<PointResult>(),
                StageName = "a",
                StageNumber = 1
            };

            var result = _Sut.Map<StageResultViewModel>(model);

            Assert.AreEqual(model.TotalValue, result.TotalValue);
            Assert.AreEqual(model.StageName, result.StageName);
            Assert.AreEqual(model.StageNumber, result.StageNumber);
            Assert.IsNotNull(result.PointResults);
        }

        [TestMethod]
        public void PointResultModelToViewModelTest()
        {
            var model = new PointResult
            {
                Name = "a",
                Value = 1
            };

            var result = _Sut.Map<PointResultViewModel>(model);

            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Value, result.Value);
        }
    }
}
