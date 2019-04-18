using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models.LocalModels.Challenges;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalModels.Challenges.LocalContactModel>;

namespace SocialLadder.Droid.Adapters.Challenges
{
    public class ContactsListAdapter : BaseAdapter
    {
        public ContactsListAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            //var icon = view.FindViewById<ImageView>(Resource.Id.selected_image);
            //if (icon != null)
            //{
            //    icon.SetColorFilter(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.Black)), PorterDuff.Mode.SrcIn);
            //}
            UpdateControls(view);
            return view;
        }

        protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            if (viewToUse != null && viewToUse.Content != null && source != null)
            {
                var image = viewToUse.Content.FindViewById<ImageView>(Resource.Id.selected_image);
                image.Visibility = ((source as LocalContactModel).IsSelected ? ViewStates.Visible : ViewStates.Gone);
                image.SetColorFilter(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.Black)), PorterDuff.Mode.SrcIn);
            }
            base.BindBindableView(source, viewToUse);
        }

        private void UpdateControls(View view)
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.name_text), FontsConstants.PN_B, (float)0.05);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.number_text), FontsConstants.PN_R, (float)0.025);
        }
    }
}