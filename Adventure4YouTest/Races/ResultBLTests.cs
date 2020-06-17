using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Races;
using Adventure4YouData;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Adventure4YouTest.Races
{
    [TestClass]
    public class ResultBLTests
    {
        private readonly Mock<ILogger<ResultsBL>> _LoggerMock = new Mock<ILogger<ResultsBL>>();
        private readonly Mock<IAdventure4YouUnitOfWork> _UnitOfWorkMock = new Mock<IAdventure4YouUnitOfWork>();
        private ResultsBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new ResultsBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void GetRaceResult()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var stageId1 = Guid.NewGuid();
            var stageId2 = Guid.NewGuid();

            var pointId1 = Guid.NewGuid();
            var pointId2 = Guid.NewGuid();
            var pointId3 = Guid.NewGuid();

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { new UserLink
                {
                    UserId = userId,
                    RaceId = raceId
                }});
            _UnitOfWorkMock.Setup(m => m.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.Is<string>(s => s.Equals("Teams,Teams.VisitedPoints,Stages,Stages.Points")))).Returns(new List<Race> { new Race
                {
                    EndTime = new DateTime(2020, 1, 1, 18, 0, 0),
                    PenaltyPerMinuteLate = 5,
                    MinimumPointsToCompleteStage = 1,
                    Teams = new List<Team>
                    {
                        new Team
                        {
                            Name = "TweedeOpTijd",
                            Number = 1,
                            FinishTime = new DateTime(2020, 1, 1, 17, 0, 0),
                            VisitedPoints = new List<VisitedPoint>
                            {
                                new VisitedPoint { PointId = pointId1 },
                                new VisitedPoint { PointId = pointId2 },
                                new VisitedPoint { PointId = pointId3 }
                            }
                        },
                        new Team
                        {
                            Name = "Eerste",
                            Number = 2,
                            FinishTime = new DateTime(2020, 1, 1, 16, 59, 0),
                            VisitedPoints = new List<VisitedPoint>
                            {
                                new VisitedPoint { PointId = pointId1 },
                                new VisitedPoint { PointId = pointId2 },
                                new VisitedPoint { PointId = pointId3 }
                            }
                        },
                        new Team
                        {
                            Name = "EtappeMinder",
                            Number = 3,
                            FinishTime = new DateTime(2020, 1, 1, 18, 0, 0),
                            VisitedPoints = new List<VisitedPoint>
                            {
                                new VisitedPoint { PointId = pointId1 },
                                new VisitedPoint { PointId = pointId2 }
                            }
                        },
                        new Team
                        {
                            Name = "TeLaat",
                            Number = 4,
                            FinishTime = new DateTime(2020, 1, 1, 18, 1, 0),
                            VisitedPoints = new List<VisitedPoint>
                            {
                                new VisitedPoint { PointId = pointId1 },
                                new VisitedPoint { PointId = pointId2 },
                                new VisitedPoint { PointId = pointId3 }
                            }
                        }
                    },
                    Stages = new List<Stage>
                    {
                        new Stage
                        {
                            Name = "Stage1",
                            Number = 1,
                            Points = new List<Point>
                            {
                                new Point { PointId = pointId1, Name = "PointStage1", Value = 10 }
                            }
                        },
                        new Stage
                        {
                            Name = "Stage2",
                            Number = 2,
                            MimimumPointsToCompleteStage = 2,
                            Points = new List<Point>
                            {
                                new Point { PointId = pointId2, Name = "Point1Stage2", Value = 20 },
                                new Point { PointId = pointId3, Name = "Point2Stage2", Value = 30 }
                            }
                        }
                    }
                }});
            _UnitOfWorkMock.Setup(m => m.RaceRepository).Returns(raceRepositoryMock.Object);

            var result = _Sut.GetRaceResults(userId, raceId).ToList();

            Assert.AreEqual("Eerste", result[0].TeamName);
            Assert.AreEqual(2, result[0].TeamNumber);
            Assert.AreEqual(2, result[0].NumberOfStages);
            Assert.AreEqual(3, result[0].NumberOfPoints);
            Assert.AreEqual(60, result[0].TotalValue);
            Assert.AreEqual(new DateTime(2020, 1, 1, 16, 59, 0), result[0].EndTime);
            Assert.AreEqual(2, result[0].StageResults.Count);
            Assert.AreEqual("Stage1", result[0].StageResults[0].StageName);
            Assert.AreEqual(1, result[0].StageResults[0].StageNumber);
            Assert.AreEqual(1, result[0].StageResults[0].NumberOfPoints);
            Assert.AreEqual(10, result[0].StageResults[0].TotalValue);
            Assert.AreEqual(1, result[0].StageResults[0].PointResults.Count);
            Assert.AreEqual("PointStage1", result[0].StageResults[0].PointResults[0].Name);
            Assert.AreEqual(10, result[0].StageResults[0].PointResults[0].Value);
            Assert.AreEqual("Stage2", result[0].StageResults[1].StageName);
            Assert.AreEqual(2, result[0].StageResults[1].StageNumber);
            Assert.AreEqual(2, result[0].StageResults[1].NumberOfPoints);
            Assert.AreEqual(50, result[0].StageResults[1].TotalValue);
            Assert.AreEqual("Point1Stage2", result[0].StageResults[1].PointResults[0].Name);
            Assert.AreEqual(20, result[0].StageResults[1].PointResults[0].Value);
            Assert.AreEqual("Point2Stage2", result[0].StageResults[1].PointResults[1].Name);
            Assert.AreEqual(30, result[0].StageResults[1].PointResults[1].Value);

            Assert.AreEqual("TweedeOpTijd", result[1].TeamName);
            Assert.AreEqual(1, result[1].TeamNumber);
            Assert.AreEqual(2, result[1].NumberOfStages);
            Assert.AreEqual(3, result[1].NumberOfPoints);
            Assert.AreEqual(60, result[1].TotalValue);
            Assert.AreEqual(new DateTime(2020, 1, 1, 17, 0, 0), result[1].EndTime);
            Assert.AreEqual(2, result[1].StageResults.Count);

            Assert.AreEqual("TeLaat", result[2].TeamName);
            Assert.AreEqual(4, result[2].TeamNumber);
            Assert.AreEqual(2, result[2].NumberOfStages);
            Assert.AreEqual(3, result[2].NumberOfPoints);
            Assert.AreEqual(55, result[2].TotalValue);
            Assert.AreEqual(new DateTime(2020, 1, 1, 18, 1, 0), result[2].EndTime);
            Assert.AreEqual(2, result[2].StageResults.Count);

            Assert.AreEqual("EtappeMinder", result[3].TeamName);
            Assert.AreEqual(3, result[3].TeamNumber);
            Assert.AreEqual(1, result[3].NumberOfStages);
            Assert.AreEqual(1, result[3].NumberOfPoints);
            Assert.AreEqual(10, result[3].TotalValue);
            Assert.AreEqual(new DateTime(2020, 1, 1, 18, 0, 0), result[3].EndTime);
            Assert.AreEqual(2, result[3].StageResults.Count);
        }

        [TestMethod]
        public void GetRaceResultRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(m => m.RaceRepository).Returns(raceRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.GetRaceResults(userId, raceId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Race with ID '{raceId}' not found.", exception.Message);
        }

        [TestMethod]
        public void GetRaceResultNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { });
            _UnitOfWorkMock.Setup(m => m.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.Is<string>(s => s.Equals("Teams,Teams.VisitedPoints,Stages,Stages.Points")))).Returns(new List<Race> { new Race() });
            _UnitOfWorkMock.Setup(m => m.RaceRepository).Returns(raceRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.GetRaceResults(userId, raceId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);
        }
    }
}
