using Microsoft.AspNetCore.Mvc;
using PatientHistoryService.Model;

namespace PatientHistoryService.Interface
{
    public interface IPatientHistory
    {
        public Task<ActionResult<List<object>>> AddPatientHistory(HistoryRequest historyRequest, int userId);
        public Task<ActionResult<List<object>>> GetHistory(int PatientId, int userId);
    }
}
