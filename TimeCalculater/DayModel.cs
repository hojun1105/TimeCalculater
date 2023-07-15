using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculater
{
    public class DayModel : INotifyPropertyChanged
    {
        public string _StartTime;
        public string StartTime 
        {
            get => _StartTime;
            set 
            { 
                if (value != _StartTime)
                {
                    _StartTime = value;
                    OnPropertyChanged(nameof(StartTime));
                } 
            }
        }

        public string _EndTime;
        public string EndTime
        {
            get => _EndTime;
            set
            {
                if (value != _EndTime)
                {
                    _EndTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }
        public DateTime RoundedStartTime { get; set;}
        public DateTime RoundedEndTime { get; set;}
        public TimeSpan WorkDuration { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
