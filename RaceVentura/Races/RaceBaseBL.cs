﻿using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace RaceVentura.Races
{
    public abstract class RaceBaseBL
    {
        protected readonly IRaceVenturaUnitOfWork _UnitOfWork;
        protected readonly ILogger _Logger;

        public RaceBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger)
        {
            _UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected UserLink GetRaceUserLink(Guid userId, Guid raceId)
        {
            return _UnitOfWork.UserLinkRepository.Get(link => link.UserId == userId && link.RaceId == raceId).FirstOrDefault();
        }

        protected void CheckUserIsAuthorizedForRace(Guid userId, Guid raceId)
        {
            if (GetRaceUserLink(userId, raceId) == null)
            {
                _Logger.LogWarning($"Error in {GetType().Name}: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");
                throw new BusinessException($"User not authorized for race.", BLErrorCodes.UserUnauthorized);
            }
        }

        protected void CheckIfRaceExsists(Guid userId, Guid raceId)
        {
            if (_UnitOfWork.RaceRepository.GetByID(raceId) == null)
            {
                _Logger.LogError($"Error in {GetType().Name}: User with ID '{userId}' tried to access race with ID '{raceId}' but it does not exsist.");
                throw new BusinessException($"Race with ID '{raceId}' does not exsist.", BLErrorCodes.NotFound);
            }
        }
    }
}