using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.Races
{
    [TestClass]
    public class PointViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<PointViewModel, RequiredNotEmptyAttribute>("StageId");
            TestUtils.TestAttributeOnProperty<PointViewModel, RequiredAttribute>("Name");
            TestUtils.TestStringLengthAttributeOnProperty<PointViewModel>("Name", 50);
            TestUtils.TestAttributeOnProperty<PointViewModel, RequiredAttribute>("Type");
            TestUtils.TestAttributeOnProperty<PointViewModel, RequiredAttribute>("Value");
            TestUtils.TestAttributeOnProperty<PointViewModel, RequiredAttribute>("Latitude");
            TestUtils.TestAttributeOnProperty<PointViewModel, RequiredAttribute>("Longitude");
        }
    }
}
