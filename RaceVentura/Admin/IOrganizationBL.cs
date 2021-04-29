using RaceVenturaData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaceVentura.Admin
{
    public interface IOrganizationBL
    {
        IEnumerable<Organization> Get();
        Task Add(Organization organization);
        Task<Organization> Edit(Organization organization);
        Task Delete(Guid organizationId);
        Task AddUserToOrganization(Guid organizationId, string emailAddress);
        Task RemoveUserFromOrganization(Guid organizationId, string emailAddress);
        IEnumerable<string> GetUserEmails(Guid organizationId);
    }
}
