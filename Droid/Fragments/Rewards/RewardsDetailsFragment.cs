using Android.Animation;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V4.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Webkit;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Cache;
using FFImageLoading.Cross;
using FFImageLoading.Helpers;
using FFImageLoading.Work;
using Java.Util.Concurrent.Atomic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.CustomControls;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Helpers.RewardsCircleHelper;
using SocialLadder.Extensions;
using SocialLadder.Models;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.Rewards;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using static Android.Animation.Animator;
using static Android.Animation.ValueAnimator;
using static Android.Views.ViewGroup;

namespace SocialLadder.Droid.Fragments.Rewards
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class RewardsDetailsFragment : BaseFragment<RewardsDetailsViewModel>, View.IOnScrollChangeListener
    {
        protected override int FragmentId => Resource.Layout.RewardsDetailsLayout;
        protected override bool HasBackButton => true;

        private ScrollView parallaxScrollView;
        private ImageView parallaxImage;
        private ImageView _overlayImage;
        private ImageView _overlayImageGradient;

        private LinearLayout _imageContainer;
        private TextView _rewardNameText;
        private TextView _categoryNameText;
        private WebView _descriptionWebView;

        private MvxCachedImageView _imageView;

        private TextView _claimText;
        private TextView _priceText;

        private ConstraintLayout _resultView;
        private TextView _resultViewChallengeTitle;
        private TextView _resultViewChallengeTitle2;
        private TextView _resultViewMessage;
        private ImageView _polygonView;
        private LinearLayout _resultViewBackground;
        private Button _collectClaimInfoButton;
        private ImageButton _closeInfoButton;
        private View view;

        private ConstraintLayout _submitButton;

        private IMvxInteraction<RewardResponseModel> _claimRewardInteraction;
        public IMvxInteraction<RewardResponseModel> ClaimRewardInteraction
        {
            get => _claimRewardInteraction;
            set
            {
                if (_claimRewardInteraction != null)
                    _claimRewardInteraction.Requested -= ShowClaimRewardMessageBox;

                _claimRewardInteraction = value;
                _claimRewardInteraction.Requested += ShowClaimRewardMessageBox;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);

            var set = this.CreateBindingSet<RewardsDetailsFragment, RewardsDetailsViewModel>();
            set.Bind(this).For(v => v.ClaimRewardInteraction).To(viewModel => viewModel.ClaimRewardInteraction).OneWay();
            set.Apply();

            parallaxScrollView = view.FindViewById<ScrollView>(Resource.Id.parallax_scroll_view);
            parallaxImage = view.FindViewById<ImageView>(Resource.Id.parallax_image);
            parallaxScrollView.SetOnScrollChangeListener(this);

            _imageContainer = view.FindViewById<LinearLayout>(Resource.Id.top_linearLayout);

            _overlayImage = view.FindViewById<ImageView>(Resource.Id.overlay_image);
            _overlayImageGradient = view.FindViewById<ImageView>(Resource.Id.overlay_image_gradient);

            _rewardNameText = view.FindViewById<TextView>(Resource.Id.reward_name_text);
            _categoryNameText = view.FindViewById<TextView>(Resource.Id.category_name_text);
            _descriptionWebView = view.FindViewById<WebView>(Resource.Id.description_webView);
            _submitButton = view.FindViewById<ConstraintLayout>(Resource.Id.submit_button);

            _imageView = view.FindViewById<MvxCachedImageView>(Resource.Id.imageView);

            _claimText = view.FindViewById<TextView>(Resource.Id.claim_text);
            _priceText = view.FindViewById<TextView>(Resource.Id.price_text);

            /*DON'T TOUCH!!*/
            _resultView = view.FindViewById<ConstraintLayout>(Resource.Id.RewardsCompleteLayout);
            _resultViewChallengeTitle = view.FindViewById<TextView>(Resource.Id.complete_title_text);
            _resultViewChallengeTitle2 = view.FindViewById<TextView>(Resource.Id.complete_title_text2);
            _resultViewMessage = view.FindViewById<TextView>(Resource.Id.complete_message_text);
            _polygonView = view.FindViewById<ImageView>(Resource.Id.rewardIcon);
            _resultViewBackground = view.FindViewById<LinearLayout>(Resource.Id.background_view);
            _collectClaimInfoButton = view.FindViewById<Button>(Resource.Id.collectButton);
            _closeInfoButton = view.FindViewById<ImageButton>(Resource.Id.btn_close);

            _submitButton.Click += _submitButton_Click;
            _collectClaimInfoButton.Click += _collectClaimInfoButton_Click;
            _closeInfoButton.Click += _closeInfoButton_Click;
            /*DON'T TOUCH!!*/

            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.reward_name_text), FontsConstants.PN_R, 0.06f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.category_name_text), FontsConstants.PN_R, 0.045f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.more_text), FontsConstants.PN_B, 0.0275f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.points_text), FontsConstants.PN_R, 0.03f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.claim_text), FontsConstants.PN_R, 0.035f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.price_text), FontsConstants.PN_R, 0.035f);

            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.unlock_time_text), FontsConstants.PN_R, 0.08f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.unlock_header_text), FontsConstants.PN_R, 0.04f);

            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text), FontsConstants.PN_B, 0.06f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text2), FontsConstants.PN_B, 0.05f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_message_text), FontsConstants.PN_R, 0.035f);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.collectButton), FontsConstants.PN_R, 0.05f);

            return view;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.IsBusy))
            {
                if (ViewModel.IsBusy)
                {
                    AnimateImage(_imageView, Resource.Drawable.ic_loadingIndicator);
                }
                if (!ViewModel.IsBusy)
                {
                    _imageView.ClearAnimation();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            _submitButton.Click -= _submitButton_Click;
            _collectClaimInfoButton.Click -= _collectClaimInfoButton_Click;
            _closeInfoButton.Click -= _closeInfoButton_Click;
        }

        private void _collectClaimInfoButton_Click(object sender, EventArgs e)
        {
            _claimText.Visibility = ViewStates.Visible;
            _priceText.Text = $"{ViewModel.RewardScore} pts";
            _imageView.SetImageResource(Resource.Drawable.claim_btn_background);
            if (ViewModel.RewardScore > SL.Profile.Score)
                _imageView.SetImageResource(Resource.Drawable.reward_more_score);
            ViewModel.IsCompleteViewVisible = false;
        }

        private void _closeInfoButton_Click(object sender, EventArgs e)
        {
            ViewModel.CloseClaimRewardViewCommand.Execute();

            _claimText.Visibility = ViewStates.Visible;
            _priceText.Text = $"{ViewModel.RewardScore} pts";
            _imageView.SetImageResource(Resource.Drawable.claim_btn_background);
            if (ViewModel.RewardScore > SL.Profile.Score)
                _imageView.SetImageResource(Resource.Drawable.reward_more_score);
        }

        private void _submitButton_Click(object sender, EventArgs e)
        {
            ViewModel.ClaimRewardCommand.Execute();
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            //_overlayImage.SetY(_overlayImage.Top - scrollY / 3);
            //_overlayImageGradient.SetY(_overlayImageGradient.Top - scrollY / 3);
            //parallaxImage.SetY(parallaxImage.Top - scrollY / 3);
            //_imageContainer.SetY(_imageContainer.Top - scrollY / 3);

            var imageParams = parallaxImage.LayoutParameters;
            imageParams.Width = DimensHelper.GetScreenWidth() + scrollY / 4;
            parallaxImage.LayoutParameters = imageParams;
            _overlayImage.LayoutParameters = imageParams;
            //_overlayImageGradient.LayoutParameters = imageParams;
            //_imageContainer.LayoutParameters = imageParams;
        }

        private void ShowClaimRewardMessageBox(object sender, MvxValueEventArgs<RewardResponseModel> eventArgs)
        {
            if (_resultView == null || eventArgs.Value == null)
            {
                return;
            }
            RewardResponseModel responseModel = eventArgs.Value;
            if (responseModel.UpdatedReward.AutoUnlockDate?.ToLocalTime() > DateTime.Now)
            {
                RewardsCircleCreator.Create(Activity, view);
                return;
            }
            RewardsCircleCreator.Create(Activity, view, () => {
                SetupMaskedIcon(responseModel.UpdatedReward.MainImageURL);
                _resultViewChallengeTitle.Text = responseModel.ResponseCode > 0 ? "Congratulations, " : "Oh No, we`re sorry!";
                _resultViewChallengeTitle2.Text = responseModel.ResponseCode > 0 ? "You got this reward!" : "You did not get this reward...";

                _resultViewMessage.Text = !string.IsNullOrWhiteSpace(responseModel.ResponseMessage) ? responseModel.ResponseMessage
                    : (responseModel.ResponseCode > 0 ? $"You spent {responseModel.UpdatedReward.MinScore} pts" : "There are no more units available");

                var backgroundColor = responseModel.ResponseCode > 0 ? Resources.GetColor(Resource.Color.rewardClaimeSucesfuly) : Resources.GetColor(Resource.Color.rewardClaimeFailed);
                _resultViewBackground.SetBackgroundColor(backgroundColor);

                if (responseModel.ResponseCode > 0)
                {
                    _imageView.SetImageResource(Resource.Drawable.reward_claimed_successful);
                    _claimText.Visibility = ViewStates.Invisible;
                    _priceText.Text = "Claimed";
                }
                else
                {
                    _imageView.SetImageResource(Resource.Drawable.reward_claimed_fail);
                    _claimText.Visibility = ViewStates.Invisible;
                    _priceText.Text = "Didn't Get";

                    _collectClaimInfoButton.Background.Alpha = 0;
                    _collectClaimInfoButton.Text = "See what other rewards you quality for >";
                    _collectClaimInfoButton.SetTextColor(new Color(0, 213, 255, 255));
                    FontHelper.UpdateFont(_collectClaimInfoButton, FontsConstants.PN_R, 0.035f);
                }

                ViewModel.IsCompleteViewVisible = true;
            });
        }

        private void SetupMaskedIcon(string iconUrl)
        {
            var original = GetImageBitmapFromUrl(iconUrl);
            var masked = GetMaskedChallengeIcon(original);
            _polygonView.SetImageBitmap(masked);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private Bitmap GetMaskedChallengeIcon(Bitmap original)
        {
            if (original == null)
            {
                return null;
            }

            Bitmap mask = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Polygon);

            int iv_height = original.Height;
            int iv_width = original.Width;

            var results = Bitmap.CreateBitmap(iv_width, iv_height, Bitmap.Config.Argb8888);
            var maskBitmap = Bitmap.CreateScaledBitmap(mask, iv_width, iv_height, true);

            Canvas canvas = new Canvas(results);

            Paint paint = new Paint(PaintFlags.AntiAlias);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));

            canvas.DrawBitmap(original, 0f, 0f, null);
            canvas.DrawBitmap(maskBitmap, 0f, 0f, paint);

            paint.SetXfermode(null);
            paint.SetStyle(Paint.Style.Stroke);

            return results;
        }
    }
}