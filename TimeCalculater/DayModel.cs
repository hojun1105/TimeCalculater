using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculater
{
    public class DayModel
    {
        public string StartTime {get;set;}
        public string EndTime {get; set;}
        public DateTime RoundedStartTime { get; set;}
        public DateTime RoundedEndTime { get; set;}
        public TimeSpan WorkDuration { get; set; }
    }
}
