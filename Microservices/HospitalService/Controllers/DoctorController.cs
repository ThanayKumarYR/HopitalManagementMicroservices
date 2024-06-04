using HospitalService.Entity;
using HospitalService.Interface;
using HospitalService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctor _doctor;

        public DoctorController(IDoctor doctor)
        {
            _doctor = doctor;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(DoctorRequest request)
        {
            try
            {
                var result =  await _doctor.CreateDoctor(request);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Doctor Created Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                return BadRequest("invalid input");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetDoctorById")]
        public async Task<IActionResult> GetDoctorById(int doctorId)
             
        {
            try
            {
                var doctor = await _doctor.GetDoctorById(doctorId);
                var response = new ResponseModel<IEnumerable<DoctorEntity>>
                {
                    Success = true,
                    Message = "Doctor details:",
                    Data = doctor
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var result = await _doctor.GetAllDoctors();
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<DoctorEntity>>
                    {
                        Success = true,
                        Message = "Details Fetched Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Input",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{doctorId}")]
        public  async Task<IActionResult> UpdateDoctor(int doctorId, DoctorRequest request)
        {
            try
            {
                var result = await _doctor.UpdateDoctor(doctorId, request);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Updated Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Input",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("specialization/{specialization}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(string specialization)
        {
            try
            {
                var result = await _doctor.GetDoctorsBySpecialization(specialization);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<DoctorEntity>>
                    {
                        Success = true,
                        Message = "Details Fetched Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Input",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{doctorId}")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            try
            {
                var result = await _doctor.DeleteDoctor(doctorId);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Deleted Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Input",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

    }

}

