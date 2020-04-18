using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using System;

namespace Adventure4You.Races
{
    public abstract class TeamBaseBL : RaceBaseBL
    {
        public TeamBaseBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        protected Team GetTeam(Guid teamId)
        {
            var team = _UnitOfWork.TeamRepository.GetByID(teamId);
            if (team == null)
            {
                throw new BusinessException($"Stage with ID '{teamId}' is unknown", BLErrorCodes.Unknown);
            }

            return team;
        }
    }
}
