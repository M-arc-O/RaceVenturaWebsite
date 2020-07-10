using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.Races
{
    [TestClass]
    public class VisitedPointViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<VisitedPointViewModel, RequiredNotEmptyAttribute>("PointId");
            TestUtils.TestAttributeOnProperty<VisitedPointViewModel, RequiredNotEmptyAttribute>("TeamId");
        }
    }
}
