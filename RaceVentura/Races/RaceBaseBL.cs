using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace RaceVentura.Races
{
    public abstract class RaceBaseBL
    {
        protected readonly IRaceVenturaUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;

        public RaceBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected UserLink GetRaceUserLink(Guid userId, Guid raceId)
        {
            return _unitOfWork.UserLinkRepository.Get(link => link.UserId == userId && link.RaceId == raceId).FirstOrDefault();
        }

        protected UserLink CheckUserIsAuthorizedForRace(Guid userId, Guid raceId, RaceAccessLevel minimumAccessLevel)
        {
            var userLink = GetRaceUserLink(userId, raceId);
            if (userLink == null || (int)userLink.RaceAccess > (int)minimumAccessLevel)
            {
                _logger.LogWarning($"Error in {GetType().Name}: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");
                throw new BusinessException($"User is not authorized for race.", BLErrorCodes.UserUnauthorized);
            }

            return userLink;
        }

        protected void CheckIfRaceExsists(Guid userId, Guid raceId)
        {
            if (_unitOfWork.RaceRepository.GetByID(raceId) == null)
            {
                _logger.LogError($"Error in {GetType().Name}: User with ID '{userId}' tried to access race with ID '{raceId}' but it does not exsist.");
                throw new BusinessException($"Race with ID '{raceId}' does not exsist.", BLErrorCodes.NotFound);
            }
        }
    }
}
