using System.Windows;

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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        
        var crawler = new Crawler(((LoginViewModel)DataContext).Id, ((LoginViewModel)DataContext).Password);
        var data = crawler.Crawl();
        Close();
        var window = new TimeCalculatorView { DataContext = new TimeCalculatorViewModel(data) };
        window.Show();
        
    }
}