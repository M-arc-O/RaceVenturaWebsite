using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adventure4YouDataTest
{
    [TestClass]
    public class Adventure4YouDbContextTests
    {
        private Adventure4YouDbContext _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new Adventure4YouDbContext(new DbContextOptions<Adventure4YouDbContext>());
        }

        [TestMethod]
        public void DbContextPropertiesTypesTest()
        {
            Assert.IsInstanceOfType(_Sut.UserLinks, typeof(DbSet<UserLink>));
            Assert.IsInstanceOfType(_Sut.Races, typeof(DbSet<Race>));
        }
    }
}
