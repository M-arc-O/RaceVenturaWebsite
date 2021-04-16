using RaceVenturaData.Models;
using System;
using System.Collections.Generic;

namespace RaceVentura.Admin
{
    public interface IOrganisationBL
    {
        IEnumerable<Organisation> Get();
        void Add(Organisation organisation);
        void Edit(Organisation organisation);
        void Delete(Guid organizationId);
        void AddUserToOrganisation(Guid organizationId, string emailAddress);
        void RemoveUserToOrganisation(Guid organizationId, string emailAddress);
    }
}
