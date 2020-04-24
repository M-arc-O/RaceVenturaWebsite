using Adventure4YouData.Models.Races;
using System;
using System.Collections.Generic;

namespace Adventure4You.Races
{
    public interface IGenericCrudBL<EntityType> : IGenericCudBL<EntityType>
    {
        IEnumerable<EntityType> Get(Guid userId);
        EntityType GetById(Guid userId, Guid entityId);
    }
}