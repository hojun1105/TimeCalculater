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
            var loginViewModel = new LoginViewModel();
            var loginView = new LoginView
            {
                DataContext = loginViewModel, 
                Width = 300, 
                Height = 150, 
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            loginView.Show();

        }
    }
}
