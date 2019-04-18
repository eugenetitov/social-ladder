using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards.Models
{
    public interface IRewardTableCell
    {
        void UpdateCellData(RewardItemModel item, nfloat offset, RewardStatus status = RewardStatus.None, bool isRightOrientation = true);
        void UpdateCellData(RewardItemModel item, nfloat offset, bool isRightOrientation = true);
        void UpdateCellData(RewardItemModel item);
    }
}