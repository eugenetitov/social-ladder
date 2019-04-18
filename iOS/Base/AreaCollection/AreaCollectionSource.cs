using System;
using Foundation;
using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;

namespace SocialLadder.iOS.Areas
{
    public class AreaCollectionSource : UICollectionViewSource
    {
        private List<AreaModel> ListAreas;

        public AreaCollectionSource()
        {
            ListAreas = new List<AreaModel>();
            ListAreas.Add(null);
            ListAreas.AddRange(SL.Profile.AreaSubsList as IEnumerable<AreaModel>);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ListAreas.Count; //add 1 for the add areas option
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
            AreaModel area = ListAreas[indexPath.Row];

            var cell = (AreaCollectionViewCell)collectionView.DequeueReusableCell(AreaCollectionViewCell.ClassName, indexPath);

            if (area != null)
                cell.UpdateCellData(area);
            else
                cell.UpdateToAddArea();

            return cell;
        }

        public async override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            AreaCollectionView areaCollection = collectionView as AreaCollectionView;
            if (areaCollection != null)
            {
                AreaModel area = ListAreas[indexPath.Row];
                if (area != null)
                {
                    SL.AreaGUID = area.areaGUID;

                    SL.ChallengeList = null;
                    SL.RewardList = null;
                    SL.Feed = null;

                    areaCollection.Hide();
                    await areaCollection.Refresh();
                    ProfileResponseModel response = await SL.Manager.FinalCheckInAsync(Platform.Lat, Platform.Lon);
                }
                else
                {
                    areaCollection.AddMoreAreas();
                }
            }
        }
    }
}