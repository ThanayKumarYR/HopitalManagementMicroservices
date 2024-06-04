using UserHospital.Entity;
using UserHospital.Models;

namespace UserHospital.Interface
{
    public interface IUser
    {
        public Task<bool> RegisterUser(UserRegistrationModel userRegistrationModel,string Role);
        public Task<string> UserLogin(UserLoginModel userLogin);

        public Task<IEnumerable<UserEntity>> GetUserDetails();
    }
}
