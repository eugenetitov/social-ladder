using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.MessangerModels
{
    public class MessangerRewardModel : MvxMessage
    {
        public RewardItemModel RewardItem
        {
            get; set;
        }

        public string CategoryName
        {
            get;set; 
        }

        public MessangerRewardModel(object sender, RewardItemModel rewardItem, string categoryName = null) : base(sender)
        {
            RewardItem = rewardItem;
            CategoryName = categoryName;
        }


        public MvxObservableCollection<RewardItemModel> ParentRewardItems
        {
            get; set;
        }


        public MessangerRewardModel(object sender, MvxObservableCollection<RewardItemModel> rewardsList, string categoryName) : base(sender)
        {
            ParentRewardItems = new MvxObservableCollection<RewardItemModel>();
            ParentRewardItems = rewardsList;
            CategoryName = categoryName;
        }

    }
}
