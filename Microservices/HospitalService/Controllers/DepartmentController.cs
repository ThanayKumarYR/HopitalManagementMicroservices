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
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartment dept;
        public DepartmentController(IDepartment dept)
        {
            this.dept = dept;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateDepartment(DepartmentRequest deptRequest)
        {
            try
            {
                var res = dept.CreateDept(deptRequest);
                var response = new ResponseModel<string>
                {
                    Message = "Department created Successful",

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Message = ex.Message,

                };
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("int")]
        public IActionResult getByDeptId(int id)
        {
            try
            {
                var getres = dept.getByDeptId(id);
                var response = new ResponseModel<DepartmentEntity>
                {
                    Message = "Department Details are: ",
                    Data = getres

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Message = ex.Message,

                };
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ByName")]
        public IActionResult getByDeptName(string name)
        {
            try
            {
                var results = dept.getByDeptName(name);
                var response = new ResponseModel<DepartmentEntity>
                {
                    Message = "Department Details are: ",
                    Data = results

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Message = ex.Message,

                };
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(int DeptId, DepartmentRequest deptRequest)
        {
            try
            {
                DepartmentRequest updatedDept = await dept.UpdateDepartment(DeptId, deptRequest);
                if (updatedDept != null)
                {
                    var response = new ResponseModel<DepartmentRequest>
                    {
                        Message = "Department Details Updated Successfully",
                        Data = updatedDept
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<string>
                    {
                        Message = "Failed to update department or department not found"
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Message = ex.Message,

                };
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> deleteDepartment(int DeptId)
        {
            try
            {
                int deleteDept = await dept.Deletedepartment(DeptId);
                if (deleteDept != null)
                {
                    var response = new ResponseModel<int>
                    {
                        Message = "Department Deleted Successfully",
                        Data = deleteDept
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<string>
                    {
                        Message = "Failed to Delete department or department not found"
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Message = ex.Message,

                };
                return BadRequest(response);
            }
        }
    }
}


    
    

