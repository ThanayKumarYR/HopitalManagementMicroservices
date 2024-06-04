namespace UserHospital.GlobalExceptions
{
    public class DuplicateEmailException :Exception
    {
        public DuplicateEmailException(string message):base(message) { }
    }
}
