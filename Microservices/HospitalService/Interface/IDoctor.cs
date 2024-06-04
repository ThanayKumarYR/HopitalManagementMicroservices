using HospitalService.Entity;
using HospitalService.Model;

namespace HospitalService.Interface
{
    public interface IDoctor
    {
        public Task<bool> CreateDoctor(DoctorRequest request);
        public  Task<IEnumerable<DoctorEntity>> GetDoctorById(int doctorId);
        public  Task<IEnumerable<DoctorEntity>> GetAllDoctors();
        public Task<bool> UpdateDoctor(int doctorId, DoctorRequest request);
        public Task<bool> DeleteDoctor(int doctorId);
        public  Task<IEnumerable<DoctorEntity>> GetDoctorsBySpecialization(string specialization);
    }
}
