using Confluent.Kafka;
using Dapper;
using System.Data;
using System.Text.RegularExpressions;
using UserHospital.Context;
using UserHospital.Entity;
using UserHospital.GlobalExceptions;
using UserHospital.Helper;
using UserHospital.Interface;
using UserHospital.Models;

namespace UserHospital.Service
{
    public class UserServicecs : IUser
    {
        private readonly UsermanagementContext _context;
        private readonly IAuthServices _authService;

        public UserServicecs(UsermanagementContext context, IAuthServices authService)
        {
            _context = context;
            _authService = authService;
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$";
            return Regex.IsMatch(password, pattern);
        }

        public async Task<bool> RegisterUser(UserRegistrationModel userRegModel, string role)
        {
            var parametersToCheckEmailIsValid = new DynamicParameters();
            parametersToCheckEmailIsValid.Add("Email", userRegModel.Email, DbType.String);

            var parameters = new DynamicParameters();
            parameters.Add("FirstName", userRegModel.FirstName, DbType.String);
            parameters.Add("LastName", userRegModel.LastName, DbType.String);

            // Check Email format using Regex
            if (!IsValidEmail(userRegModel.Email))
            {
                throw new InvalidEmailFormatException("Invalid email format");
            }

            parameters.Add("Email", userRegModel.Email, DbType.String);

            // Convert plain password into a hashed string
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegModel.Password);
            parameters.Add("Password", hashedPassword, DbType.String);

            parameters.Add("Role", role, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                // Ensure the Users table exists
                await connection.ExecuteAsync("CreateUsersTableIfNotExists", commandType: CommandType.StoredProcedure);

                // Check if email already exists
                bool emailExists = await connection.ExecuteScalarAsync<bool>("CheckEmailDuplication", parametersToCheckEmailIsValid, commandType: CommandType.StoredProcedure);
                if (emailExists)
                {
                    throw new DuplicateEmailException("Email address is already in use");
                }

                // Insert new user
                await connection.ExecuteAsync("RegisterUser", parameters, commandType: CommandType.StoredProcedure);
            }

            var registrationDetailsForPublishing = new RegistrationDetailsForPublishing(userRegModel);

            // Serialize registration details to a JSON string
            var registrationDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(registrationDetailsForPublishing);

            // Get Kafka producer configuration

            var producerConfig = KafkaProducer.GetProducerConfig();

            // Create a Kafka producer
            KafkaProducer.produceTopic(producerConfig, registrationDetailsJson);

            var consumerConfig = KafkaConsumer.GetConsumerConfig();

            KafkaConsumer.consumeTopic(consumerConfig);

            return true;
        }

        public async Task<string> UserLogin(UserLoginModel userLogin)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", userLogin.Email);

                var user = await connection.QueryFirstOrDefaultAsync<UserEntity>("UserLogin", parameters, commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    throw new UserNotFoundException($"User with email '{userLogin.Email}' not found.");
                }

                if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
                {
                    throw new InvalidPasswordException("Invalid password.");
                }

                // If the password entered by the user matches the password in the database, generate a token
                var token = _authService.GenerateJwtToken(user);
                return token;
            }
        }

        public async Task<IEnumerable<UserEntity>> GetUserDetails()
        {
            var query = "SELECT * FROM Users";
            using (var connection = _context.CreateConnection())
            {
                var registration = await connection.QueryAsync<UserEntity>(query);
                return registration.ToList();
            }
        }
    }
}
