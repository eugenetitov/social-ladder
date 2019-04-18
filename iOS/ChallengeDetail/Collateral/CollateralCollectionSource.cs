using System;
using Foundation;
using UIKit;
using SocialLadder.iOS.Camera;
using System.Collections.Generic;

namespace SocialLadder.iOS.Challenges
{
    public class CollateralCollectionSource : UICollectionViewSource
    {
        public List<CameraPicture> Pictures { get; set; }

        public CollateralCollectionSource(List<CameraPicture> pictures)//CollateralViewController collateralCollection)
        {
            Pictures = pictures == null ? new List<CameraPicture>() : pictures;
            //CollateralCollectionView = collateralCollection;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Pictures != null ? Pictures.Count : 0;
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
            CameraPicture picture = Pictures != null ? Pictures[indexPath.Row] : null;

            var cell = (CollateralCollectionViewCell)collectionView.DequeueReusableCell(CollateralCollectionViewCell.ClassName, indexPath);

            if (picture != null)
                cell.UpdateCellData(picture);

            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            CollateralCollectionView collateralCollection = collectionView as CollateralCollectionView;
            if (collateralCollection != null)
            {
                
            }
        }
    }
}