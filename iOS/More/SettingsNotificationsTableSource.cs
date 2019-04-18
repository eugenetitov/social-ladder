using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;

namespace SocialLadder.iOS.More
{
    public class SettingsNotificationsTableSource : UITableViewSource
    {

        public SettingsNotificationsTableSource()
        {

        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return SL.AreaCount;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            AreaModel area = SL.GetArea(indexPath.Row);

            SettingsNotificationsTableViewCell cell = (SettingsNotificationsTableViewCell)tableView.DequeueReusableCell(SettingsNotificationsTableViewCell.ClassName);

            cell.UpdateCellData(area);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            AreaModel area = SL.GetArea(indexPath.Row);
            if (area != null)
                SL.AreaGUID = area.areaGUID;
        }
    }
}