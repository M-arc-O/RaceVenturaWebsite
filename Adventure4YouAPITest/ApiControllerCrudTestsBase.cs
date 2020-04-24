using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Races;
using Adventure4YouAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Adventure4YouAPITest
{
    public abstract class ApiControllerCrudTestsBase<ControllerType, ModelType, GetViewModel, ViewModelType> : ApiControllerCudTestsBase<ControllerType, ModelType, ViewModelType>
    {

        protected readonly new Mock<IGenericCrudBL<ModelType>> _BLMock = new Mock<IGenericCrudBL<ModelType>>();
        protected new ICrudController<GetViewModel, ViewModelType> _Sut { get; set; }

        protected virtual void GetNoErrorsTest()
        {
            var retVal = new List<ModelType>();
            for (var i = 0; i < 2; i++)
            {
                var viewModel = (ModelType)Activator.CreateInstance(typeof(ModelType));
                retVal.Add(viewModel);
            }

            _BLMock.Setup(bl => bl.Get(It.IsAny<Guid>())).Returns(retVal);

            var result = _Sut.Get();
            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<GetViewModel>>));
            _MapperMock.Verify(m => m.Map<GetViewModel>(It.IsAny<ModelType>()), Times.Exactly(2));
        }

        protected virtual void GetBusinessErrorTest(Action setupBl)
        {
            setupBl();

            var result = _Sut.Get();

            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<GetViewModel>>));
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result.Result as BadRequestObjectResult).Value);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void GetExceptionTest(Action<Exception> setupBl)
        {
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            setupBl(exception);

            var result = _Sut.Get();

            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<GetViewModel>>));
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result.Result as StatusCodeResult).StatusCode);

            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(ControllerType)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        protected virtual void GetByIdNoErrorsTest()
        {
            var result = _Sut.GetById(Guid.NewGuid());
            Assert.IsInstanceOfType(result, typeof(ActionResult<ViewModelType>));
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Once);
        }

        protected virtual void GetByIdBusinessErrorTest(Action setupBl)
        {
            setupBl();

            var result = _Sut.GetById(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(ActionResult<ViewModelType>));
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result.Result as BadRequestObjectResult).Value);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void GetByIdExceptionTest(Action<Exception> setupBl)
        {
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            setupBl(exception);

            var result = _Sut.GetById(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(ActionResult<ViewModelType>));
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result.Result as StatusCodeResult).StatusCode);

            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(ControllerType)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        protected virtual void SetupBlGetBusinessException()
        {
            _BLMock.Setup(bl => bl.Get(It.IsAny<Guid>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected virtual void SetupBlGetException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Get(It.IsAny<Guid>())).Throws(exception);
        }

        protected virtual void SetupBlGetByIdBusinessException()
        {
            _BLMock.Setup(bl => bl.GetById(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected virtual void SetupBlGetByIdException(Exception exception)
        {
            _BLMock.Setup(bl => bl.GetById(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);
        }

        protected override void SetupBlAddBusinessException()
        {
            _BLMock.Setup(bl => bl.Add(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected override void SetupBlAddException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Add(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(exception);
        }

        protected override void SetupBlEditBusinessException()
        {
            _BLMock.Setup(bl => bl.Edit(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected override void SetupBlEditException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Edit(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(exception);
        }

        protected override void SetupBlDeleteBusinessException()
        {
            _BLMock.Setup(bl => bl.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected override void SetupBlDeleteException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);
        }
    }
}
