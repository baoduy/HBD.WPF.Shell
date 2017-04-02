#region



#endregion

using System;
using System.Windows;
using System.Windows.Input;
using HBD.Framework.Core;
using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.Navigation;
using Microsoft.Expression.Interactivity.Core;

namespace  HBD.WPF.Shell
{
    public static class NavigationExtensions
    {
        public static NavigationParameterCollector<INavigationInfo> ForCommand(this INavigationInfo @this, ICommand command, object parameter = null)
        {
            Guard.ArgumentIsNotNull(@this, typeof(INavigationInfo).Name);
            Guard.ArgumentIsNotNull(command, nameof(command));

            @this.NavigationParameters.Add(new CommandNavigationParameter(command, parameter));
            return new NavigationParameterCollector<INavigationInfo>(@this);
        }

        public static NavigationParameterCollector<INavigationInfo> ForCommand(this INavigationInfo @this, Action action)
           => @this.ForCommand(new ActionCommand(action));

        private static NavigationParameterCollector<INavigationInfo> ForRegion(this INavigationInfo @this, string regionName, string viewName, Type viewType)
        {
            Guard.ArgumentIsNotNull(@this, typeof(INavigationInfo).Name);

            @this.NavigationParameters.Add(new RegionNavigationParameter(regionName, viewName, viewType));
            return new NavigationParameterCollector<INavigationInfo>(@this);
        }

        public static NavigationParameterCollector<INavigationInfo> ForRegion(this INavigationInfo @this, string regionName, string viewName)
            => @this.ForRegion(regionName, viewName, null);

        public static NavigationParameterCollector<INavigationInfo> ForRegion(this INavigationInfo @this, string regionName, Type viewType)
           => @this.ForRegion(regionName, null, viewType);

        public static NavigationParameterCollector<INavigationInfo> ForRegion(this INavigationInfo @this, string viewName)
            => @this.ForRegion(null, viewName, null);

        public static NavigationParameterCollector<INavigationInfo> ForRegion(this INavigationInfo @this, Type viewType)
           => @this.ForRegion(null, null, viewType);

        public static NavigationParameterCollector<INavigationInfo> For<TViewType>(this INavigationInfo @this) where TViewType : UIElement
          => @this.ForRegion(null, null, typeof(TViewType));

        //public static T ForAction<T>(this T @this, Action action) where T : NavigationParameterCollection
        //{ }
        //public static T AndValidFor<T>(this T @this, string[] operations = null, string[] roles = null,
        //    string[] groups = null, string[] scopes = null) where T : IMenuInfo
        //=> @this.AndValidFor(new PermissionValidationInfo
        //{
        //    Operation = operations,
        //    Roles = roles,
        //    Groups = groups,
        //    Scopes = scopes
        //});

        //public static T AndValidForRoles<T>(this T @this, params string[] roles) where T : IMenuInfo
        //=> @this.AndValidFor(roles: roles);

        //public static T AndValidForOperations<T>(this T @this, params string[] operations) where T : IMenuInfo
        //=> @this.AndValidFor(operations);

        //public static T AndValidForGroups<T>(this T @this, params string[] groups) where T : IMenuInfo
        //=> @this.AndValidFor(groups: groups);

        //public static T AndValidForScopes<T>(this T @this, params string[] scopes) where T : IMenuInfo
        //=> @this.AndValidFor(scopes: scopes);
    }
}