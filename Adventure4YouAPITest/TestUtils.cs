using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adventure4YouAPITest
{
    public static class TestUtils
    {
        public static void TestAttributeOnProperty<T, TAttribute>(string propertyName)
        {
            var t = typeof(T);
            var pi = t.GetProperty(propertyName);
            Assert.IsTrue(Attribute.IsDefined(pi, typeof(TAttribute)));
        }

        public static void TestStringLengthAttributeOnProperty<T>(string propertyName, int length)
        {
            var t = typeof(T);
            var pi = t.GetProperty(propertyName);
            Assert.IsTrue(Attribute.IsDefined(pi, typeof(StringLengthAttribute)));
            
            var attr = pi.GetCustomAttributes(typeof(StringLengthAttribute), false);
            if (attr.Length > 0)
            {
                Assert.AreEqual((attr[0] as StringLengthAttribute).MaximumLength, length);
            }
        }
    }
}
