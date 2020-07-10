using RaceVentura;
using RaceVenturaTestUtils;
using RaceVentura.Models;
using RaceVentura.Races;
using RaceVenturaAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace RaceVenturaAPITest.Controllers.Races
{
    public abstract class ApiControllerCrudTestsBase<ControllerType, ModelType, GetViewModel, ViewModelType> : ApiControllerCudTestsBase<ControllerType, ModelType, ViewModelType>
    {
        protected new ICrudController<GetViewModel, ViewModelType> Sut { get; set; }
        protected readonly new Mock<IGenericCrudBL<ModelType>> _BLMock = new Mock<IGenericCrudBL<ModelType>>();

        protected virtual void GetNoErrorsTest()
        {
            var retVal = new List<ModelType>();
            for (var i = 0; i < 2; i++)
            {
                var viewModel = (ModelType)Activator.CreateInstance(typeof(ModelType));
                retVal.Add(viewModel);
            }

            _BLMock.Setup(bl => bl.Get(It.IsAny<Guid>())).Returns(retVal);

            var result = Sut.Get();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _MapperMock.Verify(m => m.Map<GetViewModel>(It.IsAny<ModelType>()), Times.Exactly(2));
        }

        protected virtual void GetBusinessErrorTest(Action setupBl)
        {
            setupBl();

            var result = Sut.Get();

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void GetExceptionTest(Action<Exception> setupBl)
        {
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            setupBl(exception);

            var result = Sut.Get();

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(ControllerType)}: {exceptionMessage}");
        }

        protected virtual void GetByIdNoErrorsTest()
        {
            var result = Sut.GetById(Guid.NewGuid());
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Once);
        }

        protected virtual void GetByIdBusinessErrorTest(Action setupBl)
        {
            setupBl();

            var result = Sut.GetById(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void GetByIdExceptionTest(Action<Exception> setupBl)
        {
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            setupBl(exception);

            var result = Sut.GetById(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(ControllerType)}: {exceptionMessage}");
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
