using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;
using SocialLadder.iOS.Constraints;
using System.Linq;
using SocialLadder.iOS.Navigation;
using CrashlyticsKit;

namespace SocialLadder.iOS.Challenges
{

    public class ChallengesTableSource : UITableViewSource
    {
        List<ChallengeModel> FilteredList { get; set; }
        ChallengesTableView ChallengesTableView { get; set; }
        SLViewController SLViewController { get; set; }

        private List<ChallengeModel> itemsSource;
        public List<ChallengeModel> ItemsSource
        {
            get
            {
                return itemsSource;
            }
            set
            {
                itemsSource = value;
            }
        }

        private List<string> sectionsTitles;
        public List<string> SectionsTitles
        {
            get { return sectionsTitles; }
            set
            {
                sectionsTitles = value;
            }
        }

        private List<string> OrderTitles = new List<string>() { "Expiring", "Recently Added", "More", "Pending Approval", "Completed" };

        public ChallengesTableSource(ChallengesTableView challengesTableView, SLViewController _SLViewController)
        {
            ChallengesTableView = challengesTableView;
            SLViewController = _SLViewController;
            ItemsSource = new List<ChallengeModel>();
            sectionsTitles = new List<string>();
            SectionsTitles = new List<string>();
            RefreshItemSource();
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            var titleOfSection = SectionsTitles[(int)section];
            var count = ItemsSource.Count(x => x.Group == titleOfSection);
            return count;
        }

        public override void DecelerationStarted(UIScrollView scrollView)
        {
            SLViewController?.HideAreaCollection();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            Crashlytics.Instance.Log("Challenges.ChallengesTableSource.GetCell()");
            var challenge = GetItemFromIndexWithSection(indexPath);
            ChallengesTableViewCell cell = (ChallengesTableViewCell)tableView.DequeueReusableCell(ChallengesTableViewCell.ClassName);
            if (challenge != null)
                cell.UpdateCellData(challenge);
            else
                ChallengesTableView.ReloadData();   //data refreshed during gathering cells
            return cell;
        }

        private ChallengeModel GetItemFromIndexWithSection(NSIndexPath indexPath)
        {
            var list = FilteredList != null ? FilteredList : ItemsSource;
            string sectionTitle = SectionsTitles[(int)indexPath.Section];
            var items = list != null && list.Where(x => x.Group == sectionTitle).FirstOrDefault() != null ? list.Where(y => y.Group == sectionTitle) : null;
            var challenge = items.ToList()[indexPath.Row];
            return challenge;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //if (Platform.IsInternetConnectionAvailable() == false || SL.IsBusy)
            //{
            //    return;
            //}
            //var challenge = GetItemFromIndexWithSection(indexPath);
            //ChallengesTableView challengesTable = tableView as ChallengesTableView;
            //challengesTable.ViewController.LoadChallengeDetail(challenge);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            RefreshItemSource();

            return SectionsTitles.Count;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return SizeConstants.Screen.Width * 0.105f;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            //return base.GetViewForHeader(tableView, section);
            var count = ItemsSource.Where(x => x.Group == SectionsTitles[(int)section]).Count();
            var num = count == 1 ? " Challenge " : " Challenges ";
            var topView = ChallengesTableHeaderView.Create();
            topView.TitleText = count + " " + SectionsTitles[(int)section] + num;
            topView.BadgeHidden = false;
            topView.UpdateControlls();
            return topView;
        }

        private void RefreshItemSource()
        {
            //if (ChallengesTableView.ViewController.ChallengeTypeFilter != null)
            //{
            //    ItemsSource = SL.ChallengeList.Where(x => x.TypeCodeDisplayName == ChallengesTableView.ViewController.ChallengeTypeFilter).OrderBy(x => x.EffectiveEndDate).ToList();
            //}
            //else
            //{
            //    ItemsSource = SL.ChallengeList.OrderBy(x => x.EffectiveEndDate).ToList();
            //}

            //SectionsTitles = OrderTitles.Where(x => ItemsSource.FirstOrDefault(y => x.Equals(y.Group)) != null).ToList();
        }
    }
}