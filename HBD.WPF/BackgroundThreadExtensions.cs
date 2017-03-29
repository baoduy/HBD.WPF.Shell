#region

using System;
using System.ComponentModel;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF
{
    public static class BackgroundThreadExtensions
    {
        public static void RunWorker(this IBusyIndicator @this, Action<BackgroundWorker> doWorkAction,
            Action<int, object> reportAction = null)
        {
            var w = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            w.DoWork += (s, e) => doWorkAction.Invoke(w);

            w.ProgressChanged += (s, e) =>
            {
                @this.ShowBusy($"{e.ProgressPercentage} {e.UserState}");
                reportAction?.Invoke(e.ProgressPercentage, e.UserState);
            };

            w.RunWorkerCompleted += (s, e) =>
            {
                @this.HideBusy();
                ((BackgroundWorker) s).Dispose();
            };
            w.RunWorkerAsync();
        }
    }
}