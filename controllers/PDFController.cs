using CasestudyWebsite.Reports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace CasestudyWebsite.Controllers
{   
    public class PDFController : Controller
    {
        private IHostingEnvironment _env;
        public PDFController(IHostingEnvironment env)
        {
            _env = env;
        }
        [Route("api/employeereport")]
        public IActionResult GetEmployeeReport()
        {
            EmployeeReport emp = new EmployeeReport();
            emp.generateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }

        [Route("api/callreport")]
        public IActionResult GetCallReport()
        {
            CallReport call = new CallReport();
            call.generateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }

        [Route("api/employeecallreport/[action]/{id}")]
        public IActionResult GetEmployeeCallReport(int id)
        {
            EmployeeCallReport empcall = new EmployeeCallReport();
            empcall.generateReport(_env.WebRootPath, id);
            return Ok(new { msg = "Report Generated" });
        }
    }
}