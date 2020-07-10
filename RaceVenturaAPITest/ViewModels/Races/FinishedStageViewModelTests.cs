using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RaceVenturaAPITest.ViewModels.Races
{
    [TestClass]
    public class FinishedStageViewModelTests
    {
        [TestMethod]
        public void AttributeTest()
        {
            TestUtils.TestAttributeOnProperty<FinishedStageViewModel, RequiredNotEmptyAttribute>("StageId");
            TestUtils.TestAttributeOnProperty<FinishedStageViewModel, RequiredNotEmptyAttribute>("TeamId");
        }
    }
}
