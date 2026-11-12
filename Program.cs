using System;
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
                Console.WriteLine("=== Сервісний центр ===");
                Console.WriteLine("1 — довідка про застосунок");
                Console.WriteLine("2 — таблиця вартості ремонту");
                Console.WriteLine("3 — завантаження майстрів за тиждень");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");


                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        ShowWorkloadTable();
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }


        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
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


        static void ShowCostTable()
        {
            const double rate = 220.0;       
            const double diagnostics = 150.0; 
            const double vat = 1.2;           

            double from = 0.5; 
            double to = 5.0;  
            double step = 0.5;  

            Console.WriteLine("Вартість ремонту залежно від тривалості роботи");
            Console.WriteLine("{0,8}{1,14}{2,14}", "Години", "Робота, грн", "Разом, грн");
            Console.WriteLine(new string('-', 36));

            for (double h = from; h <= to + 1e-9; h += step)
            {
                double work = rate * h;                    
                double total = diagnostics + work * vat;   
                Console.WriteLine("{0,8:F2}{1,14:F2}{2,14:F2}", h, work, total);
            }
        }

        static void ShowWorkloadTable()
        {
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };

            int rows = ReadPositiveInt("Кількість майстрів (1-20): ", 20);
            if (rows < 0)
            {
                Console.WriteLine("Помилка: потрібне ціле число від 1 до 20. Таблицю не побудовано.");
                return;
            }

            int cols = ReadPositiveInt("Кількість днів (1-7): ", 7);
            if (cols < 0)
            {
                Console.WriteLine("Помилка: потрібне ціле число від 1 до 7. Таблицю не побудовано.");
                return;
            }

            int[] colSums = new int[cols]; 
            int grandTotal = 0;            
            int lineWidth = 12 + 8 * (cols + 1);

            Console.WriteLine();
            Console.WriteLine("Завантаження майстрів (годин на день)");

 
            Console.Write("{0,-12}", "Майстер");
            for (int col = 0; col < cols; col++)
            {
                Console.Write("{0,8}", days[col]);
            }
            Console.WriteLine("{0,8}", "Разом");
            Console.WriteLine(new string('-', lineWidth));

  
            for (int row = 1; row <= rows; row++)
            {
                int rowSum = 0; 
                Console.Write("{0,-12}", "Майстер " + row);

                for (int col = 1; col <= cols; col++)
                {
                    int hours = (row * 3 + col * 5) % 6 + 2; 
                    rowSum += hours;
                    colSums[col - 1] += hours;
                    Console.Write("{0,8}", hours);
                }

                grandTotal += rowSum;
                Console.WriteLine("{0,8}", rowSum);
            }


            Console.WriteLine(new string('-', lineWidth));
            Console.Write("{0,-12}", "Разом");
            for (int col = 0; col < cols; col++)
            {
                Console.Write("{0,8}", colSums[col]);
            }
            Console.WriteLine("{0,8}", grandTotal);
        }

        static int ReadPositiveInt(string prompt, int max)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();

            if (int.TryParse(text, out int value) && value >= 1 && value <= max)
            {
                return value;
            }
            return -1;
        }
    }
}