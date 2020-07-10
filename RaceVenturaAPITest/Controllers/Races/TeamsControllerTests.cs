using RaceVenturaAPI.Controllers.Races;
using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RaceVenturaAPITest.Controllers.Races
{
    [TestClass]
    public class TeamsControllerTests : ApiControllerCudTestsBase<TeamsController, Team, TeamViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            Sut = new TeamsController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(Sut);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TeamsController(null, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new TeamsController(_BLMock.Object, null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new TeamsController(_BLMock.Object, _MapperMock.Object, null));
        }

        [TestMethod]
        public void AddTeamNoErrorsTest()
        {
            AddNoErrorsTest(Sut);
        }

        [TestMethod]
        public void AddTeamInvalidModelStateTest()
        {
            AddInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void AddTeamBusinesErrorTest()
        {
            AddBusinessErrorTest(Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddTeamExceptionTest()
        {
            AddExceptionTest(Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditTeamNoErrorsTest()
        {
            EditNoErrorsTest(Sut);
        }

        [TestMethod]
        public void EditTeamInvalidModelStateTest()
        {
            EditInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void EditTeamBusinesErrorTest()
        {
            EditBusinessErrorTest(Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditTeamExceptionTest()
        {
            EditExceptionTest(Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeleteTeamNoErrorsTest()
        {
            DeleteNoErrorsTest(Sut);
        }

        [TestMethod]
        public void DeleteTeamBusinesErrorTest()
        {
            DeleteBusinessErrorTest(Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteTeamExceptionTest()
        {
            DeleteExceptionTest(Sut, SetupBlDeleteException);
        }
    }
}
