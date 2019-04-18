using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalAreaModel>;

namespace SocialLadder.Droid.Adapters
{
    public class AreasTableAdapter : BaseAdapter
    {
        private Dictionary<string, int> sectionPosition;
        private MainActivity _activity;
        public AreasTableAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            sectionPosition = new Dictionary<string, int>();
            _activity = context as MainActivity;
        }

        protected override void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsSourceCollectionChanged(sender, e);
        }

        protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            base.BindBindableView(source, viewToUse);
            
            var sctionView = viewToUse.Content.FindViewById<LinearLayout>(Resource.Id.section_view);
            var sectionText = viewToUse.Content.FindViewById<TextView>(Resource.Id.section_title);
            var oval = viewToUse.Content.FindViewById<ImageView>(Resource.Id.ic_oval);
            sctionView.Visibility = ViewStates.Gone;
            if (sectionPosition.ContainsKey("my_areas") && sectionPosition["my_areas"] == (source as LocalAreaModel).areaID)
            {
                sectionText.Text = "My Areas";
                sctionView.Visibility = ViewStates.Visible;
            }
            if (sectionPosition.ContainsKey("suggested_areas") && sectionPosition["suggested_areas"] == (source as LocalAreaModel).areaID)
            {
                sectionText.Text = "Suggested Areas";
                sctionView.Visibility = ViewStates.Visible;
            }
            if (!(source as LocalAreaModel).IsSuggestedArea && !sectionPosition.ContainsKey("my_areas"))
            {
                sectionText.Text = "My Areas";
                sctionView.Visibility = ViewStates.Visible;
                sectionPosition.Add("my_areas", (source as LocalAreaModel).areaID);
            }
            if ((source as LocalAreaModel).IsSuggestedArea && !sectionPosition.ContainsKey("suggested_areas"))
            {
                sectionText.Text = "Suggested Areas";
                sctionView.Visibility = ViewStates.Visible;
                sectionPosition.Add("suggested_areas", (source as LocalAreaModel).areaID);
            }
            if (!(source as LocalAreaModel).IsSuggestedArea)
            {
                oval.SetColorFilter(Android.Graphics.Color.ParseColor("#22F3D1"));
            }
            if ((source as LocalAreaModel).IsSuggestedArea)
            {
                oval.SetColorFilter(Android.Graphics.Color.ParseColor("#F2FA98"));
            }
            UpdateControls(sectionText, viewToUse.Content.FindViewById<TextView>(Resource.Id.description_text), viewToUse.Content.FindViewById<TextView>(Resource.Id.title_text));
        }

        private void UpdateControls(TextView sectionText, TextView descriptionText, TextView titleText)
        {
            FontHelper.UpdateFont(sectionText, FontsConstants.PN_B, (float)0.04);
            FontHelper.UpdateFont(descriptionText, FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(titleText, FontsConstants.PN_R, (float)0.055);
        }


    }
}