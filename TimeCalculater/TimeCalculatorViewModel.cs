using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TimeCalculator
{
    public class TimeCalculatorViewModel : INotifyPropertyChanged
    {
        public TimeCalculatorViewModel()
        {
            Monday = new DayModel();
            Tuesday = new DayModel();
            Wednesday= new DayModel();
            Thursday= new DayModel();
            Friday= new DayModel();
            ExpectedFriday = new DayModel();

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

        public DayModel _ExpectedFriday;

        public DayModel ExpectedFriday
        {
            get => _ExpectedFriday;
            set
            {
                if (value != _ExpectedFriday)
                {
                    _ExpectedFriday = value;
                    OnPropertyChanged(nameof(ExpectedFriday));
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

        public TimeSpan LeftTimeSpan { get; set; }

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
                        //반차 처리
                        if (startTime.Hour is <= 14 and >= 13 || endTime.Hour is >= 13 and <= 14)
                        {
                            item.IsHalfDayOff = true;
                        }
                        if (startTime.Minute is >= 51 and <= 59)
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
            TimeSpan stackedTime = TimeSpan.Zero;
            foreach(var item in DayModels)
            {
                if (item.RoundedStartTime == DateTime.MinValue || item.RoundedEndTime == DateTime.MinValue) break;
                {
                    item.WorkDuration = item.RoundedEndTime - item.RoundedStartTime - TimeSpan.FromHours(1);
                    if (item.IsHalfDayOff)
                    {
                        item.WorkDuration += TimeSpan.FromHours(4);
                    }
                    stackedTime += item.WorkDuration;
                }
            }
            if(stackedTime == TimeSpan.Zero) { return; }
            WorkedTime = $"{stackedTime.Days * 24 + stackedTime.Hours}:{stackedTime.Minutes:00}";

            LeftTimeSpan = (TimeSpan.FromHours(40) - stackedTime);
            LeftTime = $"{LeftTimeSpan.Days * 24 + LeftTimeSpan.Hours}:{LeftTimeSpan.Minutes:00}";
        }

        //연월차 + 공휴일 처리간편하게 할거같아서 만들어놓음
        public void SplitAndSetMemo2()
        {
            string[] delimiter = { "\n", "\r\n", "출근", "퇴근" };
            var splitSegments = Memo.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).ToList();
            splitSegments.RemoveAt(0);

            splitSegments.RemoveAll(a => a.StartsWith("총") || a.Equals("휴무일") || a.Equals("휴일"));
            splitSegments = ManageDayOff(splitSegments);

            for (int i = 0; i < splitSegments.Count; i++)
            {
                var quotient = i / 2;
                var remainder = i % 2;
                switch (remainder)
                {
                    case 0:
                        DayModels[quotient].StartTime = splitSegments[i];
                        break;
                    case 1:
                        DayModels[quotient].EndTime = splitSegments[i];
                        break;
                }
            }
        }

        public void SplitAndSetMemo()
        {
            string[] delimiter= { "\n","\r\n","출근","퇴근" }; 
            var splitSegments = Memo.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).ToList();
            splitSegments.RemoveAt(0);
            splitSegments = ManageDayOff(splitSegments);
            splitSegments.RemoveAll(a => !DateTime.TryParse(a, out _));
            
            for(int i =0; i<splitSegments.Count; i++)
            {
                var quotient = i / 2;
                var remainder = i % 2;
                switch (remainder)
                {
                    case 0:
                        DayModels[quotient].StartTime = splitSegments[i];
                        break;
                    case 1:
                        DayModels[quotient].EndTime = splitSegments[i];
                        break;
                }
                
            }
        }

        private List<string> ManageDayOff(List<string> splitSegments)
        {
            if (splitSegments.Any(s => s.Equals("연차(종일)") || s.Equals("월차(종일)") || s.Equals("설날") || s.Equals("대체공휴일") || s.Equals("삼일절")))
            {
                var indices = splitSegments.Select((value, index) => new { value, index })
                    .Where(pair => pair.value is "연차(종일)" or "월차(종일)" or "설날" or "대체공휴일")
                    .Select(a => a.index).OrderByDescending(s => s).ToList();

                foreach (var index in indices)
                {
                    splitSegments.InsertRange(index, new[] { "09:00", "18:00" });
                }
            }

            return splitSegments;
        }


        public void TimeExpect()
        {
            if (Friday.StartTime != null)
            {
                ExpectedFriday.RoundedStartTime = Friday.RoundedStartTime;
                ExpectedFriday.StartTime = Friday.StartTime;
                ExpectedFriday.RoundedEndTime = Friday.RoundedStartTime + LeftTimeSpan + TimeSpan.FromHours(1);
                ExpectedFriday.EndTime = ExpectedFriday.RoundedEndTime.ToString("HH:mm");
            }
            else
            {
                var listTillThursday = DayModels.Where(a => a.EndTime != null).ToList();
                var averageTicks = listTillThursday.Select(a => a.RoundedStartTime).Select(date => date.TimeOfDay.Ticks).Average();
                TimeSpan averageTime = new TimeSpan((long)averageTicks);
                ExpectedFriday.StartTime = averageTime.ToString(@"hh\:mm");
                if (DateTime.TryParseExact(ExpectedFriday.StartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    if (startTime.Minute is >= 51 and <= 59)
                    {
                        ExpectedFriday.RoundedStartTime = new DateTime(1, 1, 1, startTime.Hour + 1, 0, 0);
                    }
                    else
                    {
                        var startTimeMinute = (int)(Math.Ceiling((double)(startTime.Minute) / 10)) * 10;
                        ExpectedFriday.RoundedStartTime = new DateTime(1, 1, 1, startTime.Hour, startTimeMinute, 0);
                    }
                }
                ExpectedFriday.RoundedEndTime = ExpectedFriday.RoundedStartTime + LeftTimeSpan + TimeSpan.FromHours(1);
                ExpectedFriday.EndTime = ExpectedFriday.RoundedEndTime.ToString("HH:mm");
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
