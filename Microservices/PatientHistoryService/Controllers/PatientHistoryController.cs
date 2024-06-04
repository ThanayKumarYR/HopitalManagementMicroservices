using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientHistoryService.Entity;
using PatientHistoryService.Interface;
using PatientHistoryService.Model;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientHistoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientHistoryController : ControllerBase
    {
        private readonly IPatientHistory _history;
        private readonly HttpClient _httpClient;

        public PatientHistoryController(IPatientHistory history, HttpClient httpClient)
        {
            _history = history;
            _httpClient = httpClient;
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost("AddHistory")]
        public async Task<ActionResult<List<object>>> AddPatientHistory(HistoryRequest historyRequest)
        {
            try
            {
                Console.WriteLine("inside");
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = int.Parse(userIdClaim);
                Console.WriteLine("inside");
                var data = await _history.AddPatientHistory(historyRequest, userId);
                Console.WriteLine("Outside");
                return Ok(new { Success = true, Message = "Patient Added Successfully", Data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetHistory(int PatientId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = int.Parse(userIdClaim);
                var history =  await _history.GetHistory(PatientId, userId);
                return Ok(new { Succes = "True", Message = "Patient Details: ",Data = history });
            }
            catch(Exception ex)
            {
                return Ok(new { Succes = "false", Message = ex.Message });
            }
        }
    }
}
