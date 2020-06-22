using Adventure4YouAPI.ViewModels.AppApi;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.AppApi
{
    [TestClass]
    public class RegisterPointViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("TeamId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("UniqueId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredNotEmptyAttribute>("PointId");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredAttribute>("Latitude");
            TestUtils.TestAttributeOnProperty<RegisterPointViewModel, RequiredAttribute>("Longitude");
        }
    }
}
