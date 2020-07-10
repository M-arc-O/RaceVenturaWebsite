using RaceVenturaData;
using RaceVenturaData.Models;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace RaceVenturaTest
{
    public static class TestUtils
    {
        public static void SetupUnitOfWorkToPassAuthorizedAndRace(Mock<IRaceVenturaUnitOfWork> unitOfWorkMock, List<UserLink> userLinks, Guid raceId)
        {
            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(userLinks);

            unitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId)))).Returns(new Race { RaceId = raceId });

            unitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);
        }
    }
}
