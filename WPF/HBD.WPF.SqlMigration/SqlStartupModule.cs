#region

using System;
using System.Collections.Generic;
using System.Windows;
using HBD.Mef.Shell;
using HBD.Mef.Shell.Navigation;
using HBD.Mef.Shell.Services;
using HBD.WPF.Shell.Modularity;
using HBD.WPF.SqlMigration.Module.Resources;
using HBD.WPF.SqlMigration.Module.Views;
using Prism.Mef.Modularity;
using Prism.Modularity;

#endregion

namespace HBD.WPF.SqlMigration.Module
{
    [ModuleExport(typeof(SqlStartupModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class SqlStartupModule : ModuleBase
    {
        protected override IEnumerable<IViewInfo> GetStartUpViewTypes()
        {
            yield break;
        }

        public override void Initialize()
        {
            base.Initialize();

            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary
                {
                    Source =
                        new Uri("pack://application:,,,/HBD.WPF.SqlMigration.Module;component/Resources/Resources.xaml",
                            UriKind.Absolute)
                });
        }

        protected override void MenuConfiguration(IShellMenuService menuSet)
        {
            menuSet.Menu("Sql Tools")
                .WithIcon(GetResource(ResourceKeys.SqlIcon))
                .Children
                .AddTitle("Migration Scripts")
                .AndNavigation("Migration Script Generation");

            menuSet.Menu("Sql Tools")
                .Children
                .AddNavigation("Migration Script Generation")
                .WithIcon(GetResource(ResourceKeys.DatabaseIcon))
                .For(new ViewInfo(typeof(MigrationScriptView)));
        }
    }
}