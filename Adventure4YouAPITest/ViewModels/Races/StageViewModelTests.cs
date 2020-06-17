using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.Races
{
    [TestClass]
    public class StageViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<StageViewModel, RequiredNotEmptyAttribute>("RaceId");
            TestUtils.TestAttributeOnProperty<StageViewModel, RequiredAttribute>("Number");
            TestUtils.TestAttributeOnProperty<StageViewModel, RequiredAttribute>("Name");
            TestUtils.TestStringLengthAttributeOnProperty<StageViewModel>("Name", 50);
        }
    }
}
