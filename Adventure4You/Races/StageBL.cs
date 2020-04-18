using System;
using System.Linq;
using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;

namespace Adventure4You.Races
{
    public class StageBL : StageBaseBL, IGenericBL<Stage>
    {
        public StageBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Stage entity)
        {
            CheckUserIsAuthorizedForRace(userId, entity.RaceId);

            _UnitOfWork.StageRepository.Insert(entity);
            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, Stage newEntity)
        {
            CheckIfRaceExsists(userId, newEntity.RaceId);
            CheckUserIsAuthorizedForRace(userId, newEntity.RaceId);

            var stage = GetStage(newEntity.StageId);

            if (stage.RaceId != newEntity.RaceId)
            {
                _Logger.LogWarning($"Error in {typeof(StageBL)}: User with ID '{userId}' tried to edit stage with ID '{newEntity.StageId}' but the race ID '{newEntity.RaceId}' of this entity does not match the race ID '{stage.RaceId}' of the stage in the database.");
                throw new BusinessException($"Stage with ID '{newEntity.StageId}' is not part of race with ID '{newEntity.RaceId}'", BLErrorCodes.Unknown);
            }

            if (newEntity.Number != stage.Number)
            {
                CheckIfStageNumberExists(newEntity);
            }

            stage.Name = newEntity.Name;
            stage.Number = newEntity.Number;
            stage.MimimumPointsToCompleteStage = newEntity.MimimumPointsToCompleteStage;

            _UnitOfWork.StageRepository.Update(stage);
            _UnitOfWork.SaveAsync();
        }

        public void Delete(Guid userId, Guid entityId)
        {
            var stage = GetStage(entityId);
            CheckIfRaceExsists(userId, stage.RaceId);
            CheckUserIsAuthorizedForRace(userId, stage.RaceId);

            _UnitOfWork.StageRepository.Delete(entityId);
            _UnitOfWork.SaveAsync();
        }

        private bool CheckIfStageNumberExists(Stage stage)
        {
            return _UnitOfWork.StageRepository.Get(s => s.RaceId == stage.RaceId).Any(s => s.Number == stage.Number);
        }
    }
}
