//Project: Case Study1
//Purpose: Demonstrate the route/api 
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
    public class EmployeeController:ControllerBase
    {
        private readonly ILogger _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel();
                viewmodel.Email = email;
                viewmodel.GetByEmail();
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpPut]
        public IActionResult Put(EmployeeViewModel viewModel)
        {
            try
            {
                int retVal = viewModel.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok(new { msg = "Employee " + viewModel.Lastname + " updated!" });
                    case -1:
                        return Ok(new { msg = "Employee " + viewModel.Lastname + " not updated!" });
                    case -2:
                        return Ok(new { msg = "Data stale for " + viewModel.Lastname + ", Employee not updated!" });
                    default:
                        return Ok(new { msg = "Employee " + viewModel.Lastname + " not updated!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                EmployeeViewModel viewModel = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = viewModel.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpPost]
        public IActionResult Post(EmployeeViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Employee " + viewmodel.Lastname + " added!" })
                : Ok(new { msg = "Employee " + viewmodel.Lastname + " not added!" });

            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {

                EmployeeViewModel viewModel = new EmployeeViewModel();
                viewModel.Id = id;
                return viewModel.Delete() == 1
                    ? Ok(new { msg = "Employee " + id + " deleted!" })
                    : Ok(new { msg = "Employee " + id + " not deleted!" });
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
