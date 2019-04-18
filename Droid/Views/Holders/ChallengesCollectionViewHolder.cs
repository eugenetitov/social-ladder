using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models.LocalModels.Challenges;
using static Android.Views.ViewGroup;
using SocialLadder.Droid.Assets;
using Android.Graphics.Drawables;
using Android.Views.Animations;

namespace SocialLadder.Droid.Views.Holders
{
    public class ChallengesCollectionViewHolder : MvxRecyclerViewHolder
    {
        private ConstraintLayout mainView;
        private ImageView icon;
        private string BackgroundColor { get; set; }
        private int topDimen = DimensHelper.GetDimensById(Resource.Dimension.challenges_collection_unselected_top);
        private int sideDimen = DimensHelper.GetDimensById(Resource.Dimension.challenges_collection_unselected_side);
        private int selectedDimen = DimensHelper.GetDimensById(Resource.Dimension.challenges_collection_selected);
        private int left_margin { get; set; }
        private MarginLayoutParams marginParams;

        public ChallengesCollectionViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            mainView = itemView.FindViewById<ConstraintLayout>(Resource.Id.main_view);
            icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            if (icon != null)
            {
                icon.SetColorFilter(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.textIcon)), PorterDuff.Mode.SrcIn);
            }
            marginParams = mainView.LayoutParameters as MarginLayoutParams;
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.top_text), FontsConstants.PN_R, (float)0.03);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.bottom_text), FontsConstants.PN_B, (float)0.035);
        }

        public void SetFirstItemMargin()
        {
            marginParams.SetMargins(sideDimen, topDimen, sideDimen, topDimen);
            left_margin = marginParams.LeftMargin > 0 ? marginParams.LeftMargin : 0;
            UpdateMainView();
        }

        public void ResetLeftMargin()
        {
            left_margin = 0;
        }

        public void SetItemSelected()
        {
            marginParams.SetMargins(left_margin - selectedDimen, topDimen - selectedDimen, sideDimen - selectedDimen, topDimen - selectedDimen);
            UpdateMainView();

            //Animation ani = new ChallengesCollectionAnimation(mainView, marginParams);
            //ani.Duration = 100;
            //mainView.StartAnimation(ani);
        }

        public void SetItemUnselected()
        {
            marginParams.SetMargins(left_margin, topDimen, sideDimen, topDimen);
            UpdateMainView();

            //Animation ani = new ChallengesCollectionAnimation(mainView, marginParams);
            //ani.Duration = 100;
            //mainView.StartAnimation(ani);
        }

        private void UpdateMainView()
        {
            mainView.LayoutParameters = marginParams;
            //mainView.Invalidate();
            mainView.LayoutTransition = null;
        }

        public void SetColor(string color)
        {
            if (color == null)
            {
                return;
            }
            GradientDrawable gradient = new GradientDrawable(Android.Graphics.Drawables.GradientDrawable.Orientation.BottomTop, new int[]{Android.Graphics.Color.ParseColor(color)});
            gradient.SetShape(ShapeType.Rectangle);
            gradient.SetCornerRadius(10f);
            gradient.SetColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor(color)));
            mainView.SetBackgroundDrawable(gradient);
        }
    }

    public class ChallengesCollectionAnimation : Animation
    {
        ConstraintLayout _mainView;
        MarginLayoutParams _marginParams;

        public ChallengesCollectionAnimation(ConstraintLayout mainView, MarginLayoutParams marginParams)
        {
            _mainView = mainView;
            _marginParams = marginParams;
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            //base.ApplyTransformation(interpolatedTime, t);
            _mainView.LayoutParameters = _marginParams;
            _mainView.RequestLayout();
        }

        public override void Initialize(int width, int height, int parentWidth, int parentHeight)
        {
            base.Initialize(width, height, parentWidth, parentHeight);
        }

        public override bool WillChangeBounds()
        {
            //return base.WillChangeBounds();
            return true;
        }
    }
}