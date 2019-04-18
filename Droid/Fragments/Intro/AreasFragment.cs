using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Adapters;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Extensions;
using SocialLadder.Helpers;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Intro
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(IntroContainerViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame)]
    public class AreasFragment : BaseFragment<AreasCollectionViewModel>, IMvxOverridePresentationAttribute
    {
        public MvxBasePresentationAttribute PresentationAttribute()
        {
            MvxFragmentPresentationAttribute attr = new MvxFragmentPresentationAttribute();
            if (NavigationHelper.ShowAreasCollectionPageFromMainVM)
            {
                attr.ActivityHostViewModelType = typeof(MainViewModel);
                attr.FragmentContentId = Resource.Id.content_frame_full;
                attr.AddToBackStack = true;
            }
            if (!NavigationHelper.ShowAreasCollectionPageFromMainVM)
            {
                attr.ActivityHostViewModelType = typeof(IntroContainerViewModel);
                attr.FragmentContentId = Resource.Id.content_frame_full;
                attr.AddToBackStack = true;
            }
            return attr;
        }

        protected override int FragmentId => Resource.Layout.AreasLayout;
        protected override bool HasBackButton => true;

        private MvxListView areasList;
        private LinearLayout areasDetailsView;
        private ImageView areasDetailsIndicatorImage;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            areasList = view.FindViewById<MvxListView>(Resource.Id.areas_list);
            areasList.Adapter = new AreasTableAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext);
            areasDetailsView = view.FindViewById<LinearLayout>(Resource.Id.areas_details_view);
            areasDetailsIndicatorImage = view.FindViewById<ImageView>(Resource.Id.ic_oval);

            UndateControls();
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            if (NavigationHelper.ShowAreasCollectionPageFromMainVM)
            {
                view.SetBackgroundColor(Android.Graphics.Color.Black);
                //view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar).Visibility = ViewStates.Visible;         
            }
            NavigationHelper.ShowAreasCollectionPageFromMainVM = false;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.SelectedArea))
            {
                if (ViewModel.SelectedArea != null && !string.IsNullOrEmpty(ViewModel.SelectedArea.areaPrimaryColor))
                {
                    //areasDetailsIndicatorImage.SetColorFilter(Android.Graphics.Color.ParseColor(ViewModel.SelectedArea.areaPrimaryColor));
                    if (!ViewModel.SelectedArea.IsSuggestedArea)
                    {
                        areasDetailsIndicatorImage.SetColorFilter(Android.Graphics.Color.ParseColor("#22F3D1"));
                    }
                    if (ViewModel.SelectedArea.IsSuggestedArea)
                    {
                        areasDetailsIndicatorImage.SetColorFilter(Android.Graphics.Color.ParseColor("#F2FA98"));
                    }
                }
            }
        }


        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_areas_join), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_or), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<Button>(Resource.Id.invite_button), FontsConstants.PN_B, (float)0.04);

            FontHelper.UpdateFont(areasDetailsView.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_B, (float)0.055);
            FontHelper.UpdateFont(areasDetailsView.FindViewById<TextView>(Resource.Id.description), FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(areasDetailsView.FindViewById<TextView>(Resource.Id.subscribed_text), FontsConstants.PN_R, (float)0.035);
            FontHelper.UpdateFont(areasDetailsView.FindViewById<Button>(Resource.Id.btn_invite_code), FontsConstants.PN_R, (float)0.037);
        }
    }
}