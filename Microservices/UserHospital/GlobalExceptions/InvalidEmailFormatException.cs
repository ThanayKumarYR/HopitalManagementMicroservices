namespace UserHospital.GlobalExceptions
{
    public class InvalidEmailFormatException : Exception

    {
        public InvalidEmailFormatException(string message):base(message) { }
    }
}
