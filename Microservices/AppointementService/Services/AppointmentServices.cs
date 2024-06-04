using Dapper;
using Microsoft.AspNetCore.Mvc;
using AppointementService.Entity;
using AppointementService.Interface;
using AppointementService.Model;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppointementService.Conetxt;

namespace AppointementService.Services
{
    public class AppointmentServices : IAppointment
    {
        private readonly DapperContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AppointmentServices(DapperContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> CreateAppointment(AppointmentRequest appointment, int PatientID, int DoctorID)
        {
            try
            {
                var insertQuery = "spCreateAppointment";
                var appointmentEntity = MapToEntity(appointment, PatientID, getDoctorById(DoctorID));
                using (var connection = _context.CreateConnection())
                {
                    return await connection.ExecuteAsync(insertQuery, appointmentEntity, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private AppointmentEntity MapToEntity(AppointmentRequest request, int PatientId, DoctorEntity userObject)
        {
            return new AppointmentEntity
            {
                PatientName = request.PatientName,
                PatientAge = request.PatientAge,
                Issue = request.Issue,
                DoctorName = userObject?.DoctorName ?? "",
                Specialization = userObject?.Specialization ?? "",
                AppointmentDate = DateTime.Now,
                Status = false,
                BookedWith = userObject?.DoctorId ?? 0,
                BookedBy = PatientId
            };
        }

        public DoctorEntity getDoctorById(int doctorId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("GetDoctorById");
                var response = httpClient.GetAsync($"GetDoctorById?doctorId={doctorId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadFromJsonAsync<DoctorEntity>().Result;
                }
                else
                {
                    throw new Exception("Doctor not found. Please check the provided DoctorID.");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred while fetching doctor details: {e.Message}");
            }
        }

        public async Task<IEnumerable<AppointmentRequest>> GetAllAppointmentsByPatient(int patientId)
        {
            try
            {
                var query = "spGetAllAppointmentsByPatient";
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<AppointmentRequest>(query, new { PatientId = patientId }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving appointments by patient.", ex);
            }
        }

        public async Task<IEnumerable<AppointmentRequest>> GetAllAppointmentsByDoctor(int doctorId)
        {
            try
            {
                var query = "spGetAllAppointmentsByDoctor";
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<AppointmentRequest>(query, new { DoctorId = doctorId }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving appointments by doctor. {ex.Message}");
            }
        }

        public async Task<IEnumerable<AppointmentRequest>> GetAppointmentsById(int appointmentId)
        {
            try
            {
                var query = "spGetAppointmentById";
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<AppointmentRequest>(query, new { AppointmentId = appointmentId }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving appointments by ID. {ex.Message}");
            }
        }

        public async Task<AppointmentRequest> UpdateAppointment(AppointmentRequest request, int patientId, int appointmentId)
        {
            try
            {
                var existingAppointment = GetAppointmentsById(appointmentId).Result.FirstOrDefault();
                if (existingAppointment == null)
                {
                    throw new Exception("Appointment not found");
                }

                existingAppointment.PatientName = request.PatientName;
                existingAppointment.PatientAge = request.PatientAge;
                existingAppointment.Issue = request.Issue;
                existingAppointment.AppointmentDate = request.AppointmentDate;

                var updateQuery = "spUpdateAppointment";
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(updateQuery, existingAppointment, commandType: CommandType.StoredProcedure);
                    return existingAppointment;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the appointment.", ex);
            }
        }

        public async Task<AppointmentRequest> UpdateStatus(int appointmentId, string status)
        {
            try
            {
                var query = "spUpdateStatus";
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { AppointmentId = appointmentId, Status = status }, commandType: CommandType.StoredProcedure);
                    return (AppointmentRequest)await GetAppointmentsById(appointmentId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the appointment status.", ex);
            }
        }

        public async Task<bool> CancelAppointment(int bookedBy, int appointmentId)
        {
            try
            {
                var deleteQuery = "spDeleteAppointment";
                using (var connection = _context.CreateConnection())
                {
                    var rowsAffected = await connection.ExecuteAsync(deleteQuery, new { AppointmentId = appointmentId, BookedBy = bookedBy }, commandType: CommandType.StoredProcedure);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the appointment.", ex);
            }
        }
    }
}
