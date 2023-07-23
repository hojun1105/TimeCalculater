using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculator
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

        public DateTime _RoundedStartTime;
        public DateTime RoundedStartTime 
        {
            get => _RoundedStartTime;
            set
            {
                if (value != _RoundedStartTime)
                {
                    _RoundedStartTime = value;
                    OnPropertyChanged(nameof(RoundedStartTime));
                }
            }
        }

        public DateTime _RoundedEndTime;
        public DateTime RoundedEndTime 
        { 
            get => _RoundedEndTime;
            set
            {
                if(value != _RoundedEndTime)
                {
                    _RoundedEndTime= value;
                    OnPropertyChanged(nameof(RoundedEndTime));
                }
            }
        }

        public bool IsHalfDayOff { get; set; }
        public TimeSpan WorkDuration { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
