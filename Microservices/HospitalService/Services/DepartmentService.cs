using Dapper;
using HospitalService.Context;
using HospitalService.Entity;
using HospitalService.Interface;
using HospitalService.Model;
using System.Data;
using System.Linq.Expressions;

namespace HospitalService.Services
{
    public class DepartmentService : IDepartment
    {
        private readonly DapperContext context;

        public DepartmentService(DapperContext _context)
        {
            context = _context;
        }

        public object? CreateDept(DepartmentRequest deptRequest)
        {
            try
            {
                var query = "spCreateDepartment";
                DepartmentEntity e = MapToEntity(deptRequest);
                var parameters = new DynamicParameters();
                parameters.Add("@DeptName", deptRequest.DeptName);
                using (var connection = context.CreateConnection())
                {
                    var result = connection.QueryFirstOrDefault<int>(query, parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public DepartmentEntity? getByDeptId(int id)
        {
            try
            {
                var query = "spGetDepartmentById";
                var parameters = new DynamicParameters();
                parameters.Add("@DeptId", id);
                using (var connection = context.CreateConnection())
                {
                    var result = connection.QueryFirstOrDefault<DepartmentEntity>(query, parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<DepartmentRequest> UpdateDepartment(int DeptId, DepartmentRequest deptRequest)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("DeptId", DeptId, DbType.Int64);
                parameters.Add("DeptName", deptRequest.DeptName, DbType.String);
                var query = "spUpdateDepartment";
                using (var connection = context.CreateConnection())
                {
                    int rowsaffected = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                    if (rowsaffected > 0)
                    {
                        return deptRequest;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while updating the department: {ex.Message}");
                return default;
            }
        }

        public async Task<int> Deletedepartment(int DeptId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("DeptId", DeptId, DbType.Int64);
                var query = "spDeleteDepartment";
                using (var connection = context.CreateConnection())
                {
                    var rowsaffected = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                    if (rowsaffected > 0)
                    {
                        return rowsaffected;
                    }
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public DepartmentEntity? getByDeptName(string name)
        {
            try
            {
                string query = "spGetDepartmentByName";
                using (var connection = context.CreateConnection())
                {
                    var result = connection.QueryFirstOrDefault<DepartmentEntity>(query, new { DeptName = name }, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        private DepartmentEntity MapToEntity(DepartmentRequest request) => new DepartmentEntity { DeptName = request.DeptName };
    }
}
