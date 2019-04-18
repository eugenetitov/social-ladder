using System;
using System.ComponentModel;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Net;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Authentication;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.More;

namespace SocialLadder.Droid.Fragments.Main
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainActivity), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame)]
    public class MoreFragment : BaseFragment<MoreViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_more;
        protected override bool HasBackButton => false;

        private View view;
        private ImageView fbNetworkImage, twitterNetworkImage, instaNetworkImage, fbNetworkLoader, twitterNetworkLoader, instaNetworkLoader;
        LinearLayout fbView, twitterView, instaView;
        TextView fbText, twitterText, instaText, networkTextTop, networkTextBottom, linkedText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            fbView = view.FindViewById<LinearLayout>(Resource.Id.fb_view);
            twitterView = view.FindViewById<LinearLayout>(Resource.Id.twitter_view);
            instaView = view.FindViewById<LinearLayout>(Resource.Id.insta_view);
            fbText = fbView.FindViewById<TextView>(Resource.Id.networkText);
            twitterText = twitterView.FindViewById<TextView>(Resource.Id.networkText);
            instaText = instaView.FindViewById<TextView>(Resource.Id.networkText);
            fbNetworkImage = fbView.FindViewById<ImageView>(Resource.Id.networkImage);
            fbNetworkLoader = fbView.FindViewById<ImageView>(Resource.Id.networkLoader);
            twitterNetworkImage = twitterView.FindViewById<ImageView>(Resource.Id.networkImage);
            twitterNetworkLoader = twitterView.FindViewById<ImageView>(Resource.Id.networkLoader);
            instaNetworkImage = instaView.FindViewById<ImageView>(Resource.Id.networkImage);
            instaNetworkLoader = instaView.FindViewById<ImageView>(Resource.Id.networkLoader);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.more_recycler_view);
            if (recyclerView != null)
            {
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
                recyclerView.ScrollChange += (s, e) =>
                {
                    OnScrollViewChanged(s, e, recyclerView.ScrollState);
                };
            }

            CreateBindings();
            UpdateControls();
            return view;
        }

        private void CreateBindings()
        {
            var owner = this as IMvxBindingContextOwner;
            var set = owner.CreateBindingSet<IMvxBindingContextOwner, MoreViewModel>();
            set.Bind(fbNetworkImage).For(v => v.Drawable).To(vm => vm.FBImage).WithConversion("StringDrawableConverter");
            set.Bind(twitterNetworkImage).For(v => v.Drawable).To(vm => vm.TwitterImage).WithConversion("StringDrawableConverter");
            set.Bind(instaNetworkImage).For(v => v.Drawable).To(vm => vm.InstaImage).WithConversion("StringDrawableConverter");

            set.Bind(fbNetworkLoader).For(v => v.Drawable).To(vm => vm.FBLoaderImage).WithConversion("StringDrawableConverter");
            set.Bind(twitterNetworkLoader).For(v => v.Drawable).To(vm => vm.TwitterLoaderImage).WithConversion("StringDrawableConverter");
            set.Bind(instaNetworkLoader).For(v => v.Drawable).To(vm => vm.InstaLoaderImage).WithConversion("StringDrawableConverter");
            set.Apply();
        }

        private void UpdateControls()
        {
            fbText.Text = "Facebook";
            twitterText.Text = "Twitter";
            instaText.Text = "Instagram";
            ChangeSocialNetworkTextColor();
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.txtUserName), FontsConstants.PN_B, (float)0.037);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.txtLocation), FontsConstants.PN_R, (float)0.034);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.textNetworksConnected), FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.score_count), FontsConstants.PN_R, (float)0.025);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.textAddMore), FontsConstants.PN_R, (float)0.034);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.txtCountFriends), FontsConstants.PN_R, (float)0.034);
            FontHelper.UpdateFont(fbText, FontsConstants.PN_R, (float)0.028);
            FontHelper.UpdateFont(twitterText, FontsConstants.PN_R, (float)0.028);
            FontHelper.UpdateFont(instaText, FontsConstants.PN_R, (float)0.028);
        }

        public static MoreFragment NewInstance()
        {
            var frag = new MoreFragment { Arguments = new Bundle() };
            return frag;
        }

        private void ChangeSocialNetworkTextColor()
        {
            var connectedNetworks = ViewModel.CheckConnectedNetworks();
            fbText.SetTextColor(ContextCompat.GetColorStateList(this.Activity, connectedNetworks.FacebookConnected ? Resource.Color.Black : Resource.Color.areas_description_tex_color ));
            twitterText.SetTextColor(ContextCompat.GetColorStateList(this.Activity, connectedNetworks.TwitterkConnected ? Resource.Color.Black : Resource.Color.areas_description_tex_color));
            instaText.SetTextColor(ContextCompat.GetColorStateList(this.Activity, connectedNetworks.InstagramConnected ? Resource.Color.Black : Resource.Color.areas_description_tex_color));
        }

        #region Authentication
        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.FBLoaderImage))
            {
                if (ViewModel.IsBusy)
                {
                    AnimateImage(twitterNetworkLoader);
                    var auth = new FacebookAuthenticator(Configuration.FbClientId, Configuration.Scope, ViewModel);
                    var authenticator = auth.GetAuthenticator();
                    var intent = authenticator.GetUI(this.Activity);
                    this.StartActivity(intent);
                    AnimateImage(fbNetworkLoader);
                }
                if (!ViewModel.IsBusy)
                {
                    fbNetworkLoader.ClearAnimation();
                }
                ChangeSocialNetworkTextColor();
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.TwitterLoaderImage))
            {
                if (ViewModel.IsBusy)
                {
                    var auth = new TwitterAuthentificator(Configuration.ConsumerKeyTwitter, Configuration.ConsumerSecretTwitter, Configuration.Scope, ViewModel);
                    var authenticator = auth.GetAuthenticator();
                    var intent = authenticator.GetUI(this.Activity);
                    this.StartActivity(intent);
                    AnimateImage(twitterNetworkLoader);
                }
                if (!ViewModel.IsBusy)
                {
                    twitterNetworkLoader.ClearAnimation();
                }
                ChangeSocialNetworkTextColor();
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.InstaLoaderImage))
            {
                if (ViewModel.IsBusy)
                {
                    var auth = new InstagramAuthenticator(Configuration.ConsumerKeyInsta, string.Empty, Configuration.InstaScope, ViewModel);
                    var authenticator = auth.GetAuthenticator();
                    var intent = authenticator.GetUI(this.Activity);
                    this.StartActivity(intent);
                    AnimateImage(instaNetworkLoader);
                }
                if (!ViewModel.IsBusy)
                {
                    instaNetworkLoader.ClearAnimation();
                }
                ChangeSocialNetworkTextColor();
            }
        }
        #endregion
    }
}