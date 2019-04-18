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

namespace SocialLadder.Droid.Helpers.RewardsCircleHelper
{
    public class ViewIdGenerator
    {
        static int id = 1;
        private Activity _parent;

        public ViewIdGenerator(Activity parent)
        {
            _parent = parent;
        }

        public int FindFreeId()
        {
            View view = _parent.FindViewById(id);
            while (view != null)
            {
                id++;
                view = _parent.FindViewById(id);
            }
            var tview = _parent.FindViewById(id);
            return id;
        }
    }
}