using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ChallengeCollectionView : UICollectionView
    {
        public int SelectedItem { get; set; }
        public ChallengesViewController ViewController { get; set; }

        public ChallengeCollectionView (IntPtr handle) : base (handle)
        {
            BackgroundColor = UIColor.Clear;
            SelectedItem = -1;
        }
    }
}