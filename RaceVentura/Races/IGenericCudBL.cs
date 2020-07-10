using System;

namespace RaceVentura.Races
{
    public interface IGenericCudBL<T>
    {
        void Add(Guid userId, T entity);
        void Edit(Guid userId, T newEntity);
        void Delete(Guid userId, Guid entityId);
    }
}
