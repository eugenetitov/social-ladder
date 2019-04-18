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
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using SocialLadder.Models;

namespace SocialLadder.Droid.TemplateSelectors
{
    public class RewardsTemplateSelector : MvxTemplateSelector<RewardItemModel>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(RewardItemModel forItemObject)
        {
            if (forItemObject.Type != "REWARD")
            {
                return Resource.Layout.reward_subcategory_cell;
            }

            bool locked =  (forItemObject.AutoUnlockDate?.ToLocalTime() ?? DateTime.Now) > DateTime.Now;
            bool noPoints  = forItemObject.MinScore > SL.Profile.Score;
            if (noPoints && locked)
            {
                return Resource.Layout.reward_cell_layout1;
            }
            if (noPoints && !locked)
            {
                return Resource.Layout.reward_cell_layout2;
            }
            if (!noPoints && locked)
            {
                return Resource.Layout.reward_cell_layout3;
            }

            return Resource.Layout.reward_cell_layout4;
        }
    }
}