using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;

namespace SocialLadder.iOS.More
{
    public class MenuOption
    {
        public MenuOption(string text, string image)
        {
            Text = text;
            Image = image;
        }
        public string Text { get; set; }
        public string Image { get; set; }
    }

    public class MoreTableSource : UITableViewSource
    {
		MoreViewController ViewController { get; set; }
        List<MenuOption> MenuList { get; set; }
        private CoreGraphics.CGRect MainFrame = UIScreen.MainScreen.Bounds;

		public MoreTableSource(MoreViewController controller)
        {
			ViewController = controller;
            MenuList = new List<MenuOption>();// { "FAQ", "Settings", "Privacy Policy", "Terms of Service", "Logout" };
            MenuList.Add(new MenuOption("FAQ", "account-question-icon"));
            MenuList.Add(new MenuOption("Settings", "account-settings-icon"));
            MenuList.Add(new MenuOption("Privacy Policy", "account-docs-icon"));
            MenuList.Add(new MenuOption("Terms of Service", "account-docs-icon"));
            MenuList.Add(new MenuOption("Logout", "account-logout-icon"));
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return MenuList.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            MoreTableViewCell cell = (MoreTableViewCell)tableView.DequeueReusableCell(MoreTableViewCell.ClassName);

            cell.UpdateCellData(MenuList[indexPath.Row]);

            return cell;
        }

        void LoadSettings()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("SettingsViewController");
            ViewController.NavigationController.PushViewController(ctrl, true);
        }

        void LoadFAQ()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("FAQViewController");
            ViewController.NavigationController.PushViewController(ctrl, true);
        }

        void LoadPrivacyPolicy()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("PrivacyPolicyViewController");
            ViewController.NavigationController.PushViewController(ctrl, true);
        }

        void LoadTermsService()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("TermsServiceViewController");
            ViewController.NavigationController.PushViewController(ctrl, true);
        }

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //string selection = MenuList[indexPath.Row].Text;
            //if (selection != null)
            //{
            //    if (selection == "Logout")
            //    {
            //        if (ViewController != null)
            //            ViewController.Logout();
            //    }
            //    else if (selection == "Settings")
            //    {
            //        LoadSettings();
            //    }
            //    else if (selection == "FAQ")
            //    {
            //        LoadFAQ();
            //    }
            //    else if (selection == "Privacy Policy")
            //    {
            //        LoadPrivacyPolicy();
            //    }
            //    else if (selection == "Terms of Service")
            //    {
            //        LoadTermsService();
            //    }
            //}
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return MainFrame.Width * 0.102f;
        }
    }
}