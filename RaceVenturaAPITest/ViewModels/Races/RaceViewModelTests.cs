using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.Races
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
