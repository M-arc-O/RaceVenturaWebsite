using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using System;

namespace RaceVentura.Races
{
    public abstract class TeamBaseBL : RaceBaseBL
    {
        public TeamBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        protected Team GetTeam(Guid teamId)
        {
            var team = _UnitOfWork.TeamRepository.GetByID(teamId);
            if (team == null)
            {
                throw new BusinessException($"Team with ID '{teamId}' is unknown", BLErrorCodes.NotFound);
            }

            return team;
        }
    }
}
