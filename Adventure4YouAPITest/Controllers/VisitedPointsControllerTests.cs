using Adventure4YouAPI.Controllers.Races;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adventure4YouAPITest.Controllers
{
    [TestClass]
    public class VisitedPointsControllerTests : ApiControllerCudTestsBase<VisitedPointsController, VisitedPoint, VisitedPointViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new VisitedPointsController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(_Sut);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new VisitedPointsController(null, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new VisitedPointsController(_BLMock.Object, null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new VisitedPointsController(_BLMock.Object, _MapperMock.Object, null));
        }

        [TestMethod]
        public void AddVisitedPointNoErrorsTest()
        {
            AddNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void AddVisitedPointInvalidModelStateTest()
        {
            AddInvalidModelStateTest(_Sut);
        }

        [TestMethod]
        public void AddVisitedPointBusinesErrorTest()
        {
            AddBusinessErrorTest(_Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddVisitedPointExceptionTest()
        {
            AddExceptionTest(_Sut, SetupBlAddException);
        }

        [TestMethod]
        public void DeleteVisitedPointNoErrorsTest()
        {
            DeleteNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void DeleteVisitedPointBusinesErrorTest()
        {
            DeleteBusinessErrorTest(_Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteVisitedPointExceptionTest()
        {
            DeleteExceptionTest(_Sut, SetupBlDeleteException);
        }
    }
}
