using UserHospital.Entity;

namespace UserHospital.Interface
{
    public interface IAuthServices
    {
        public string GenerateJwtToken(UserEntity user);
    }
}
