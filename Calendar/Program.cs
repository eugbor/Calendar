using System;

namespace Calendar
{
    class Program
    {
        public static void Main()
        {
            string strDate = Console.ReadLine();
            Console.WriteLine();

            try
            {
                Calendar date = new Calendar(strDate);
                date.Show();
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат ввода даты");
            }

            Console.ReadKey();
        }
    }
}

