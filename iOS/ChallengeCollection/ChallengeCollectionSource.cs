using System;
using Foundation;
using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System.Linq;
using CrashlyticsKit;
using MvvmCross.Binding.iOS.Views;

namespace SocialLadder.iOS.Challenges
{
    public class ChallengeCollectionSource : MvxCollectionViewSource//UICollectionViewSource
    {
        public ChallengeCollectionSource(UICollectionView collectionView) : base(collectionView)
        {
            ChallengesTypes = new List<ChallengeTypeModel>();
        }

        public List<ChallengeTypeModel> ChallengesTypes;

        public Action ChallengeItemSelected { get; set; }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            var list = SL.ChallengeTypeList != null ? SL.ChallengeTypeList : ChallengesTypes;
            ChallengesTypes = list;
            CheckTypes();
            return list != null ? list.Count : 0;
        }


        public void CheckTypes()
        {
            var listTypes = SL.ChallengeTypeList != null ? SL.ChallengeTypeList : ChallengesTypes;

            foreach (var challengeType in listTypes.ToList())
            {
                var itemsExists = SL.ChallengeList.Any(chal => chal.TypeCode == challengeType.TypeCode);

                if (itemsExists == false)
                {
                    ChallengesTypes.Remove(challengeType);
                }
            }
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            ChallengeItemSelected?.Invoke();
        }

        public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //var cell = (UserCell) collectionView.CellForItem(indexPath);
            //cell.ImageView.Alpha = 0.5f;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            /*
            var cell = (UserCell) collectionView.CellForItem(indexPath);
            cell.ImageView.Alpha = 1;

            UserElement row = Rows[indexPath.Row];
            row.Tapped.Invoke();
            */
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            Crashlytics.Instance.Log("Challenges_ChallengesCollectionSource_GetCell()");
            try
            {
                var list = SL.ChallengeTypeList != null ? SL.ChallengeTypeList : ChallengesTypes;
                ChallengeTypeModel summary = list != null && indexPath.Row < list.Count ? list[indexPath.Row] : null;

                var cell = (ChallengeCollectionViewCell)collectionView.DequeueReusableCell(ChallengeCollectionViewCell.ClassName, indexPath);

                cell.ApplyStyle();

                if (summary != null)
                {
                    ChallengeCollectionView challengeCollection = collectionView as ChallengeCollectionView;
                    bool isSelected = challengeCollection != null && indexPath.Row == challengeCollection.SelectedItem;
                    cell.UpdateCellData(summary, isSelected, challengeCollection.Frame.Height);
                }
                else
                    collectionView.ReloadData();    //dataset does not match

                return cell;
            }
            catch (Exception e)
            {
                Crashlytics.Instance.Log($"Challenges_ChallengesCollectionSource_GetCell() - {e.Message}");
                throw;
            }
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //if (SL.IsBusy)
            //{
            //    return;
            //}
            //ChallengeCollectionView challengeCollection = collectionView as ChallengeCollectionView;
            //ChallengeItemSelected?.Invoke();
            //if (challengeCollection != null)
            //{
            //    var list = SL.ChallengeTypeList;
            //    ChallengeTypeModel summary = list != null && indexPath.Row < list.Count ? list[indexPath.Row] : null;

            //    if (summary != null)
            //    {
            //        bool didSelect = challengeCollection.ViewController.ChallengeTypeFilter != summary.DisplayName;
            //        if (didSelect)
            //        {
            //            challengeCollection.ViewController.ChallengeTypeFilter = summary.DisplayName;
            //            challengeCollection.SelectedItem = indexPath.Row;
            //            challengeCollection.ReloadData();
            //            challengeCollection.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredHorizontally, true);

            //        }
            //        else
            //        {
            //            challengeCollection.ViewController.ChallengeTypeFilter = null;
            //            challengeCollection.SelectedItem = -1;
            //            challengeCollection.ReloadData();
            //        }

            //    }
            //    else
            //    {
            //        challengeCollection.SelectedItem = indexPath.Row;
            //        challengeCollection.ReloadData();
            //        challengeCollection.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredVertically, true);
            //    }
            //}
        }
    }
}