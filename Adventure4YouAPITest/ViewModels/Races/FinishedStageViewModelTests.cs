using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adventure4YouAPITest.ViewModels.Races
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
