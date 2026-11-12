using System;
using System.Globalization;
using System.Text;

namespace ServiceCenter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": RunCostTable(); Pause(); break;
                    case "3": RunWorkloadTable(); Pause(); break;
                    case "0": running = false; break;
                    default: ShowWrongChoice(); Pause(); break;
                }
            }

            Console.WriteLine("До побачення.");
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Сервісний центр ===");
            Console.WriteLine("1 — довідка про застосунок");
            Console.WriteLine("2 — таблиця вартості ремонту");
            Console.WriteLine("3 — завантаження майстрів за тиждень");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static void ShowWrongChoice()
        {
            Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static void PrintLine(int width)
        {
            Console.WriteLine(new string('-', width));
        }

        static void ShowAbout()
        {
            Console.WriteLine("Тема: облік заявок на ремонт ноутбуків.");
            Console.WriteLine("Головна сутність: заявка на ремонт.");
            Console.WriteLine("Поля запису (7):");
            Console.WriteLine("  1. Номер заявки      — int (призначає програма)");
            Console.WriteLine("  2. ПІБ клієнта       — string");
            Console.WriteLine("  3. Модель ноутбука   — string");
            Console.WriteLine("  4. Опис несправності — string");
            Console.WriteLine("  5. Дата прийому      — DateTime");
            Console.WriteLine("  6. Вартість ремонту  — double");
            Console.WriteLine("  7. Статус заявки     — string");
            Console.WriteLine("Підсумкова величина: загальна вартість усіх ремонтів.");
        }

        static double CalculateWorkCost(double hours, double rate)
        {
            return hours * rate;
        }

        static double CalculateTotalCost(double workCost, double diagnostics, double vatFactor)
        {
            return diagnostics + workCost * vatFactor;
        }

        static void RunCostTable()
        {
            double start = ReadDouble("Початок проміжку, год: ", 0.1, 24);
            double end = ReadDouble("Кінець проміжку, год: ", start, 24);
            double step = ReadDouble("Крок, год: ", 0.05, 5);

            Console.WriteLine();
            PrintCostTable(start, end, step);
        }

        static void PrintCostTable(double start, double end, double step)
        {
            const double rate = 220.0;
            const double diagnostics = 150.0;
            const double vat = 1.2;

            Console.WriteLine("Вартість ремонту залежно від тривалості роботи");
            Console.WriteLine("{0,8}{1,14}{2,14}", "Години", "Робота, грн", "Разом, грн");
            PrintLine(36);

            for (double h = start; h <= end + 1e-9; h += step)
            {
                double work = CalculateWorkCost(h, rate);
                double total = CalculateTotalCost(work, diagnostics, vat);
                Console.WriteLine("{0,8:F2}{1,14:F2}{2,14:F2}", h, work, total);
            }
        }

        static int CalculateWorkload(int master, int day)
        {
            return (master * 3 + day * 5) % 6 + 2;
        }

        static void RunWorkloadTable()
        {
            int rows = ReadInt("Кількість майстрів: ", 1, 20);
            int cols = ReadInt("Кількість днів: ", 1, 7);

            Console.WriteLine();
            PrintWorkloadTable(rows, cols);
        }

        static void PrintWorkloadTable(int rows, int cols)
        {
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };
            int[] colSums = new int[cols];
            int grandTotal = 0;
            int width = 12 + 8 * (cols + 1);

            Console.WriteLine("Завантаження майстрів (годин на день)");
            Console.Write("{0,-12}", "Майстер");
            for (int col = 0; col < cols; col++)
            {
                Console.Write("{0,8}", days[col]);
            }
            Console.WriteLine("{0,8}", "Разом");
            PrintLine(width);

            for (int row = 1; row <= rows; row++)
            {
                int rowSum = 0;
                Console.Write("{0,-12}", "Майстер " + row);

                for (int col = 1; col <= cols; col++)
                {
                    int hours = CalculateWorkload(row, col);
                    rowSum += hours;
                    colSums[col - 1] += hours;
                    Console.Write("{0,8}", hours);
                }

                grandTotal += rowSum;
                Console.WriteLine("{0,8}", rowSum);
            }

            PrintLine(width);
            Console.Write("{0,-12}", "Разом");
            for (int col = 0; col < cols; col++)
            {
                Console.Write("{0,8}", colSums[col]);
            }
            Console.WriteLine("{0,8}", grandTotal);
        }

        static bool TryParseDouble(string? text, out double value)
        {
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            {
                return true;
            }
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? text = Console.ReadLine();

                if (!TryParseDouble(text, out double value))
                {
                    Console.WriteLine("Помилка: введіть число.");
                    continue;
                }

                if (!(value >= min && value <= max))
                {
                    Console.WriteLine("Помилка: значення має бути від " + min + " до " + max + ".");
                    continue;
                }

                return value;
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? text = Console.ReadLine();

                if (!int.TryParse(text, out int value))
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine("Помилка: значення має бути від " + min + " до " + max + ".");
                    continue;
                }

                return value;
            }
        }
    }
}