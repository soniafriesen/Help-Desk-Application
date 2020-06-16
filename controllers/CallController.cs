using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HelpdeskDAL;

namespace CasestudyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly ILogger _logger;

        public CallController(ILogger<CallController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                CallViewModel call = new CallViewModel();
                call.Id = id;
                call.GetById();
                return Ok(call);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }      

        [HttpPut] 
        public IActionResult Put(CallViewModel call)
        {
            try
            {
                int retVal = call.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok(new { msg = "Call " + call.Id + " updated!" });
                    case -1:
                        return Ok(new { msg = "Call " + call.Id + " not updated!" });
                    case -2:
                        return Ok(new { msg = "Data stale for " + call.Id + ", Call not updated!" });
                    default:
                        return Ok(new { msg = "Call " + call.Id + " not updated!" });
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
                CallViewModel viewModel = new CallViewModel();
                List<CallViewModel> allCalls = viewModel.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpPost]
        public IActionResult Post(CallViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Call " + viewmodel.Id + " added!" })
                : Ok(new { msg = "Call " + viewmodel.Id + " not added!" });

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

                CallViewModel viewModel = new CallViewModel();
                viewModel.Id = id;
                return viewModel.Delete() == 1
                    ? Ok(new { msg = "Call " + id + " deleted!" })
                    : Ok(new { msg = "Call " + id + " not deleted!" });
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