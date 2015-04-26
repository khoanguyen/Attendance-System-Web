using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models.LogicModel
{
    public class ClassLogicModel
    {
        public ClassLogicModel() { }

        public ClassLogicModel(Class classObj, bool populateSession = true)
        {
            Id = classObj.Id;
            Name = classObj.Name;
            ProfessorName = classObj.ProfessorName;
            StartDate = classObj.StartDate;
            EndDate = classObj.EndDate;
            ExcusedTime = classObj.ExcusedTime;

            if (populateSession)
            {
                Sessions = new List<ClassSessionLogicModel>(
                    classObj.ClassSessions.Select(session => new ClassSessionLogicModel(session))
                    );
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfessorName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? ExcusedTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ClassSessionLogicModel> Sessions { get; set; }
    }
}