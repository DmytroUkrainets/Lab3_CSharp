namespace Lab3.Models
{
    public static class ZodiacHelper
    {
        public static string GetWesternZodiac(DateTime birthDate)
        {
            int day = birthDate.Day;
            int month = birthDate.Month;

            return month switch
            {
                1 => (day <= 19) ? "Козеріг" : "Водолій",
                2 => (day <= 18) ? "Водолій" : "Риби",
                3 => (day <= 20) ? "Риби" : "Овен",
                4 => (day <= 19) ? "Овен" : "Телець",
                5 => (day <= 20) ? "Телець" : "Близнюки",
                6 => (day <= 20) ? "Близнюки" : "Рак",
                7 => (day <= 22) ? "Рак" : "Лев",
                8 => (day <= 22) ? "Лев" : "Діва",
                9 => (day <= 22) ? "Діва" : "Терези",
                10 => (day <= 22) ? "Терези" : "Скорпіон",
                11 => (day <= 21) ? "Скорпіон" : "Стрілець",
                12 => (day <= 21) ? "Стрілець" : "Козеріг",
                _ => "Невідомий"
            };
        }

        public static string GetChineseZodiac(int year)
        {
            string[] signs = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return signs[year % 12];
        }
    }
}
