using Lab3.Models;

namespace Lab3
{
    public static class MockDataGenerator
    {
        private static string[] firstNames = { "Іван", "Петро", "Олена", "Василь", "Катерина", "Дмитро", "Остап", "Валентина" };
        private static string[] lastNames = { "Шевченко", "Петренко", "Шевчук", "Іваненко", "Бондарчук", "Іванчук", "Ткач" };
        
        private static Random rnd = new Random();

        public static List<Person> GeneratePeople(int count)
        {
            var list = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                string fn = firstNames[rnd.Next(firstNames.Length)];
                string ln = lastNames[rnd.Next(lastNames.Length)];
                string email = fn.ToLower() + i + "@example.com";
                DateTime bd = DateTime.Today.AddDays(-rnd.Next(365 * 50));

                var p = new Person(fn, ln, email, bd);
                list.Add(p);
            }
            return list;
        }
    }
}
