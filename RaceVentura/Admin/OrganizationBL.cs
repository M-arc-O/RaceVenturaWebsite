using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models;
using RaceVenturaData.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaceVentura.Admin
{
    public class OrganizationBL : IOrganizationBL
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRaceVenturaUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public OrganizationBL(UserManager<AppUser> userManager, IRaceVenturaUnitOfWork unitOfWork, ILogger<OrganizationBL> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Organization> Get()
        {
            return _unitOfWork.OrganizationRepository.Get();
        }

        public async Task Add(Organization organization)
        {
            CheckIfOrganisationNameExists(organization.Name);

            _unitOfWork.OrganizationRepository.Insert(organization);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Organization> Edit(Organization newOrganization)
        {
            CheckIfOrganizationExsists(newOrganization.OrganizationId);
            CheckIfOrganisationNameExists(newOrganization.Name);

            var organization = _unitOfWork.OrganizationRepository.GetByID(newOrganization.OrganizationId);
            organization.Name = newOrganization.Name;

            _unitOfWork.OrganizationRepository.Update(organization);
            await _unitOfWork.SaveAsync();

            return organization;
        }

        public async Task Delete(Guid organizationId)
        {
            CheckIfOrganizationExsists(organizationId);

            _unitOfWork.OrganizationRepository.Delete(organizationId);

            _userManager.Users.Where(user => user.OrganizationId.Equals(organizationId)).ToList().ForEach(user =>
            {
                user.OrganizationId = Guid.Empty;
                _userManager.UpdateAsync(user);
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task AddUserToOrganization(Guid organizationId, string emailAddress)
        {
            CheckIfOrganizationExsists(organizationId);
            var user = await GetUser(emailAddress);

            user.OrganizationId = organizationId;

            await _userManager.UpdateAsync(user);
        }

        public async Task RemoveUserFromOrganization(Guid organizationId, string emailAddress)
        {
            CheckIfOrganizationExsists(organizationId);
            var user = await GetUser(emailAddress);

            if (user.OrganizationId != organizationId)
            {
                throw new BusinessException($"User with email address '{emailAddress}' is not part of organization with ID '{organizationId}'.", BLErrorCodes.NotAsignedToOrganization);
            }

            user.OrganizationId = Guid.Empty;

            await _userManager.UpdateAsync(user);
        }

        private async Task<AppUser> GetUser(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user == null)
            {
                throw new BusinessException($"User with email address '{emailAddress}' does not exists.", BLErrorCodes.NotFound);
            }

            return user;
        }

        private void CheckIfOrganizationExsists(Guid organizationId)
        {
            if (_unitOfWork.OrganizationRepository.GetByID(organizationId) == null)
            {
                _logger.LogError($"Error in {GetType().Name}: Someone tried to access organization with ID '{organizationId}' but it does not exsist.");
                throw new BusinessException($"Organization with ID '{organizationId}' does not exsist.", BLErrorCodes.NotFound);
            }
        }

        private void CheckIfOrganisationNameExists(string name)
        {
            if (_unitOfWork.OrganizationRepository.Get().Any(organisation => organisation.Name.ToUpper().Equals(name.ToUpper())))
            {
                throw new BusinessException($"An organization with name '{name}' already exists.", BLErrorCodes.Duplicate);
            }
        }
    }
}
