using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.Races
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
            TestUtils.TestStringLengthAttributeOnProperty<TeamViewModel>("Name", 100);
        }
    }
}
