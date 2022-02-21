using RaceVentura;
using RaceVenturaTestUtils;
using RaceVentura.Models;
using RaceVentura.Races;
using RaceVenturaAPI.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace RaceVenturaAPITest.Controllers.Races
{
    public abstract class ApiControllerCudTestsBase<ControllerType, ModelType, ViewModelType>
    {
        protected readonly Mock<IGenericCudBL<ModelType>> _BLMock = new();
        protected readonly Mock<IMapper> _MapperMock = new();
        protected readonly Mock<ILogger<ControllerType>> _LoggerMock = new();
        protected ICudController<ViewModelType> Sut;

        protected virtual void AddNoErrorsTest(ICudController<ViewModelType> sut)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));

            var result = sut.Add(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Once);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Once);
        }

        protected virtual void AddInvalidModelStateTest(ICudController<ViewModelType> sut)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));
            sut.ModelState.AddModelError("test", "test");

            var result = sut.Add(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Never);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void AddBusinessErrorTest(ICudController<ViewModelType> sut, Action setupBl)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));
            setupBl();

            var result = sut.Add(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Once);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void AddExceptionTest(ICudController<ViewModelType> sut, Action<Exception> setupBl)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            setupBl(exception);

            var result = sut.Add(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Once);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(ControllerType)}: {exceptionMessage}");
        }
        
        protected virtual void EditNoErrorsTest(ICudController<ViewModelType> sut)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));

            var result = sut.Edit(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Once);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Once);
        }

        protected virtual void EditInvalidModelStateTest(ICudController<ViewModelType> sut)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));
            sut.ModelState.AddModelError("test", "test");

            var result = sut.Edit(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Never);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void EditBusinessErrorTest(ICudController<ViewModelType> sut, Action setupBl)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));

            setupBl();

            var result = sut.Edit(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Once);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
        }

        protected virtual void EditExceptionTest(ICudController<ViewModelType> sut, Action<Exception> setupBl)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            setupBl(exception);

            var result = sut.Edit(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _MapperMock.Verify(m => m.Map<ModelType>(It.IsAny<ViewModelType>()), Times.Once);
            _MapperMock.Verify(m => m.Map<ViewModelType>(It.IsAny<ModelType>()), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(ControllerType)}: {exceptionMessage}");
        }

        protected virtual void DeleteNoErrorsTest(ICudController<ViewModelType> sut)
        {
            var result = sut.Delete(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        protected virtual void DeleteBusinessErrorTest(ICudController<ViewModelType> sut, Action setupBl)
        {
            setupBl();

            var result = sut.Delete(Guid.NewGuid());
;
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        protected virtual void DeleteExceptionTest(ICudController<ViewModelType> sut, Action<Exception> setupBl)
        {
            var viewModel = (ViewModelType)Activator.CreateInstance(typeof(ViewModelType));
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);
            setupBl(exception);

            var result = sut.Delete(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(ControllerType)}: {exceptionMessage}");
        }

        protected void SetControllerContext(ICudController<ViewModelType> sut)
        {
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity("Username", "Token"), new[]
            {
                new Claim(RaceVenturaAPI.Helpers.Constants.Strings.JwtClaimIdentifiers.Id, Guid.NewGuid().ToString()),
                new Claim(RaceVenturaAPI.Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, RaceVenturaAPI.Helpers.Constants.Strings.JwtClaims.ApiAccess)
            })));

            sut.ControllerContext.HttpContext = contextMock.Object;

            Assert.IsNotNull(sut.HttpContext.User);
        }

        protected virtual void SetupBlAddBusinessException()
        {
            _BLMock.Setup(bl => bl.Add(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected virtual void SetupBlAddException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Add(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(exception);
        }

        protected virtual void SetupBlEditBusinessException()
        {
            _BLMock.Setup(bl => bl.Edit(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected virtual void SetupBlEditException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Edit(It.IsAny<Guid>(), It.IsAny<ModelType>())).Throws(exception);
        }

        protected virtual void SetupBlDeleteBusinessException()
        {
            _BLMock.Setup(bl => bl.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));
        }

        protected virtual void SetupBlDeleteException(Exception exception)
        {
            _BLMock.Setup(bl => bl.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);
        }
    }
}
