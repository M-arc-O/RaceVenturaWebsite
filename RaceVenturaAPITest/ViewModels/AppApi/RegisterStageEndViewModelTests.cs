using RaceVenturaAPI.ViewModels.AppApi;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.AppApi
{
    [TestClass]
    public class RegisterStageEndViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredAttribute>("UniqueId");
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredNotEmptyAttribute>("StageId");
        }
    }
}
