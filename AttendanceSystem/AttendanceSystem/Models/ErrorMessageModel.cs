using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    public class ErrorMessageModel
    {
        public string Error { get; set; }

        public string ErrorMessage { get; set; }

        public LinkModel ReturnLink { get; set; }
    }
}