#region

using HBD.Mef.Shell;
using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.Navigation;

#endregion

namespace HBD.WPF
{
    public static class MenuExtensions
    {
        public static MenuTitleInfo AddTitle(this IMenuInfoCollection @this, string title)
        {
            var m = new MenuTitleInfo(@this) {Title = title};
            @this.Add(m);
            return m;
        }

        public static NavigationInfo AndNavigation(this MenuTitleInfo @this, string title)
            => @this.Parent.AddNavigation(title);

        public static MenuInfo AndMenu(this MenuTitleInfo @this, string title)
            => @this.Parent.Menu(title);

        public static IMenuInfoCollection AndSeparator(this MenuTitleInfo @this)
        {
            @this.Parent.AddSeparator();
            return @this.Parent;
        }
    }
}