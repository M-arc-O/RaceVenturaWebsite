using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using System;

namespace RaceVentura.Races
{
    public abstract class StageBaseBL: RaceBaseBL
    {
        public StageBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger) : base (unitOfWork, logger)
        {

        }

        protected Stage GetStage(Guid stageId)
        {
            var stage = _UnitOfWork.StageRepository.GetByID(stageId);
            if (stage == null)
            {
                throw new BusinessException($"Stage with ID '{stageId}' is unknown", BLErrorCodes.NotFound);
            }

            return stage;
        }

        protected Stage GetAndCheckStage(Guid userId, Guid stageId)
        {
            var stage = GetStage(stageId);
            CheckIfRaceExsists(userId, stage.RaceId);
            CheckUserIsAuthorizedForRace(userId, stage.RaceId);
            return stage;
        }
    }
}
