namespace Lab3.Exceptions
{
    public class FutureDateOfBirthException : Exception
    {
        public FutureDateOfBirthException() : base("Дата народження не може бути в майбутньому.") { }
    }
}
