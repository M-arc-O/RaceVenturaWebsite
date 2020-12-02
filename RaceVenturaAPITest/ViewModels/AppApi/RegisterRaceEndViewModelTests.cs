using RaceVenturaAPI.ViewModels.AppApi;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.AppApi
{
    [TestClass]
    public class RegisterRaceEndViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RegisterRaceEndViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterRaceEndViewModel, RequiredAttribute>("UniqueId");
        }
    }
}
