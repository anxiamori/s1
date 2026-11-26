using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace PatientScheduleApp
{
    public class PatientSchedule
    {
        private List<Appointment> appointments = new List<Appointment>();

        public bool AddAppointment(Appointment appointment)
        {
            DateTime newStart = appointment.Date + appointment.Time;
            DateTime newEnd = newStart.AddHours(1);

            foreach (var item in appointments)
            {
                DateTime oldStart = item.Date + item.Time;
                DateTime oldEnd = oldStart.AddHours(1);

                if (newStart < oldEnd && oldStart < newEnd)
                    return false;
            }

            appointments.Add(appointment);
            appointments = appointments
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)
                .ToList();
            return true;
        }

        public bool DeleteAppointment(DateTime date, TimeSpan time)
        {
            var appointment = appointments.FirstOrDefault(
                a => a.Date == date.Date && a.Time == time);

            if (appointment == null) return false;

            appointments.Remove(appointment);
            return true;
        }

        public void PrintAllSchedule()
        {
            if (appointments.Count == 0)
            {
                Console.WriteLine("Расписание пустое.");
                return;
            }

            foreach (var appointment in appointments)
                Console.WriteLine(appointment);
        }

        public void PrintScheduleForDay(DateTime date)
        {
            var daily = appointments
                .Where(a => a.Date == date.Date)
                .OrderBy(a => a.Time)
                .ToList();

            if (daily.Count == 0)
            {
                Console.WriteLine("На этот день записей нет.");
                return;
            }

            foreach (var appointment in daily)
                Console.WriteLine(appointment);
        }

        public void SaveToFile(string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (var appointment in appointments)
                    {
                        string dateStr = appointment.Date.ToString("yyyy-MM-dd");
                        string timeStr = appointment.Time.ToString(@"hh\:mm");
                        writer.WriteLine($"{dateStr}|{timeStr}|{appointment.PatientSurname}");
                    }
                }
                Console.WriteLine("Расписание сохранено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        public void LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            appointments.Clear();

            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            if (DateTime.TryParseExact(parts[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) &&
                                TimeSpan.TryParseExact(parts[1], @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan time))
                            {
                                Appointment appointment = new Appointment(date, time, parts[2]);
                                AddAppointment(appointment);
                            }
                        }
                    }
                }
                Console.WriteLine("Расписание загружено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}