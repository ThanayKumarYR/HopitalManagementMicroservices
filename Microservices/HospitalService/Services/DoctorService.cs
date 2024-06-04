using Dapper;
using HospitalService.Context;
using HospitalService.Entity;
using HospitalService.Interface;
using HospitalService.Model;
using System.Data;
using System.Numerics;
using System.Threading.Tasks;

namespace HospitalService.Services
{
    public class DoctorService : IDoctor
    {
        private readonly DapperContext _context;

        public DoctorService(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateDoctor(DoctorRequest request)
        {
            try
            {
                var query = "spCreateDoctor";
                DoctorEntity e = MapToEntity(request);
                var parameters = new DynamicParameters();
                parameters.Add("@DoctorId", e.DoctorId);
                parameters.Add("@DeptId", e.DeptId);
                parameters.Add("@DoctorName", e.DoctorName);
                parameters.Add("@DoctorAge", e.DoctorAge);
                parameters.Add("@DoctorAvailable", e.DoctorAvailable);
                parameters.Add("@Specialization", e.Specialization);
                parameters.Add("@Qualifications", e.Qualifications);
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DoctorEntity MapToEntity(DoctorRequest request)
        {
            return new DoctorEntity
            {
                DoctorId = request.DoctorId,
                DeptId = request.DeptId,
                DoctorName = request.DoctorName,
                DoctorAge = request.DoctorAge,
                DoctorAvailable = request.DoctorAvailable,
                Specialization = request.Specialization,
                Qualifications = request.Qualifications
            };
        }

        public async Task<IEnumerable<DoctorEntity>> GetDoctorById(int doctorId)
        {
            try
            {
                var query = "spGetDoctorById";
                var parameters = new DynamicParameters();
                parameters.Add("@DoctorId", doctorId);
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryAsync<DoctorEntity>(query, parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<DoctorEntity>> GetAllDoctors()
        {
            try
            {
                var query = "spGetAllDoctors";
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryAsync<DoctorEntity>(query, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateDoctor(int doctorId, DoctorRequest request)
        {
            try
            {
                var query = "spUpdateDoctor";
                var parameters = new DynamicParameters();
                parameters.Add("@DoctorId", doctorId);
                parameters.Add("@DoctorName", request.DoctorName);
                parameters.Add("@DoctorAge", request.DoctorAge);
                parameters.Add("@DoctorAvailable", request.DoctorAvailable);
                parameters.Add("@Specialization", request.Specialization);
                parameters.Add("@Qualifications", request.Qualifications);

                using (var connection = _context.CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteDoctor(int doctorId)
        {
            try
            {
                var query = "spDeleteDoctor";
                var parameters = new DynamicParameters();
                parameters.Add("@DoctorId", doctorId);

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<DoctorEntity>> GetDoctorsBySpecialization(string specialization)
        {
            try
            {
                var query = "spGetDoctorsBySpecialization";
                var parameters = new DynamicParameters();
                parameters.Add("@Specialization", specialization);

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryAsync<DoctorEntity>(query, parameters, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
