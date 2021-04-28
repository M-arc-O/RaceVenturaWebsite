using System;

namespace RaceVentura.Races
{
    public interface IGenericCudBL<EntityType>
    {
        void Add(Guid userId, EntityType entity);
        EntityType Edit(Guid userId, EntityType newEntity);
        void Delete(Guid userId, Guid entityId);
    }
}
