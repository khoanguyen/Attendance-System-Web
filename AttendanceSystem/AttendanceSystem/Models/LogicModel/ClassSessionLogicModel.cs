using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models.LogicModel
{
    public class ClassSessionLogicModel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek WeekDay { get; set; }        
        public string Room { get; set; }

        public ClassSessionLogicModel() { }
        public ClassSessionLogicModel(ClassSession session)
        {
            Id = session.Id;
            StartTime = session.StartTime;
            EndTime = session.EndTime;
            WeekDay = GetDayOfWeek(session.Weekday);
            Room = session.Room;
        }

        private DayOfWeek GetDayOfWeek(string input)
        {
            switch (input.Trim().ToLower())
            {
                case "mon": return DayOfWeek.Monday;
                case "tue": return DayOfWeek.Tuesday;
                case "wed": return DayOfWeek.Wednesday;
                case "thu": return DayOfWeek.Thursday;
                case "fri": return DayOfWeek.Friday;
                case "sat": return DayOfWeek.Saturday;
                default: return DayOfWeek.Sunday;
            }
        }
    }
}