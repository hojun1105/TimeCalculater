using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

            DayModels = new List<DayModel> { Monday, Tuesday, Wednesday, Thursday, Friday };
        }


        #region Properties

        public List<DayModel> DayModels { get; set; }


        public DayModel _Monday;
        public DayModel Monday 
        {
            get=>_Monday;
            set
            {
                if (value != _Monday)
                {
                    _Monday = value;
                    OnPropertyChanged(nameof(Monday));
                }
            }
        }

        public DayModel _Tuesday;
        public DayModel Tuesday
        {
            get => _Tuesday;
            set
            {
                if (value != _Tuesday)
                {
                    _Tuesday = value;
                    OnPropertyChanged(nameof(Tuesday));
                }
            }
        }

        public DayModel _Wednesday;
        public DayModel Wednesday
        {
            get => _Wednesday;
            set
            {
                if (value != _Wednesday)
                {
                    _Wednesday = value;
                    OnPropertyChanged(nameof(Wednesday));
                }
            }
        }

        public DayModel _Thursday;
        public DayModel Thursday
        {
            get => _Thursday;
            set
            {
                if (value != _Thursday)
                {
                    _Thursday = value;
                    OnPropertyChanged(nameof(Thursday));
                }
            }
        }

        public DayModel _Friday;
        public DayModel Friday
        {
            get => _Friday;
            set
            {
                if (value != _Friday)
                {
                    _Friday = value;
                    OnPropertyChanged(nameof(Friday));
                }
            }
        }

        public string _Memo;
        public string Memo
        {
            get => _Memo;
            set
            {
                if (value != Memo)
                {
                    _Memo = value;
                    OnPropertyChanged(nameof(Memo));
                }
            }
        }

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

        public string _LeftTime;
        public string LeftTime
        {
            get => _LeftTime; 
            set 
            {
                if(value != _LeftTime)
                {
                    _LeftTime = value;
                    OnPropertyChanged(nameof(LeftTime));
                }
            }
        }

        #region Method

        public void FillDayModels()
        {
            
            DayModels = new List<DayModel>{ Monday, Tuesday, Wednesday, Thursday, Friday };
            
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

        public void SplitAndSetMemo()
        {
            
            string[] delimiter= { "\n","\r\n","출근","퇴근" };
            List<string> splittedStringList = new List<string>();
            splittedStringList = Memo.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).ToList();
            splittedStringList.RemoveAt(0);
        
            for(int i =0; i<splittedStringList.Count; i++)
            {
                var quotient = i / 3;
                var remainder = i % 3;

                switch (remainder)
                {
                    case 0:
                        DayModels[quotient].StartTime = splittedStringList[i];
                        break;
                    case 1:
                        DayModels[quotient].EndTime = splittedStringList[i];
                        break;
                    default:
                        break;
                }
            }
        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
