using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EE.BM.View;

namespace EE.BM
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //set the main window here
            var mainWindow = new wdLogin();
            //var mainWindow = new wdMain();
            Application.Current.MainWindow = mainWindow;

            LoginViewModel loginViewModel = new LoginViewModel();
            //ReceiptViewModel viewmodel = new ReceiptViewModel();
            //set data context here
            mainWindow.DataContext = loginViewModel;

            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }

        
    }


}
