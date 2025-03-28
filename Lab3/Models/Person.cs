using Lab3.Exceptions;
using System.Text.RegularExpressions;

namespace Lab3.Models
{
    public class Person
    {
        private string _email;
        private DateTime _dateOfBirth;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email 
        {
            get => _email;
            set
            {
                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new InvalidEmailException();
                _email = value;
            }
        }
        public DateTime BirthDate
        {
            get => _dateOfBirth;
            set
            {
                if (value > DateTime.Now)
                    throw new FutureDateOfBirthException();
                if (value < DateTime.Now.AddYears(-135)) 
                    throw new TooOldDateOfBirthException();
                _dateOfBirth = value;
            }
        }

        public bool IsAdult { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = string.IsNullOrWhiteSpace(email) ? throw new InvalidEmailException() : email;
            BirthDate = birthDate;

            CalculateProperties();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.MinValue) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate) { }

        private void CalculateProperties()
        {
            int age = DateTime.Today.Year - BirthDate.Year;
            if (BirthDate.Date > DateTime.Today.AddYears(-age))
                age--;

            IsAdult = age >= 18;
            IsBirthday = BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month;

            SunSign = ZodiacHelper.GetWesternZodiac(BirthDate);
            ChineseSign = ZodiacHelper.GetChineseZodiac(BirthDate.Year);
        }
    }
}