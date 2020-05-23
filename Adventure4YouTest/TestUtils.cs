using Adventure4YouData;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Adventure4YouTest
{
    public static class TestUtils
    {
        public static void SetupUnitOfWorkToPassAuthorizedAndRace(Mock<IAdventure4YouUnitOfWork> unitOfWorkMock, List<UserLink> userLinks, Guid raceId)
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
