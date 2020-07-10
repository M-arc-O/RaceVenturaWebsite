using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models;
using RaceVenturaData.Models.Races;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RaceVenturaDataTest
{
    [TestClass]
    public class RaceVenturaDbContextTests
    {
        private RaceVenturaDbContext _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new RaceVenturaDbContext(new DbContextOptions<RaceVenturaDbContext>());
        }

        [TestMethod]
        public void DbContextPropertiesTypesTest()
        {
            Assert.IsInstanceOfType(_Sut.UserLinks, typeof(DbSet<UserLink>));
            Assert.IsInstanceOfType(_Sut.Races, typeof(DbSet<Race>));
        }
    }
}
