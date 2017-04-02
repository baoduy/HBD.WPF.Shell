﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Prism.Common;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.Regions
{
    public class ScopedRegionNavigationContentLoader : IRegionNavigationContentLoader
    {
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionNavigationContentLoader" /> class with
        ///     a service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public ScopedRegionNavigationContentLoader(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        /// <summary>
        ///     Gets the view to which the navigation request represented by
        ///     <paramref
        ///         name="navigationContext" />
        ///     applies.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="navigationContext">The context representing the navigation request.</param>
        /// <returns>The view to be the target of the navigation request.</returns>
        /// <remarks>
        ///     If none of the views in the region can be the target of the navigation request, a new
        ///     view is created and added to the region.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     when a new view cannot be created for the navigation request.
        /// </exception>
        public object LoadContent([NotNull]IRegion region, [NotNull]NavigationContext navigationContext)
        {
            Guard.ArgumentIsNotNull(region, nameof(region));
            Guard.ArgumentIsNotNull(navigationContext, nameof(navigationContext));

            var candidateTargetContract = GetContractFromNavigationContext(navigationContext);
            var candidates = GetCandidatesFromRegion(region, candidateTargetContract);

            var acceptingCandidates =
                candidates.Where(
                    v =>
                    {
                        var navigationAware = v as INavigationAware;
                        if (navigationAware != null && !navigationAware.IsNavigationTarget(navigationContext))
                            return false;

                        var frameworkElement = v as FrameworkElement;
                        if (frameworkElement == null)
                            return true;

                        navigationAware = frameworkElement.DataContext as INavigationAware;
                        return navigationAware == null || navigationAware.IsNavigationTarget(navigationContext);
                    });

            var view = acceptingCandidates.FirstOrDefault();

            if (view != null)
                return view;

            view = CreateNewRegionItem(candidateTargetContract);
            if (region.Views.Contains(view)) return view;

            var viewHasScopedRegions = view.CastAs<IScopeRegion>();
            if (viewHasScopedRegions != null && viewHasScopedRegions.CreateScopeRegionManager)
            {
                var newRegionManager = region.Add(view, null, true);
                viewHasScopedRegions.ScopeRegionManager = newRegionManager;
            }
            else region.Add(view, null, false);

            return view;
        }

        /// <summary>
        ///     Provides a new item for the region based on the supplied candidate target contract name.
        /// </summary>
        /// <param name="candidateTargetContract">The target contract to build.</param>
        /// <returns>An instance of an item to put into the <see cref="IRegion" />.</returns>
        protected virtual object CreateNewRegionItem(string candidateTargetContract)
        {
            object newRegionItem;
            try
            {
                newRegionItem = _serviceLocator.GetInstance<object>(candidateTargetContract);
            }
            catch (ActivationException)
            {
                throw new InvalidOperationException("Cannot create navigation target");
            }
            return newRegionItem;
        }

        /// <summary>
        ///     Returns the candidate TargetContract based on the <see cref="NavigationContext" />.
        /// </summary>
        /// <param name="navigationContext">The navigation contract.</param>
        /// <returns>
        ///     The candidate contract to seek within the <see cref="IRegion" /> and to use, if not found,
        ///     when resolving from the container.
        /// </returns>
        protected virtual string GetContractFromNavigationContext(NavigationContext navigationContext)
        {
            if (navigationContext == null) throw new ArgumentNullException(nameof(navigationContext));

            var candidateTargetContract = UriParsingHelper.GetAbsolutePath(navigationContext.Uri);
            candidateTargetContract = candidateTargetContract.TrimStart('/');
            return candidateTargetContract;
        }

        /// <summary>
        ///     Returns the set of candidates that may satisfiy this navigation request.
        /// </summary>
        /// <param name="region">The region containing items that may satisfy the navigation request.</param>
        /// <param name="candidateNavigationContract">
        ///     The candidate navigation target as determined by <see cref="GetContractFromNavigationContext" />
        /// </param>
        /// <returns>An enumerable of candidate objects from the <see cref="IRegion" /></returns>
        protected virtual IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            return region.Views.Where(v =>
                string.Equals(v.GetType().Name, candidateNavigationContract, StringComparison.Ordinal) ||
                string.Equals(v.GetType().FullName, candidateNavigationContract, StringComparison.Ordinal));
        }
    }
}