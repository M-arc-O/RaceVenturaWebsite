using Adventure4YouAPI.Controllers;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adventure4YouAPITest
{
    [TestClass]
    public class StagesControllerTests : ApiControllerCudTestsBase<StagesController, Stage, StageViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new StagesController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(_Sut);
        }

        [TestMethod]
        public void AddStageNoErrorsTest()
        {
            AddNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void AddStageBusinesErrorTest()
        {
            AddBusinessErrorTest(_Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddStageExceptionTest()
        {
            AddExceptionTest(_Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditStageNoErrorsTest()
        {
            EditNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void EditStageBusinesErrorTest()
        {
            EditBusinessErrorTest(_Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditStageExceptionTest()
        {
            EditExceptionTest(_Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeleteStageNoErrorsTest()
        {
            DeleteNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void DeleteStageBusinesErrorTest()
        {
            DeleteBusinessErrorTest(_Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteStageExceptionTest()
        {
            DeleteExceptionTest(_Sut, SetupBlDeleteException);
        }
    }
}
