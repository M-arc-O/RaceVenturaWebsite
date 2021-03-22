using System;
using System.Collections.Generic;

namespace RaceVentura.Races
{
    public interface IGenericCrudBL<EntityType> : IGenericCudBL<EntityType>
    {
        IEnumerable<EntityType> Get(Guid userId);
        EntityType GetById(Guid userId, Guid entityId);
    }
}