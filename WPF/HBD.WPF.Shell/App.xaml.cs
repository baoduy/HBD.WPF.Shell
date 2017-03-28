#region

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Threading;
using HBD.Mef.Logging;
using HBD.WPF.Shell.Services;
using Prism.Logging;

#endregion

namespace HBD.WPF.Shell
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IMessageBoxService MessageBoxService { get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILoggerFacade Logger { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
#endif
            new Bootstrapper().Run();
            Current.MainWindow.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            const string message = "Do you want to close this application?";
            const string title = "Closing Application Confirmation";

#if !DEBUG
            
            if (MessageBoxService == null)
            {
                if (MessageBox.Show(Application.Current.MainWindow, message, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
                    != MessageBoxResult.Yes)
                    e.Cancel = true;
            }
            else if (MessageBoxService.ConfirmDialog(App.Current.MainWindow, message, title).Result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
#endif
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            GlobalExceptionHandle(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            => GlobalExceptionHandle(e.ExceptionObject as Exception);

        private void GlobalExceptionHandle(Exception ex)
        {
            Logger.Exception(ex);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}