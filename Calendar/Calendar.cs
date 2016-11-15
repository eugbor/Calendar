using System;
using System.Text.RegularExpressions;

namespace Calendar
{
    class Calendar
    {
        private const int column = 6;
        private const int row = 7;

        private DateTime _date;

        private int[,] _calendar;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="date"></param>
        public Calendar(string date)
        {
            if (IsDateValid(date))
            {
                this._date = DateTime.Parse(date);
                Initialize();
            }
            else this._date = DateTime.Now;
        }

        /// <summary>
        /// Проверяет входящую дату
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>True</returns>
        public bool IsDateValid(string date)
        {
            string regex =
                @"^(?:(?:31(\/|-|\.|\ )(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.|\ )(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})
                $|^(?:29(\/|-|\.|\ )0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))
                $|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.|\ )(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";
            
            if (!Regex.IsMatch(date, regex))
            {
                throw new FormatException();
            }

            return true;
        }

        /// <summary>
        /// Заполняет двумерный массив
        /// </summary>
        private void Initialize()
        {
            // разница между началом одномерного массива и началом месяца
            var diff = (int)new DateTime(_date.Year, _date.Month, 1).DayOfWeek - 1;
            if (diff < 0)
                diff = (int)DaysAndNumbers.CountFullWeek - 1;

            // заполнение с учетом разницы одномерного массива 
            int[] myArray = new int[45];
            for (var i = 0; i < DateTime.DaysInMonth(_date.Year, _date.Month); i++)
                myArray[i + diff] = i + 1;

            // заполнение двумерного массива
            _calendar = new int[row, column];
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    _calendar[j, i] = myArray[i * row + j];

                }
            }
        }

        /// <summary>
        /// Выводит на консоль
        /// </summary>
        public void Show()
        {
            string[] dayOfWeek = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };

            for (int j = 0; j < row; ++j)
            {
                Console.Write(dayOfWeek[j]);

                for (int i = 0; i < column; ++i)
                {
                    // вывод искомого числа в скобках и числа, в этой же строке после искомого
                    if (_calendar[j, i] == _date.Day)
                    {
                        // если за искомым числом в этой же строке идет число
                        if (_calendar[j, i + 1] == _date.Day + (int)DaysAndNumbers.CountFullWeek)
                        {
                            // если искомое число однозначное
                            if (_date.Day < (int)DaysAndNumbers.SmallestTwoFigureNumber)
                            {
                                if (i == 0) // первая колонка
                                {
                                    Console.Write(_calendar[j, i + 1] < (int)DaysAndNumbers.SmallestTwoFigureNumber

                                        // если число, идущее за искомым, однозначное
                                        ? string.Join("", ".[.", _calendar[j, i] + "]..." + _calendar[j, i + 1])

                                        // если число, идущее за искомым, двузначное
                                        : string.Join("", ".[.", _calendar[j, i] + "].." + _calendar[j, i + 1]));
                                }
                                else// вторая колонка
                                    // если число, идущее за искомым, двузначное 
                                    Console.Write(string.Join("", "..[.", _calendar[j, i] + "].." + _calendar[j, i + 1]));
                            }
                            else Console.Write(string.Join("", "..[", _calendar[j, i] + "].." + _calendar[j, i + 1]));
                        }
                        else Console.Write(string.Join("", "..[", _calendar[j, i] + "]"));
                    }

                    // вывод многоточий
                    else
                    {
                        // если число в первой колонке равно 0
                        if (_calendar[j, i] == 0 && i == 0) 
                            Console.Write("....");
                        else
                        // условие исключающее вывод искомого числа в скобках и числа, в этой же строке после искомого
                        if (_calendar[j, i] != 0 && _calendar[j, i] != _date.Day + (int)DaysAndNumbers.CountFullWeek)
                        {
                                // однозначное число, идущее после дня недели
                            if (_calendar[j, i] < (int)DaysAndNumbers.SmallestTwoFigureNumber && i == 0 ||
                                // двузначное число
                                _calendar[j, i] > (int)DaysAndNumbers.BiggestOneFigureNumber)
                                Console.Write(string.Join("", "...", _calendar[j, i]));

                            else// однозначное число
                                Console.Write(string.Join("", "....", _calendar[j, i])); 
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
