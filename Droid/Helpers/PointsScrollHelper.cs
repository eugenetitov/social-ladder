using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SocialLadder.Droid.Activities.Main;

namespace SocialLadder.Droid.Helpers
{
    public class PointsScrollHelper
    {
        private bool WasCreate { get; set; } = false;
        private ScrollView scrollView { get; set; }
        private Type CurrentViewType { get; set; }

        public PointsScrollHelper(ScrollView scrollView, bool wasCreate, Type type)
        {
            this.scrollView = scrollView;
            WasCreate = wasCreate;
            CurrentViewType = type;
        }

        public void RemoveEvents()
        {
            scrollView.ScrollChange -= ScrollView_ScrollChange;
            MainActivity.ScrollHandler -= ScrollHandler;
        }

        public void AddEvents()
        {
            scrollView.ScrollChange += ScrollView_ScrollChange;
            MainActivity.ScrollHandler += ScrollHandler;
        }

        private void ScrollHandler(object sender, View.ScrollChangeEventArgs e)
        {
            if ((sender as Type) == CurrentViewType)
            {
                return;
            }
            scrollView.ScrollTo(e.ScrollX, e.ScrollY);
        }

        private void ScrollView_ScrollChange(object sender, View.ScrollChangeEventArgs e)
        {
            if (!WasCreate)
            {
                scrollView.ScrollTo(0, MainActivity.ScrollY);
                WasCreate = true;
                return;
            }
            MainActivity.PointsScrolling(CurrentViewType, e);
        }
    }
}