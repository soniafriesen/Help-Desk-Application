using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HelpdeskDAL;
using HelpdeskViewModels;

namespace CasestudyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ILogger _logger;

        public ProblemController(ILogger<ProblemController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{description}")]
        public IActionResult GetByDescription(string description)
        {
            try
            {
                ProblemViewModel viewmodel = new ProblemViewModel();
                viewmodel.Description = description;
                viewmodel.GetByDescription();
                return Ok(viewmodel);
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
                ProblemViewModel viewModel = new ProblemViewModel();
                List<ProblemViewModel> allProblems = viewModel.GetAll();
                return Ok(allProblems);
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