using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Views;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class CollateralTrackingFragment : BaseFragment<CollateralTrackingViewModel>, View.IOnScrollChangeListener, IOnMapReadyCallback
    {
        protected override int FragmentId => Resource.Layout.ChallengesCollateralTrackingLayout;
        protected override bool HasBackButton => true;

        private ScrollView parallaxScrollView;
        private MapView parallaxMapView;
        private ImageView parallaxImage;
        private ImageButton button;
        private View view;
        private GoogleMap _map;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            parallaxScrollView = view.FindViewById<ScrollView>(Resource.Id.parallax_scroll_view);
            parallaxImage = view.FindViewById<ImageView>(Resource.Id.parallax_image);
            parallaxMapView = view.FindViewById<MapView>(Resource.Id.parallax_map_view);
            parallaxMapView.OnCreate(savedInstanceState);
            parallaxMapView.GetMapAsync(this);
            parallaxScrollView.SetOnScrollChangeListener(this);
            UndateControls();
            button = view.FindViewById<ImageButton>(Resource.Id.photo_button);
            PermissionHelper.AddPermission(Activity, new List<string> { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation });
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            parallaxMapView.OnStart();
        }

        public override void OnResume()
        {
            base.OnResume();
            parallaxMapView.OnResume();
            var challengeTriangle = view.FindViewById<TriangleChallengeView>(Resource.Id.challenge_popup_triangle);
            challengeTriangle.SetParameters(0, 0, ViewModel as BaseChallengesViewModel);
            challengeTriangle.Invalidate();
        }

        public override void OnStop()
        {
            base.OnStop();
            parallaxMapView.OnStop();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            parallaxMapView.OnDestroy();
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => (ViewModel as BaseChallengesViewModel).Challenge))
            {
                if ((ViewModel as BaseChallengesViewModel).Challenge != null && (ViewModel as BaseChallengesViewModel).Challenge.LocationLat != null && (ViewModel as BaseChallengesViewModel).Challenge.LocationLong != null && _map != null)
                {
                    //ViewModel.RequiredLocation = true;
                    GoogleMapHelper.UpdateMapZoom(_map, ViewModel.Challenge.LocationLat.Value, ViewModel.Challenge.LocationLong.Value, ViewModel.Challenge.RadiusMeters);
                }
                //else
                //    ViewModel.RequiredLocation = false;
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.SubmitButtonAnimated))
            {
                if (ViewModel.SubmitButtonAnimated)
                {
                    AnimateButton(button, Resource.Drawable.ic_loadingIndicator);
                }
                if (!ViewModel.SubmitButtonAnimated)
                {
                    button.SetImageResource(Resource.Drawable.challenges_postering_btn);
                    button.ClearAnimation();
                }
            }
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            parallaxMapView.SetY(parallaxMapView.Top - scrollY / 3);
            var mapParams = parallaxImage.LayoutParameters;
            mapParams.Width = DimensHelper.GetScreenWidth() + scrollY / 4;
            parallaxImage.LayoutParameters = mapParams;

            parallaxImage.SetY(parallaxImage.Top - scrollY / 3);
            var imageParams = parallaxImage.LayoutParameters;
            imageParams.Width = DimensHelper.GetScreenWidth() + scrollY / 4;
            parallaxImage.LayoutParameters = imageParams;
        }

        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_R, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.time), FontsConstants.PN_R, (float)0.035);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text), FontsConstants.PN_B, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_peopleCount_text), FontsConstants.PN_R, (float)0.045);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            try
            {
                _map = googleMap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}