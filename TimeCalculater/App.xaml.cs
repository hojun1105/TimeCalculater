using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimeCalculator;

namespace TimeCalculater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView{DataContext = new LoginViewModel(), Width = 200, Height = 100, WindowStartupLocation = WindowStartupLocation.CenterScreen};
            loginView.Show();

        }
    }
}
