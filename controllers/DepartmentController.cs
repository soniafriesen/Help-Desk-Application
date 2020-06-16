//Project: Case Study1
//Purpose: Division controller, route.api
//Coder: Sonia Friesen, 0813682
//Date: Due oct 23rd.


using Microsoft.AspNetCore.Mvc;
using System;
using HelpdeskViewModels;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CasestudyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger _logger;
        public DepartmentController(ILogger<DepartmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                DepartmentViewModel viewModel = new DepartmentViewModel();
                List<DepartmentViewModel> allDepartments = viewModel.GetAll();
                return Ok(allDepartments);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}