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
    public class OrganisationBL : IOrganisationBL
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRaceVenturaUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public OrganisationBL(UserManager<AppUser> userManager, IRaceVenturaUnitOfWork unitOfWork, ILogger<OrganisationBL> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Organisation> Get()
        {
            return _unitOfWork.OrganisationRepository.Get();
        }

        public async Task Add(Organisation organisation)
        {
            CheckIfOrganisationNameExists(organisation.Name);

            _unitOfWork.OrganisationRepository.Insert(organisation);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Organisation> Edit(Organisation newOrganisation)
        {
            CheckIfOrganizationExsists(newOrganisation.OrganizationId);
            CheckIfOrganisationNameExists(newOrganisation.Name);

            var organisation = _unitOfWork.OrganisationRepository.GetByID(newOrganisation.OrganizationId);
            organisation.Name = newOrganisation.Name;

            _unitOfWork.OrganisationRepository.Update(organisation);
            await _unitOfWork.SaveAsync();

            return organisation;
        }

        public async Task Delete(Guid organizationId)
        {
            CheckIfOrganizationExsists(organizationId);

            _unitOfWork.OrganisationRepository.Delete(organizationId);

            _userManager.Users.Where(user => user.OrganizationId.Equals(organizationId)).ToList().ForEach(user =>
            {
                user.OrganizationId = Guid.Empty;
                _userManager.UpdateAsync(user);
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task AddUserToOrganisation(Guid organizationId, string emailAddress)
        {
            CheckIfOrganizationExsists(organizationId);
            var user = await GetUser(emailAddress);

            user.OrganizationId = organizationId;

            await _userManager.UpdateAsync(user);
        }

        public async Task RemoveUserFromOrganisation(Guid organizationId, string emailAddress)
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
            if (_unitOfWork.OrganisationRepository.GetByID(organizationId) == null)
            {
                _logger.LogError($"Error in {GetType().Name}: Someone tried to access organisation with ID '{organizationId}' but it does not exsist.");
                throw new BusinessException($"Organisation with ID '{organizationId}' does not exsist.", BLErrorCodes.NotFound);
            }
        }

        private void CheckIfOrganisationNameExists(string name)
        {
            if (_unitOfWork.OrganisationRepository.Get().Any(organisation => organisation.Name.ToUpper().Equals(name.ToUpper())))
            {
                throw new BusinessException($"An organisation with name '{name}' already exists.", BLErrorCodes.Duplicate);
            }
        }
    }
}
