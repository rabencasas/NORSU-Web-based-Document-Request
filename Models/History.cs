using System;
using System.Collections.Generic;

namespace NGODP.Models
{
    public class History
    {
        public string Timestamp { get; set; }
        public string Timedate { get; set; }
        public string Requestref { get; set; }
        public string Student { get; set; }
    }

    public class HistoryViewModel
    {
        public string Timestamp { get; set; }
        public string Timedate { get; set; }
        public string Requestref { get; set; }
        public string Student { get; set; }
        public string Type { get; set;}
        public string Amount { get; set; }
    }
}