using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.Races
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
