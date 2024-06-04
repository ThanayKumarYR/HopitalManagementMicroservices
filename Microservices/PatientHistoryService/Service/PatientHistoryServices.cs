using Dapper;
using Microsoft.AspNetCore.Mvc;
using PatientHistoryService.Context;
using PatientHistoryService.Entity;
using PatientHistoryService.Interface;
using PatientHistoryService.Model;
using System.Net.Http;

namespace PatientHistoryService.Service
{
    public class PatientHistoryServices : IPatientHistory
    {
        private readonly DapperContext _context;

        private readonly IHttpClientFactory httpClientFactory;

        public PatientHistoryServices(DapperContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ActionResult<List<object>>> AddPatientHistory(HistoryRequest historyRequest, int doctorId)
        {
            try
            {
                string historyQuery = "INSERT INTO History (DoctorId, PatientId, Issue, VisitsToDoctor) " +
                                      "VALUES (@DoctorId, @PatientId, @Issue, @VisitsToDoctor);";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("PatientId", historyRequest.PatientId);
                dynamicParameters.Add("DoctorId", doctorId);
                dynamicParameters.Add("Issue", historyRequest.Issue);
                dynamicParameters.Add("VisitsToDoctor", historyRequest.VisitsToDoctor);
                Console.WriteLine("inside");
                UserEntity user = getUserById(historyRequest.PatientId);
                Console.WriteLine("Outside");
                if (user == null)
                {
                    Console.WriteLine("user not found");
                }

                var patientQuery = @"INSERT INTO PatientHistory (PatientId, PatientName, Email) 
                         VALUES (@PatientId, @PatientName, @Email)";

                DynamicParameters dynamicParameter = new DynamicParameters();
                dynamicParameter.Add("PatientId", user.UserID);
                dynamicParameter.Add("PatientName", user.FirstName);
                dynamicParameter.Add("Email", user.Email);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(historyQuery, dynamicParameters);

                    var val = await GetPatientDetails(user.UserID);
                    if (val == null)
                    {
                        await connection.ExecuteAsync(patientQuery, dynamicParameter);
                    }

                    return await GetHistory(user.UserID, doctorId);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public UserEntity getUserById(int patientId)
        {
            var httpclient = httpClientFactory.CreateClient("getbyid");
           Console.WriteLine("part 4" + patientId);
            var response = httpclient.GetAsync($"getbyid?id={patientId}").Result;
           Console.WriteLine("part 4" + response);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<UserEntity>().Result;
            }
            throw new Exception("UserNotFound Create User FIRST OE TRY DIFFERENT EMAIL ID");
        }

        public async Task<ActionResult<List<object>>> GetHistory(int patientId, int doctorId)
        {
            try
            {
                PatientHistory patientHistory = await GetPatientDetails(patientId);
                List<HistoryResponse> patientHistoryList = await GetPatientHistory(patientId, doctorId);
                var result = new List<object> { patientHistory, patientHistoryList };
                return result;
            }
            catch (Exception ex)
            {
               
                return new List<object> { new { Error = $"An error occurred: {ex.Message}" } };
            }
        }


        private async Task<PatientHistory> GetPatientDetails(int patientId)
        {
            try
            {
                string selectPatientQuery = @"SELECT PatientId, PatientName, Email FROM PatientHistory WHERE PatientId = @PatientId;";

                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<PatientHistory>(selectPatientQuery, new { PatientId = patientId });
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred while fetching patient details: {ex.Message}");
                return null; 
            }
        }

        private async Task<List<HistoryResponse>> GetPatientHistory(int patientId, int doctorId)
        {
            try
            {
                string selectHistoryQuery = @"SELECT Issue, VisitsToDoctor FROM History WHERE PatientId = @PatientId AND DoctorId = @DoctorId;";

                using (var connection = _context.CreateConnection())
                {
                    return (await connection.QueryAsync<HistoryResponse>(selectHistoryQuery, new { PatientId = patientId, DoctorId = doctorId })).ToList();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching patient details: {ex.Message}");
                return null;
            }
        }

    }
}
