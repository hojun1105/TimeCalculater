using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeCalculator;

namespace TimeCalculator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TimeCalculatorView : Window
    {
        public TimeCalculatorView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is TimeCalculatorViewModel viewModel)) return;
            viewModel.FillDayModels();
            viewModel.TimeCalculate();
            if (viewModel.Thursday.StartTime != null)
            {
                viewModel.TimeExpect(); 
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                DataContext = new TimeCalculatorViewModel();
                var viewModel = (TimeCalculatorViewModel)DataContext;
                viewModel.Memo = Clipboard.GetText();
                viewModel.SplitAndSetMemo();
            }
        }
    }

    
    #region Converter

    public class WorkTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string time)
            {
                return $"근무 시간 : {time}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LeftTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string time)
            {
                if (time.Contains("-"))
                {
                    var removedTime = time.Remove(time.IndexOf("-"),1);
                    return $"초과 시간 : {removedTime}";
                }
                return $"남은 시간 : {time}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DefaultTimeToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime date)
            {
                if(date == DateTime.MinValue)
                {
                    return string.Empty;
                }
                return date;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
