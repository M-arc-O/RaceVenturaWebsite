using Adventure4YouAPI.Controllers;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adventure4YouAPITest
{
    [TestClass]
    public class PointsControllerTests : ApiControllerCudTestsBase<PointsController, Point, PointViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new PointsController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(_Sut);
        }

        [TestMethod]
        public void AddTeamNoErrorsTest()
        {
            AddNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void AddTeamBusinesErrorTest()
        {
            AddBusinessErrorTest(_Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddTeamExceptionTest()
        {
            AddExceptionTest(_Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditTeamNoErrorsTest()
        {
            EditNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void EditTeamBusinesErrorTest()
        {
            EditBusinessErrorTest(_Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditTeamExceptionTest()
        {
            EditExceptionTest(_Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeleteTeamNoErrorsTest()
        {
            DeleteNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void DeleteTeamBusinesErrorTest()
        {
            DeleteBusinessErrorTest(_Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteTeamExceptionTest()
        {
            DeleteExceptionTest(_Sut, SetupBlDeleteException);
        }
    }
}
