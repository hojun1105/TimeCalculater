using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculater
{
    public class TimeCalculaterViewModel : INotifyPropertyChanged
    {
        public TimeCalculaterViewModel()
        {
            Monday = new DayModel();
            Tuesday = new DayModel();
            Wednesday= new DayModel();
            Thursday= new DayModel();
            Friday= new DayModel();
        }

        #region Properties

        public List<DayModel> DayModels { get; set; }

       
        public DayModel Monday {get;set;}
        public DayModel Tuesday { get; set; }
        public DayModel Wednesday { get; set; }
        public DayModel Thursday { get; set; }
        public DayModel Friday { get; set; }

        #endregion 

        public string _WorkedTime;
        public string WorkedTime 
        { 
            get => _WorkedTime; 
            set 
            {
                if (value != _WorkedTime)
                {
                    _WorkedTime = value;
                    OnPropertyChanged(nameof(WorkedTime));
                }
            }
        }

        public string _leftTime;
        public string LeftTime
        {
            get => _leftTime; 
            set 
            {
                if(value != _leftTime)
                {
                    _leftTime = value;
                    OnPropertyChanged(nameof(LeftTime));
                }
            }
        }

        #region Method

        public void FillDayModels()
        {
            DayModels = new List<DayModel>();
            DayModels.AddRange(new List<DayModel> { Monday, Tuesday, Wednesday, Thursday, Friday });
            
            foreach (var item in DayModels)
            {
                if (item.StartTime != null && item.EndTime != null)
                {
                    if(DateTime.TryParseExact(item.StartTime ,"HH:mm" ,CultureInfo.InvariantCulture ,DateTimeStyles.None,out DateTime startTime)
                        && DateTime.TryParseExact(item.EndTime ,"HH:mm" ,CultureInfo.InvariantCulture ,DateTimeStyles.None ,out DateTime endTime))
                    {
                        if (startTime.Minute >= 51 && startTime.Minute <= 59)
                        {
                            item.RoundedStartTime = new DateTime(1, 1, 1, startTime.Hour + 1, 0, 0);
                        }
                        else
                        {
                            var startTimeMinute = (int)(Math.Ceiling((double)(startTime.Minute) / 10)) * 10;
                            item.RoundedStartTime = new DateTime(1, 1, 1, startTime.Hour, startTimeMinute, 0);
                        }

                        var endTimeMinute = (int)Math.Floor((double)endTime.Minute / 10) * 10;
                        item.RoundedEndTime = new DateTime(1, 1, 1, endTime.Hour, endTimeMinute, 0);
                    }
                }   
            }
        }

        public void TimeCalculate()
        {
            TimeSpan StackedTime = TimeSpan.Zero;
            foreach(var item in DayModels)
            {

                if (item.RoundedStartTime == DateTime.MinValue || item.RoundedEndTime == DateTime.MinValue) break;
                else
                {
                    item.WorkDuration = item.RoundedEndTime - item.RoundedStartTime - TimeSpan.FromHours(1);
                    StackedTime += item.WorkDuration;
                }
            }
            if(StackedTime == TimeSpan.Zero) { return; }
            WorkedTime = $"{StackedTime.Days * 24 + StackedTime.Hours}:{StackedTime.Minutes:00}";

            var leftTime = (TimeSpan.FromHours(40) - StackedTime);
            LeftTime = $"{leftTime.Days * 24 + leftTime.Hours}:{leftTime.Minutes:00}";
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
