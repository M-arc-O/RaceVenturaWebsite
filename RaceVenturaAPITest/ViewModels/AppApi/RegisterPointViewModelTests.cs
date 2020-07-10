using RaceVenturaAPI.ViewModels.AppApi;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.AppApi
{
    [TestClass]
    public class RegisterPointViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("TeamId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredAttribute>("UniqueId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("PointId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredAttribute>("Latitude");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredAttribute>("Longitude");
        }
    }
}
