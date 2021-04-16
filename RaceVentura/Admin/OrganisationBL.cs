using RaceVenturaData;
using RaceVenturaData.Models;
using System;
using System.Collections.Generic;

namespace RaceVentura.Admin
{
    public class OrganisationBL : IOrganisationBL
    {
        private readonly IRaceVenturaUnitOfWork _unitOfWork;

        public OrganisationBL(IRaceVenturaUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Organisation> Get()
        {
            throw new NotImplementedException();
        }
        public void Add(Organisation organisation)
        {
            throw new NotImplementedException();
        }

        public void AddUserToOrganisation(Guid organizationId, string emailAddress)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public void Edit(Organisation organisation)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserToOrganisation(Guid organizationId, string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
