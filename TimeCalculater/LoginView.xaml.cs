using System.Windows;
using System.Windows.Input;

namespace TimeCalculator;

/// <summary>
///     LoginPage.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginView
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        var dataContext = (LoginViewModel)DataContext;
        if (string.IsNullOrEmpty(dataContext.Id) || string.IsNullOrEmpty(dataContext.Password))
        {
            return;
        }
        if (e.Key == Key.Enter)
        {
            ButtonBase_OnClick(sender, e);
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var dataContext = (LoginViewModel)DataContext;
        if (string.IsNullOrEmpty(dataContext.Id) || string.IsNullOrEmpty(dataContext.Password))
        {
            return;
        }
        var crawler = new Crawler(dataContext.Id, ((LoginViewModel)DataContext).Password);
        var data = crawler.Crawl();
        
        var window = new TimeCalculatorView 
        { 
            DataContext = new TimeCalculatorViewModel(data),
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        window.Show();
        Close();
    }
}