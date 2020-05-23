using System;
using System.Linq;
using Adventure4You.Models;
using Adventure4YouData;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;

namespace Adventure4You.Races
{
    public class StageBL : StageBaseBL, IGenericCudBL<Stage>
    {
        public StageBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        public void Add(Guid userId, Stage entity)
        {
            CheckIfRaceExsists(userId, entity.RaceId);
            CheckUserIsAuthorizedForRace(userId, entity.RaceId);
            CheckIfStageNumberExists(entity);
            
            _UnitOfWork.StageRepository.Insert(entity);
            _UnitOfWork.SaveAsync();
        }

        public void Edit(Guid userId, Stage newEntity)
        {
            var stage = GetAndCheckStage(userId, newEntity.StageId);

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
            GetAndCheckStage(userId, entityId);

            _UnitOfWork.StageRepository.Delete(entityId);
            _UnitOfWork.SaveAsync();
        }

        private void CheckIfStageNumberExists(Stage stage)
        {
            if (_UnitOfWork.StageRepository.Get(s => s.RaceId == stage.RaceId).Any(s => s.Number == stage.Number))
            {
                throw new BusinessException($"A stage with number '{stage.Number}' already exists.", BLErrorCodes.Duplicate);
            }
        }
    }
}
