﻿using RaceVenturaAPI.ViewModels.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace RaceVenturaAPITest.ViewModels.Races
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
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("PointInformationText");
            TestUtils.TestAttributeOnProperty<RaceDetailViewModel, RequiredAttribute>("RaceType");
        }
    }
}
