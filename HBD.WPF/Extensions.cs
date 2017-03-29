#region

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF
{
    public static class Extensions
    {
        public static double RealValue(this double @this) => double.IsNaN(@this) ? 0 : @this;

        public static void ShowBusy(this IBusyIndicator @this, string message = null)
        {
            if (@this == null) return;

            @this.IsBusy = true;
            @this.BusyMessage = message;
        }

        public static void HideBusy(this IBusyIndicator @this)
        {
            if (@this == null) return;

            @this.BusyMessage = string.Empty;
            @this.IsBusy = false;
        }

        //public static async Task SetBusyAsync(this IBusyIndicator @this, bool isBusy, string message = null)
        //    => await Functions.CreateTask(() => @this.SetBusy(isBusy, message));

        //public static ContentControl FindViewByViewModel(this IRegionManager @this, object viewModel)
        //  => (from r in @this.Regions
        //      from v in r.Views.OfType<ContentControl>()
        //      where v.DataContext == viewModel
        //      select v).FirstOrDefault();

        public static IEnumerable<T> FindChild<T>(this DependencyObject @this) where T : DependencyObject
        {
            if (@this == null) yield break;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(@this); i++)
            {
                var child = VisualTreeHelper.GetChild(@this, i);
                var findChild = child as T;
                if (findChild != null)
                    yield return findChild;

                foreach (var childOfChild in child.FindChild<T>())
                    yield return childOfChild;
            }
        }

        public static T FindParent<T>(this DependencyObject @this) where T : DependencyObject
        {
            while (true)
            {
                var p = VisualTreeHelper.GetParent(@this);
                if (p == null) return null;

                var parent = p as T;
                if (parent != null) return parent;

                @this = p;
            }
        }

        public static bool IsAddChildable(this DependencyObject @this)
        {
            if (@this == null) return false;
            if (@this is Border) return false;
            if (@this is AdornerDecorator) return false;
            return @this is IAddChild;
        }

        /// <summary>
        ///     Find the IAddChild that is parent of @this control.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DependencyObject FindAddChildableParent(this DependencyObject @this)
        {
            while (true)
            {
                var p = VisualTreeHelper.GetParent(@this);
                if (p == null) return null;

                if (p.IsAddChildable()) return p;
                @this = p;
            }
        }

        /// <summary>
        ///     Find IAddChild inside @this control.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DependencyObject FindAddChildable(this DependencyObject @this)
        {
            if (@this == null) return null;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(@this); i++)
            {
                var child = VisualTreeHelper.GetChild(@this, i);
                if (child.IsAddChildable())
                    return child;

                var c = child.FindAddChildable();
                if (c != null) return c;
            }

            return null;
        }

        /// <summary>
        ///     If @this is T then return @this as T. If DataContext of @this is then return DataContext
        ///     as T else return NULL;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T CastAs<T>(this object @this) where T : class
        {
            var t1 = @this as T;
            if (t1 != null) return t1;

            var fr = @this as FrameworkElement;
            return fr?.DataContext as T;
        }
    }
}