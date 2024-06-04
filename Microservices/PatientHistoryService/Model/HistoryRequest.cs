namespace PatientHistoryService.Model
{
    public class HistoryRequest
    {
        public int PatientId { get; set; }
        public string Issue { get; set; }
        public DateTime VisitsToDoctor { get; set; }
    }
}
