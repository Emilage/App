﻿using TravelDarkTheme.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace TravelDarkTheme
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class Spoke : TravelDarkTheme.Common.LayoutAwarePage
    {
        public Spoke()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            string[] strArray = ((String)navigationParameter).Split('|');
            var SpokeDataGroups = SpokeDataSource.GetGroups(strArray[0]);
            this.DefaultViewModel["Groups"] = SpokeDataGroups;

            this.pageTitle.Text = strArray[1];

            EnableLiveTile.CreateLiveTile.ShowliveTile(false, "Travel Dark Theme");
        }

        private void itemGridView_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailPage), "AllGroups");
        }

        private void itemGridView_Tapped_1(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailPage), "AllGroups");
        }

    }
}
