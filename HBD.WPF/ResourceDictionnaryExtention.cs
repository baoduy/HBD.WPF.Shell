#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Windows;
using HBD.Framework;

#endregion

namespace HBD.WPF
{
    public static class ResourceDictionnaryExtention
    {
        private static readonly IDictionary<Assembly, WeakReference<ResourceDictionary>> LoadedAssemblies =
            new Dictionary<Assembly, WeakReference<ResourceDictionary>>();

        public static ResourceDictionary GetResourceDictionary(this Assembly @this)
        {
            if (@this == null) return null;
            ResourceDictionary resource = null;

            if (LoadedAssemblies.ContainsKey(@this))
            {
                LoadedAssemblies[@this].TryGetTarget(out resource);
                if (resource != null) return resource;
                LoadedAssemblies.Remove(@this);
            }

            foreach (var name in @this.GetManifestResourceNames())
            {
                var info = @this.GetManifestResourceInfo(name);
                if (info != null && info.ResourceLocation == ResourceLocation.ContainedInAnotherAssembly) continue;

                var resourceStream = @this.GetManifestResourceStream(name);

                if (resourceStream == null) continue;
                using (var reader = new ResourceReader(resourceStream))
                {
                    foreach (DictionaryEntry entry in reader)
                        try
                        {
                            var location =
                                    $"pack://application:,,,/{@this.GetName().Name};component/{entry.Key.ToString().Replace("baml", "xaml")}";
                            if (!location.EndsWith("xaml")) continue;
                            if (!location.AnyOfIgnoreCase("theme", "resource")) continue;

                            var rs = new ResourceDictionary {Source = new Uri(location)};
                            if (rs.Count <= 0) continue;

                            if (resource == null) resource = rs;
                            else resource.MergedDictionaries.Add(rs);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                }

            }

            LoadedAssemblies[@this] = new WeakReference<ResourceDictionary>(resource);
            return resource;
        }

        public static ResourceDictionary GetResourceDictionary<TControl>(this TControl @this)
            where TControl : class => @this?.GetType().Assembly.GetResourceDictionary();
    }
}