using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure4YouDataTest
{
    [TestClass]
    public class GenericRepositoryTests
    {
        private Guid dbId = Guid.NewGuid();
        private List<Guid> _Ids = new List<Guid>();
        private IAdventure4YouDbContext _DbContext;
        private GenericRepository<Race> _Sut;

        private const int arrayLength = 10;

        [TestInitialize]
        public void InitializeTest()
        {
            CreateSut();
            LoadDataIntoDatabase();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _DbContext.Dispose();
        }

        [TestMethod]
        public void GetWithoutParametersTest()
        {
            CreateSut();

            var results = _Sut.Get().ToList();

            for (int i = 0; i < results.Count; i++)
            {
                Assert.AreEqual($"Name{i}", results[i].Name);
                Assert.IsNull(results[i].Teams);
            }
        }

        [TestMethod]
        public void GetWithSingleLevelIncludesTest()
        {
            CreateSut();

            var results = _Sut.Get(null, null, "Teams,Stages").ToList();

            for (int i = 0; i < results.Count; i++)
            {
                Assert.AreEqual(1, results[i].Teams.Count);
                Assert.AreEqual("TestTeam", results[i].Teams[0].Name);
                Assert.IsNull(results[i].Teams[0].VisitedPoints);
                Assert.AreEqual(1, results[i].Stages.Count);
                Assert.AreEqual("TestStage", results[i].Stages[0].Name);
            }
        }

        [TestMethod]
        public void GetWithMultiLevelIncludesTest()
        {
            CreateSut();

            var results = _Sut.Get(null, null, "Teams.VisitedPoints").ToList();

            for (int i = 0; i < results.Count; i++)
            {
                Assert.AreEqual(1, results[i].Teams.Count);
                Assert.AreEqual("TestTeam", results[i].Teams[0].Name);
                Assert.AreEqual(1, results[i].Teams[0].VisitedPoints.Count);
                Assert.AreEqual(new DateTime(2020, 1, 1), results[i].Teams[0].VisitedPoints[0].Time);
            }
        }

        [TestMethod]
        public void GetWithMultiLevelIncludesAndOrderTest()
        {
            CreateSut();

            var results = _Sut.Get(null, x => x.OrderBy(e => e.Name), "Teams.VisitedPoints,Stages").ToList();

            for (int i = 0; i < results.Count; i++)
            {
                Assert.AreEqual($"Name{i}", results[i].Name);
                Assert.AreEqual(1, results[i].Teams.Count);
                Assert.AreEqual("TestTeam", results[i].Teams[0].Name);
                Assert.AreEqual(1, results[i].Teams[0].VisitedPoints.Count);
                Assert.AreEqual(new DateTime(2020, 1, 1), results[i].Teams[0].VisitedPoints[0].Time);
                Assert.AreEqual(1, results[i].Stages.Count);
                Assert.AreEqual("TestStage", results[i].Stages[0].Name);
            }
        }

        [TestMethod]
        public void GetWithFilterTest()
        {
            var results = _Sut.Get(x => x.MaximumTeamSize == 1).ToList();

            Assert.IsInstanceOfType(results, typeof(List<Race>));
            Assert.AreEqual(arrayLength / 2, results.Count);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            for (int i = 0; i < arrayLength; i++)
            {
                var result = _Sut.GetByID(_Ids[i]);

                Assert.IsInstanceOfType(result, typeof(Race));
                Assert.AreEqual($"Name{i}", result.Name);
            }
        }

        [TestMethod]
        public async Task DeleteByIdDetachedTest()
        {
            var entity = _DbContext.Set<Race>().Find(_Ids[0]);
            _DbContext.Entry(entity).State = EntityState.Detached;
            _DbContext.SaveChanges();

            _Sut.Delete(entity);
            _DbContext.SaveChanges();

            Assert.IsFalse(await _DbContext.Races.AnyAsync(r => r.RaceId == _Ids[0]));
        }

        [TestMethod]
        public async Task DeleteByIdAttachedTest()
        {
            _Sut.Delete(_Ids[0]);
            _DbContext.SaveChanges();

            Assert.IsFalse(await _DbContext.Races.AnyAsync(r => r.RaceId == _Ids[0]));
        }

        [TestMethod]
        public void UpdateTest()
        {
            var testName = "name";

            var race = new Race
            {
                RaceId = _Ids[0],
                Name = testName
            };

            CreateSut();

            _Sut.Update(race);
            _DbContext.SaveChanges();

            Assert.AreEqual(_DbContext.Races.Find(_Ids[0]).Name, testName);
        }

        private void CreateSut()
        {
            _DbContext = InMemoryContext(dbId.ToString());
            _Sut = new GenericRepository<Race>(_DbContext);
        }

        private void LoadDataIntoDatabase()
        {
            for (int i = 0; i < arrayLength; i++)
            {
                var id = Guid.NewGuid();
                _Ids.Add(id);

                var race = new Race
                {
                    RaceId = id,
                    Name = $"Name{i}",
                    Teams = new List<Team>
                    {
                        new Team
                        {
                            Name = "TestTeam",
                            VisitedPoints = new List<VisitedPoint>
                            {
                                new VisitedPoint
                                {
                                    Time = new DateTime(2020, 1, 1)
                                }
                            }
                        }
                    },
                    Stages = new List<Stage>
                    {
                        new Stage
                        {
                            Name = "TestStage"
                        }
                    }
                };

                if (i < arrayLength / 2)
                {
                    race.MaximumTeamSize = 1;
                }
                else
                {
                    race.MaximumTeamSize = 2;
                }

                _Sut.Insert(race);
            }

            _DbContext.SaveChanges();
        }

        private IAdventure4YouDbContext InMemoryContext(string connection)
        {
            var options = new DbContextOptionsBuilder<Adventure4YouDbContext>()
                .UseInMemoryDatabase(connection)
                .EnableSensitiveDataLogging()
                .Options;
            var context = new Adventure4YouDbContext(options);

            return context;
        }
    }
}
