using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaceVentura;
using RaceVentura.Admin;
using RaceVenturaAPI.ViewModels;
using RaceVenturaAPI.ViewModels.Admin;
using RaceVenturaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaceVenturaAPI.Controllers.Admin
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationBL _organizationBL;
        private readonly IMapper _mapper;
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(IOrganizationBL organizationBL, IMapper mapper, ILogger<OrganizationController> logger)
        {
            _organizationBL = organizationBL ?? throw new ArgumentNullException(nameof(organizationBL));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getallorganizations")]
        public IActionResult Get()
        {
            try
            {
                var organizations = _organizationBL.Get();

                var retVal = _mapper.Map<List<OrganizationViewModel>>(organizations);
                foreach (var organization in retVal)
                {
                    organization.UserEmails = _organizationBL.GetUserEmails(organization.OrganizationId).ToList();
                }

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(OrganizationController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addorganization")]
        public async Task<IActionResult> Add(OrganizationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var organization = _mapper.Map<Organization>(viewModel);
                await _organizationBL.Add(organization);
                return Ok(_mapper.Map<OrganizationViewModel>(organization));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(OrganizationController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("editorganization")]
        public async Task<IActionResult> Edit(OrganizationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var newOrganization = _mapper.Map<Organization>(viewModel);
                var organization = await _organizationBL.Edit(newOrganization);
                return Ok(_mapper.Map<OrganizationViewModel>(organization));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(OrganizationController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{organizationId}/remove")]
        public async Task<IActionResult> Delete(Guid organizationId)
        {
            try
            {
                await _organizationBL.Delete(organizationId);
                return Ok(organizationId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(OrganizationController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addusertoorganization")]
        public async Task<IActionResult> AddUserToOrganization(AddUserToOrganizationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _organizationBL.AddUserToOrganization(viewModel.OrganizationId, viewModel.EmailAddress);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(OrganizationController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("removeuserfromorganization")]
        public async Task<IActionResult>  RemoveUserFromOrganization(AddUserToOrganizationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _organizationBL.RemoveUserFromOrganization(viewModel.OrganizationId, viewModel.EmailAddress);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(OrganizationController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
