using System;
using System.Globalization;

namespace PatientScheduleApp
{
    public class Appointment
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string PatientSurname { get; set; }

        public Appointment(DateTime date, TimeSpan time, string patientSurname)
        {
            if (string.IsNullOrWhiteSpace(patientSurname))
                throw new ArgumentException("Фамилия не может быть пустой.");

            Date = date.Date;
            Time = time;
            PatientSurname = patientSurname.Trim();
        }

        public override string ToString()
        {
            return $"{Date:dd.MM.yyyy} {Time:hh\\:mm} - {PatientSurname}";
        }
    }
}