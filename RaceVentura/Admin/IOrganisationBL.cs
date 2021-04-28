using RaceVenturaData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaceVentura.Admin
{
    public interface IOrganisationBL
    {
        IEnumerable<Organisation> Get();
        Task Add(Organisation organisation);
        Task<Organisation> Edit(Organisation organisation);
        Task Delete(Guid organizationId);
        Task AddUserToOrganisation(Guid organizationId, string emailAddress);
        Task RemoveUserFromOrganisation(Guid organizationId, string emailAddress);
    }
}
