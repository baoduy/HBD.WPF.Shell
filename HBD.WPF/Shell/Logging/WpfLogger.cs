using HBD.Mef.Logging;
using Prism.Logging;

namespace HBD.WPF.Shell.Logging
{
    public class WpfLogger : Log4NetLogger, ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    base.Log(message, LogCategory.Debug);
                    break;
                case Category.Exception:
                    base.Log(message, LogCategory.Exception);
                    break;
                case Category.Info:
                    base.Log(message, LogCategory.Info); break;
                case Category.Warn:
                    base.Log(message, LogCategory.Warn); break;
                default:
                    base.Log(message, LogCategory.Info);
                    break;
            }
        }
    }
}
