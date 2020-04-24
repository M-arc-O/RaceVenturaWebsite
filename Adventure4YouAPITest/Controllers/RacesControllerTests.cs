using Adventure4YouAPI.Controllers;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adventure4YouAPITest.Controllers
{
    [TestClass]
    public class RacesControllerTests : ApiControllerCrudTestsBase<RacesController, Race, RaceViewModel, RaceDetailViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new RacesController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(_Sut);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(null, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, _MapperMock.Object, null));
        }

        [TestMethod]
        public void GetRaceNoErrorsTest()
        {
            GetNoErrorsTest();
        }

        [TestMethod]
        public void GetRaceBusinesErrorTest()
        {
            GetBusinessErrorTest(SetupBlGetBusinessException);
        }

        [TestMethod]
        public void GetRaceExceptionTest()
        {
            GetExceptionTest(SetupBlGetException);
        }

        [TestMethod]
        public void GetByIdRaceNoErrorsTest()
        {
            GetByIdNoErrorsTest();
        }

        [TestMethod]
        public void GetByIdRaceBusinesErrorTest()
        {
            GetByIdBusinessErrorTest(SetupBlGetByIdBusinessException);
        }

        [TestMethod]
        public void GetByIdRaceExceptionTest()
        {
            GetByIdExceptionTest(SetupBlGetByIdException);
        }

        [TestMethod]
        public void AddRaceNoErrorsTest()
        {
            AddNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void AddRaceInvalidModelStateTest()
        {
            AddInvalidModelStateTest(_Sut);
        }

        [TestMethod]
        public void AddRaceBusinesErrorTest()
        {
            AddBusinessErrorTest(_Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddRaceExceptionTest()
        {
            AddExceptionTest(_Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditRaceNoErrorsTest()
        {
            EditNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void EditRaceInvalidModelStateTest()
        {
            EditInvalidModelStateTest(_Sut);
        }

        [TestMethod]
        public void EditRaceBusinesErrorTest()
        {
            EditBusinessErrorTest(_Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditRaceExceptionTest()
        {
            EditExceptionTest(_Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeleteRaceNoErrorsTest()
        {
            DeleteNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void DeleteRaceBusinesErrorTest()
        {
            DeleteBusinessErrorTest(_Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteRaceExceptionTest()
        {
            DeleteExceptionTest(_Sut, SetupBlDeleteException);
        }
    }
}
