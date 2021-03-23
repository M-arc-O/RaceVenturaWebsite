using Microsoft.Extensions.Logging;
using RaceVentura.Models;
using RaceVentura.Models.RaceAccess;
using RaceVenturaData;
using RaceVenturaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaceVentura.Races
{
    public class RaceAccessBL : RaceBaseBL, IRaceAccessBL
    {
        private readonly IAccountBL _accountBL;

        public RaceAccessBL(IAccountBL accountBL, IRaceVenturaUnitOfWork unitOfWork, ILogger<RaceAccessBL> logger) : base(unitOfWork, logger)
        {
            _accountBL = accountBL ?? throw new ArgumentNullException(nameof(accountBL));
        }

        public async Task<IEnumerable<RaceAccess>> Get(Guid userId, Guid raceId)
        {
            CheckUserIsAuthorizedForRace(userId, raceId, RaceAccessLevel.Owner);
            var userLinks = _unitOfWork.UserLinkRepository.Get(userlink => userlink.RaceId == raceId && userlink.RaceAccess != RaceAccessLevel.Owner);

            var retVal = new List<RaceAccess>(userLinks.Count());

            foreach (var link in userLinks)
            {
                var raceAccess = new RaceAccess
                {
                    Email = await _accountBL.FindEmailByIdAsync(link.UserId),
                    RaceAccessLevel = link.RaceAccess,
                    RaceId = link.RaceId
                };

                retVal.Add(raceAccess);
            }

            return retVal;
        }
        
        public RaceAccessLevel GetAccessLevel(Guid userId, Guid raceId)
        {
            var userLink = GetRaceUserLink(userId, raceId);
            if (userLink == null)
            {
                _logger.LogWarning($"Error in {GetType().Name}: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");
                throw new BusinessException($"User is not authorized for race.", BLErrorCodes.UserUnauthorized);
            }

            return userLink.RaceAccess;
        }

        public async Task Add(Guid userId, Guid raceId, string emailaddress, RaceAccessLevel raceAccessLevel)
        {
            CheckUserIsAuthorizedForRace(userId, raceId, RaceAccessLevel.Owner);

            var sharedUserId = await GetSharedUserId(emailaddress);

            _unitOfWork.UserLinkRepository.Insert(new UserLink
            {
                RaceId = raceId,
                UserId = sharedUserId,
                RaceAccess = raceAccessLevel
            });

            _unitOfWork.Save();
        }

        public async Task Edit(Guid userId, Guid raceId, string emailaddress, RaceAccessLevel raceAccessLevel)
        {
            var editLink = await GetLink(userId, raceId, emailaddress);

            editLink.RaceAccess = raceAccessLevel;

            _unitOfWork.UserLinkRepository.Update(editLink);
            _unitOfWork.Save();
        }

        public async Task Delete(Guid userId, Guid raceId, string emailaddress)
        {
            var deleteLink = await GetLink(userId, raceId, emailaddress);

            _unitOfWork.UserLinkRepository.Delete(deleteLink);
            _unitOfWork.Save();
        }

        private async Task<Guid> GetSharedUserId(string emailaddress)
        {
            var user = await _accountBL.FindByEmailAsync(emailaddress);

            if (user == null)
            {
                throw new BusinessException($"No user with email address '{emailaddress}' found.", BLErrorCodes.NotFound);
            }

            return new Guid(user.Id);
        }

        private async Task<UserLink> GetLink(Guid userId, Guid raceId, string emailaddress)
        {
            CheckUserIsAuthorizedForRace(userId, raceId, RaceAccessLevel.Owner);

            var sharedUserId = await GetSharedUserId(emailaddress);

            var editLink = _unitOfWork.UserLinkRepository.Get(link => link.RaceId == raceId && link.UserId == sharedUserId).FirstOrDefault();
            return editLink;
        }
    }
}
