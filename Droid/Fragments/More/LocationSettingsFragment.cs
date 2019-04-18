using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.More;

namespace SocialLadder.Droid.More
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class LocationSettingsFragment : BaseFragment<LocationSettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.LocationSettingsLayout;
        protected override bool HasBackButton => true;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var locationText = view.FindViewById<TextView>(Resource.Id.txtLocation);
            var onText = view.FindViewById<TextView>(Resource.Id.txtLocationStatus);
            var descriptionText = view.FindViewById<TextView>(Resource.Id.txtDescription);
            var descriptionText2 = view.FindViewById<TextView>(Resource.Id.txtDescription2);

            FontHelper.UpdateFont(locationText, FontsConstants.SFP_M, (float)0.039);
            FontHelper.UpdateFont(onText, FontsConstants.SFP_M, (float)0.037);
            FontHelper.UpdateFont(descriptionText, FontsConstants.SFP_R, (float)0.033);
            FontHelper.UpdateFont(descriptionText2, FontsConstants.SFP_R, (float)0.033);


            return view;
        }
    }
}