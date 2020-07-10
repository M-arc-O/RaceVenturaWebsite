using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.Races
{
    [TestClass]
    public class TeamViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<TeamViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<TeamViewModel, RequiredAttribute>("Number");
            TestUtils.TestAttributeOnProperty<TeamViewModel, RequiredAttribute>("Name");
            TestUtils.TestAttributeOnProperty<TeamViewModel, RequiredAttribute>("Category");
            TestUtils.TestStringLengthAttributeOnProperty<TeamViewModel>("Name", 100);
        }
    }
}
