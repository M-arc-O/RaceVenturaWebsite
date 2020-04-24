using Adventure4YouAPI.ViewModels.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adventure4YouAPITest.ViewModels.Validators
{
    [TestClass]
    public class RequiredNotEmptyAttributeTests
    {
        private RequiredNotEmptyAttribute _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new RequiredNotEmptyAttribute();
        }

        [TestMethod]
        public void IsValidWithNullTest()
        {
            Assert.IsFalse(_Sut.IsValid(null));
        }

        [TestMethod]
        public void IsValidDefault()
        {
            Assert.IsTrue(_Sut.IsValid(1));
        }

        [TestMethod]
        public void IsValidWithValidGuid()
        {
            Assert.IsTrue(_Sut.IsValid(Guid.NewGuid()));
        }

        [TestMethod]
        public void IsValidWithEmptyGuid()
        {
            Assert.IsFalse(_Sut.IsValid(Guid.Empty));
        }
    }
}
