using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.Core;
using System;

namespace HBD.WPF.Shell
{
    public static class NotificationExtensions
    {
        private static T For<T>(this T @this, INavigationParameter navigationParameter) where T : INotificationInfo
        {
            @this.NavigationParameter = navigationParameter;
            return @this;
        }

        public static T For<T>(this T @this, Action action) where T : INotificationInfo
            => @this.For(new ActionNavigationParameter(action));

        public static T For<T>(this T @this, string viewName, Type viewType) where T : INotificationInfo
           => @this.For(new ViewInfo(viewName, viewType));

        public static T For<T>(this T @this, string viewName) where T : INotificationInfo
          => @this.For(new ViewInfo(viewName));

        public static T For<T>(this T @this, Type viewType) where T : INotificationInfo
           => @this.For(new ViewInfo(viewType));

        public static T For<T, ViewType>(this T @this) where T : INotificationInfo
          => @this.For(typeof(ViewType));
    }
}
