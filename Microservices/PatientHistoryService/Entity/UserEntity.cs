namespace PatientHistoryService.Entity
{
    public class UserEntity
    {
        public int UserID{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string Role { get; set; }
    }
}
