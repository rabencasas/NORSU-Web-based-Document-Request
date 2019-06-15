using System;
using System.Collections.Generic;

namespace NGODP.Models
{
    public partial class Request
    {
        public string Refno { get; set; }
        public string Student { get; set; }
        public string Filedate { get; set; }
        public string Type { get; set; }
        public string Purpose { get; set; }
        public string Releasedate { get; set; }
        public string Lacking { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string Amount { get; set;}
        public string Feedback { get; set; }
        public string Notification { get; set; }
    }

    public class RequestViewModel
    {
        public string Refno { get; set; }
        public string Student { get; set; }
        public string StudentId {get; set; }
        public string Filedate { get; set; }
        public string Type { get; set; }
        public string Purpose { get; set; }
        public string Releasedate { get; set; }
        public string Lacking { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }

        public string Course { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }

        public string Amount { get; set;}
        public string Feedback { get; set; }

        public string Notification { get; set; }    
    }
}
