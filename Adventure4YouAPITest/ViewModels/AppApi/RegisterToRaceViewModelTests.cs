using Adventure4YouAPI.ViewModels.AppApi;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adventure4YouAPITest.ViewModels.AppApi
{
    [TestClass]
    public class RegisterToRaceViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RegisterToRaceViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<RegisterToRaceViewModel, RequiredNotEmptyAttribute>("TeamId");
            TestUtils.TestAttributeOnProperty<RegisterToRaceViewModel, RequiredNotEmptyAttribute>("UniqueId");
        }
    }
}
