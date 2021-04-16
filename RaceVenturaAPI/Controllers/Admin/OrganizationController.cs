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

namespace RaceVenturaAPI.Controllers.Admin
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganisationBL _organisationBL;
        private readonly IMapper _mapper;
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(IOrganisationBL organisationBL, IMapper mapper, ILogger<OrganizationController> logger)
        {
            _organisationBL = organisationBL ?? throw new ArgumentNullException(nameof(organisationBL));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getallorganizations")]
        public IActionResult Get()
        {
            try
            {
                var organisations = _organisationBL.Get();
                return Ok(_mapper.Map<List<OrganisationViewModel>>(organisations));
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
        public IActionResult Add(OrganisationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var organisation = _mapper.Map<Organisation>(viewModel);
                _organisationBL.Add(organisation);
                return Ok(_mapper.Map<OrganisationViewModel>(organisation));
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
        public IActionResult Edit(OrganisationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var organisation = _mapper.Map<Organisation>(viewModel);
                _organisationBL.Edit(organisation);
                return Ok(_mapper.Map<OrganisationViewModel>(organisation));
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
        public IActionResult Delete(Guid organizationId)
        {
            try
            {
                _organisationBL.Delete(organizationId);
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
        public IActionResult AddUserToOrganization(AddUserToOrganizationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _organisationBL.AddUserToOrganisation(viewModel.OrganizationId, viewModel.EmailAddress);
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
        public IActionResult RemoveUserFromOrganization(AddUserToOrganizationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _organisationBL.RemoveUserToOrganisation(viewModel.OrganizationId, viewModel.EmailAddress);
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
