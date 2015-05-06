using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    public class LinkModel
    {
        public string LinkText { get; set; }
        public string Link { get; set; }

        public LinkModel(string linkText, string link)
        {
            LinkText = linkText;
            Link = link;
        }

    }

    public class MenuLinkModel
    {
        public int Id { get; set; }
        public bool IsAdminMenu { get; set; }
        public LinkModel Link { get; set; }
        public bool IsActive { get; set; }
        public List<LinkModel> ChildLinks { get; set; }
    }
}