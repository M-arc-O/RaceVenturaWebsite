using RaceVenturaAPI.Controllers.Races;
using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RaceVenturaAPITest.Controllers.Races
{
    [TestClass]
    public class StagesControllerTests : ApiControllerCudTestsBase<StagesController, Stage, StageViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            Sut = new StagesController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(Sut);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StagesController(null, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new StagesController(_BLMock.Object, null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new StagesController(_BLMock.Object, _MapperMock.Object, null));
        }

        [TestMethod]
        public void AddStageNoErrorsTest()
        {
            AddNoErrorsTest(Sut);
        }

        [TestMethod]
        public void AddStageInvalidModelStateTest()
        {
            AddInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void AddStageBusinesErrorTest()
        {
            AddBusinessErrorTest(Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddStageExceptionTest()
        {
            AddExceptionTest(Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditStageNoErrorsTest()
        {
            EditNoErrorsTest(Sut);
        }

        [TestMethod]
        public void EditStageInvalidModelStateTest()
        {
            EditInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void EditStageBusinesErrorTest()
        {
            EditBusinessErrorTest(Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditStageExceptionTest()
        {
            EditExceptionTest(Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeleteStageNoErrorsTest()
        {
            DeleteNoErrorsTest(Sut);
        }

        [TestMethod]
        public void DeleteStageBusinesErrorTest()
        {
            DeleteBusinessErrorTest(Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteStageExceptionTest()
        {
            DeleteExceptionTest(Sut, SetupBlDeleteException);
        }
    }
}
