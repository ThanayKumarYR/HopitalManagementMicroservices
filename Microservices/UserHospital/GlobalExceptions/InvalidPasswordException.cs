﻿namespace UserHospital.GlobalExceptions
{
    public class InvalidPasswordException :Exception
    {
        public InvalidPasswordException(string message):base(message) { }
    }
}
