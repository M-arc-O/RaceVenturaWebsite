using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Races.MappingProfiles;
using Adventure4YouData.Models.Races;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adventure4YouAPITest.ViewModels.Races.MappingProfiles
{
    [TestClass]
    public class PointMappingProfilesTests
    {
        private IMapper _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PointMappingProfile());
            });

            _Sut = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public void ModelToViewModelTest()
        {
            var model = new Point
            {
                Answer = "a",
                Latitude = 1.2,
                Longitude = 1.1,
                Message = "b",
                Name = "c",
                PointId = Guid.NewGuid(),
                StageId = Guid.NewGuid(),
                Type = PointType.CheckPoint,
                Value = 10
            };

            var result = _Sut.Map<PointViewModel>(model);

            Assert.AreEqual(model.Answer, result.Answer);
            Assert.AreEqual(model.Latitude, result.Latitude);
            Assert.AreEqual(model.Longitude, result.Longitude);
            Assert.AreEqual(model.Message, result.Message);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.PointId, result.PointId);
            Assert.AreEqual(model.StageId, result.StageId);
            Assert.AreEqual((int)model.Type, (int)result.Type);
            Assert.AreEqual(model.Value, result.Value);
        }

        [TestMethod]
        public void ViewModelToModelTest()
        {
            var viewModel = new PointViewModel
            {
                Answer = "a",
                Latitude = 1.2,
                Longitude = 1.1,
                Message = "b",
                Name = "c",
                PointId = Guid.NewGuid(),
                StageId = Guid.NewGuid(),
                Type = PointTypeViewModel.CheckPoint,
                Value = 10
            };

            var result = _Sut.Map<Point>(viewModel);

            Assert.AreEqual(viewModel.Answer, result.Answer);
            Assert.AreEqual(viewModel.Latitude, result.Latitude);
            Assert.AreEqual(viewModel.Longitude, result.Longitude);
            Assert.AreEqual(viewModel.Message, result.Message);
            Assert.AreEqual(viewModel.Name, result.Name);
            Assert.AreEqual(viewModel.PointId, result.PointId);
            Assert.AreEqual(viewModel.StageId, result.StageId);
            Assert.AreEqual((int)viewModel.Type, (int)result.Type);
            Assert.AreEqual(viewModel.Value, result.Value);
        }
    }
}
