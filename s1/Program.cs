using System;
using System.Globalization;

namespace PatientScheduleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PatientSchedule schedule = new PatientSchedule();

            while (true)
            {
                Console.WriteLine("\n--- Расписание приема пациентов ---");
                Console.WriteLine("1. Добавить запись");
                Console.WriteLine("2. Удалить запись");
                Console.WriteLine("3. Показать всё расписание");
                Console.WriteLine("4. Показать расписание на день");
                Console.WriteLine("5. Сохранить расписание");
                Console.WriteLine("6. Загрузить расписание");
                Console.WriteLine("0. Выход");

                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите дату (дд.мм.гггг): ");
                        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        {
                            Console.WriteLine("Неверная дата.");
                            break;
                        }

                        Console.Write("Введите время (чч:мм): ");
                        if (!TimeSpan.TryParseExact(Console.ReadLine(), @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan time))
                        {
                            Console.WriteLine("Неверное время.");
                            break;
                        }

                        Console.Write("Введите фамилию пациента: ");
                        string surname = Console.ReadLine();

                        try
                        {
                            Appointment appointment = new Appointment(date, time, surname);
                            if (schedule.AddAppointment(appointment))
                                Console.WriteLine("Запись добавлена.");
                            else
                                Console.WriteLine("Это время уже занято.");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;

                    case "2":
                        Console.Write("Введите дату записи (дд.мм.гггг): ");
                        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime delDate))
                        {
                            Console.WriteLine("Неверная дата.");
                            break;
                        }

                        Console.Write("Введите время записи (чч:мм): ");
                        if (!TimeSpan.TryParseExact(Console.ReadLine(), @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan delTime))
                        {
                            Console.WriteLine("Неверное время.");
                            break;
                        }

                        if (schedule.DeleteAppointment(delDate, delTime))
                            Console.WriteLine("Запись удалена.");
                        else
                            Console.WriteLine("Запись не найдена.");
                        break;

                    case "3":
                        schedule.PrintAllSchedule();
                        break;

                    case "4":
                        Console.Write("Введите дату (дд.мм.гггг): ");
                        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                        {
                            Console.WriteLine("Неверная дата.");
                            break;
                        }
                        schedule.PrintScheduleForDay(searchDate);
                        break;

                    case "5":
                        schedule.SaveToFile("schedule.txt"); // Сохраняем в txt, так как формат не JSON
                        break;

                    case "6":
                        schedule.LoadFromFile("schedule.txt");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}