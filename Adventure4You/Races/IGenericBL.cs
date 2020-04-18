using System;

namespace Adventure4You.Races
{
    public interface IGenericBL<T>
    {
        void Add(Guid userId, T entity);
        void Edit(Guid userId, T newEntity);
        void Delete(Guid userId, Guid entityId);
    }
}
