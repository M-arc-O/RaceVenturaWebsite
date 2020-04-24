using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.Races
{
    [TestClass]
    public class RaceViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RaceViewModel, RequiredAttribute>("Name");
            TestUtils.TestStringLengthAttributeOnProperty<RaceViewModel>("Name", 100);
        }
    }
}
