using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserHospital.Entity;
using UserHospital.GlobalExceptions;
using UserHospital.Interface;
using UserHospital.Models;

namespace UserHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUser _user;

        public UserManagementController(IUser user)
        {
            _user = user;
        }


       [HttpPost]
       [Route("signup/admin")]
        public async Task<IActionResult> AdminRegistration(UserRegistrationModel userRegistration)
        {
            try
            {
                var adduser = await _user.RegisterUser(userRegistration,"Admin");
                if (adduser)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Message = "Admin Registration Successful",

                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid input");
                }

            }
            catch(Exception ex) 
            {
                if(ex is DuplicateEmailException)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else if (ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);

                }
                else
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
        [HttpPost]
        [Route("signup/patient")]
        public async Task<IActionResult> PatientRegistration(UserRegistrationModel userRegistration)
        {
            try
            {
                var adduser = await _user.RegisterUser(userRegistration,"Patient");
                if (adduser)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Message = "Patient Registration Successful",

                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid input");
                }

            }
            catch (Exception ex)
            {
                if (ex is DuplicateEmailException)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else if (ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);

                }
                else
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
        [HttpPost]
        [Route("signup/doctor")]
        public async Task<IActionResult> DoctorRegistration(UserRegistrationModel userRegistration)
        {
            try
            {
                var adduser = await _user.RegisterUser(userRegistration,"Doctor");
                if (adduser)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Message = "Doctor Registration Successful",

                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid input");
                }

            }
            catch (Exception ex)
            {
                if (ex is DuplicateEmailException)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else if (ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<UserRegistrationModel>
                    {

                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);

                }
                else
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> UserLogin(UserLoginModel userLogin)
        {
            try
            {
                // Authenticate the user and generate JWT token
                var Token = await _user.UserLogin(userLogin);

                var response = new ResponseModel<string>
                {

                    Message = "Login Sucessfull",
                    Data = Token

                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                if (ex is UserNotFoundException)
                {
                    var response = new ResponseModel<UserLoginModel>
                    {

                        Success = false,
                        Message = ex.Message

                    };
                    return Ok(response);

                }
                else if (ex is InvalidPasswordException)
                {
                    var response = new ResponseModel<UserLoginModel>
                    {

                        Success = false,
                        Message = ex.Message

                    };
                    return BadRequest(response);
                }
                else
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
        [HttpGet]
        [Route("getall")]

        public async Task<IActionResult> GetUserDetails()
        {
            try
            {

                var registration = await _user.GetUserDetails();
               
                var response = new ResponseModel<IEnumerable<UserEntity>>
                {
                    
                    Message = "User Details are:",
                    Data = registration

                };
                return Ok(response);
            }   
            catch (Exception ex)
            {

                var response = new ResponseModel<string>
                {
                    Success  = false,
                    Message = ex.Message,
                    Data = null 
                };
                return Ok(response);
            }
        }



    }
}
