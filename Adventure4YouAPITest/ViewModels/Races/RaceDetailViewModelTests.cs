using Adventure4YouAPI.ViewModels.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest.ViewModels.Races
{
    [TestClass]
    public class RaceDetailViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("CoordinatesCheckEnabled");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("SpecialTasksAreStage");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("MaximumTeamSize");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("MinimumPointsToCompleteStage");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("PenaltyPerMinuteLate");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("StartTime");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("EndTime");
        }
    }
}
