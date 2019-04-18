using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IPlatformAssetService
    {
        string SmallScoreIcon { get; }
        string NetworksScoreImage { get; }
        string NetworksScoreImageBold { get; }
        string FBUnconnectedIcon { get; }
        string FBConnectedIcon { get; }
        string TwitterUnconnectedIcon { get; }
        string TwitterConnectedIcon { get; }
        string InstaUnconnectedIcon { get; }
        string InstaConnectedIcon { get; }
        string NetworkConnectedLoaderImage { get; }
        string NetworkConnectingLoaderImage { get; }
        string NetworksScore33Per_Image { get; }
        string NetworksScore66Per_Image { get; }
        string NetworksScore100Per_Image { get; }
        string IconScoreTransactions { get; }

        string PinIconWhite { get; }
        string FBLogoIconWhite { get; }
        string InstaIconWhite { get; }
        string TicketIconWhite { get; }
        string InviteIconWhite { get; }
        string BulhornIconWhite { get; }
        string PosteringIconWhite { get; }
        string FlyeringIconWhite { get; }

        string ManualIconWhite { get; }
        string QuizIconWhite { get; }
        string SignupIconWhite { get; }
        string DocsubmitIconWhite { get; }
        string TwitterIconWhite { get; }

        string ChallengesShareButton { get; }
        string ChallengesFacebookButton { get; }
        string ChallengesInviteButton { get; }
    }
}
