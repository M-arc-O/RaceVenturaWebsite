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
            TestUtils.TestAttributeOnProperty<RegisterStageStartViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterStageStartViewModel, RequiredAttribute>("UniqueId");
            TestUtils.TestAttributeOnProperty<RegisterStageStartViewModel, RequiredNotEmptyAttribute>("StageId");
        }
    }
}
