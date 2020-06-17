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
    public class VisitedPointMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new VisitedPointMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void ModelToViewModelTest()
        {
            var model = new VisitedPoint
            {
                TeamId = Guid.NewGuid(),
                PointId = Guid.NewGuid(),
                Time = DateTime.Now,
                VisitedPointId = Guid.NewGuid()
            };

            var result = _Sut.Map<VisitedPointViewModel>(model);

            Assert.AreEqual(model.TeamId, result.TeamId);
            Assert.AreEqual(model.PointId, result.PointId);
            Assert.AreEqual(model.Time, result.Time);
            Assert.AreEqual(model.VisitedPointId, result.VisitedPointId);
        }

        [TestMethod]
        public void ViewModelToModelTest()
        {
            var viewModel = new VisitedPointViewModel
            {
                TeamId = Guid.NewGuid(),
                PointId = Guid.NewGuid(),
                Time = DateTime.Now,
                VisitedPointId = Guid.NewGuid()
            };

            var result = _Sut.Map<VisitedPoint>(viewModel);

            Assert.AreEqual(viewModel.TeamId, result.TeamId);
            Assert.AreEqual(viewModel.PointId, result.PointId);
            Assert.AreEqual(viewModel.Time, result.Time);
            Assert.AreEqual(viewModel.VisitedPointId, result.VisitedPointId);
        }
    }
}
