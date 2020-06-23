using Adventure4YouAPI.ViewModels.AppApi;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.AppApi
{
    [TestClass]
    public class RegisterStageEndViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredNotEmptyAttribute>("TeamId");
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredAttribute>("UniqueId");
            TestUtils.TestAttributeOnProperty<RegisterStageEndViewModel, RequiredNotEmptyAttribute>("StageId");
        }
    }
}
