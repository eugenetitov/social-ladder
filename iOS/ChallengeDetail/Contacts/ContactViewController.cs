using AVFoundation;
using Contacts;
using Foundation;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SocialLadder.iOS.Navigation;
using SocialLadder.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ContactViewController : UIViewController
    {
        List<PersonContact> AllContact { get; set; }
        ContactTableSource ContactTableSource { get; set; }
        int targetCount = 0;
        NSObject keyboardShowObj;
        NSObject keyboardHideObj;
        private UIButton btnDone;
        public event Action<string[]> SelectContacts;

        public ContactViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            contactTable.RegisterNibForCellReuse(ContactTableViewCell.Nib, ContactTableViewCell.ClassName);
            Title = "Contacts";
            searchBar.SizeToFit();
            searchBar.ReturnKeyType = UIReturnKeyType.Done;
            searchBar.SearchButtonClicked += (s, e) =>
            {
                searchBar.EndEditing(true);
            };

            ContactTableSource = new ContactTableSource(this, contactTable);
            GetAllContactsAndPhoneNumbers(new Action<IEnumerable<PersonContact>>((contacts)=>
            {
                if (contacts != null)
                {
                    AllContact = new List<PersonContact>(contacts);
                    targetCount = AllContact.Count;
                    ContactTableSource.ItemSource = AllContact;
                    contactTable.Source = ContactTableSource;
                    contactTable.ReloadData();
                }

                var item = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, ActionDone);
                NavigationItem.SetRightBarButtonItem(item, true);
                UpdateCTALabel(0);
            }
            ));
            //if (contacts != null)
            //{
            //    AllContact = new List<PersonContact>(contacts);
            //    targetCount = AllContact.Count;
            //    ContactTableSource.ItemSource = AllContact;
            //    contactTable.Source = ContactTableSource;
            //    contactTable.ReloadData();
            //}

            //var item = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, ActionDone);
            //NavigationItem.SetRightBarButtonItem(item, true);
            //UpdateCTALabel(0);

        }

        public void ActionDone(object sender, EventArgs args)
        {
            if (ContactTableSource.SendContact.Values.Count > 0)
            {
                SelectContacts?.Invoke(ContactTableSource.SendContact.Values.ToArray());
            }
            btnDone.Hidden = true;
            NavigationController.PopViewController(true);
            //ShowNotification();

        }

        public void ShowNotification()
        {
            if (ContactTableSource.SendContact.Values.Count != 0)
            {
                var okAlertController = UIAlertController.Create("", ContactTableSource.SendContact.Values.Count == 1 ? "Ok, we invited this folk!" : "Ok, we invited those folks!", UIAlertControllerStyle.Alert);

                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (action) => { NavigationController.PopViewController(true); }));

                // Present Alert
                PresentViewController(okAlertController, true, null);
            }
            if (ContactTableSource.SendContact.Values.Count == 0)
            {
                NavigationController.PopViewController(true);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            searchBar.TextChanged += SearchBar_TextChanged;
            RegisterForKeyboardNotifications();
            if (NavigationController is SLNavigationController)
            {
                btnDone = (NavigationController as SLNavigationController).NavTitle.btnDoneOutlet;
                btnDone.Hidden = false;
                btnDone.TouchUpInside += ActionDone;
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            searchBar.TextChanged -= SearchBar_TextChanged;
            RemoveForKeyboardNotifications();
            if (btnDone != null)
            {
                btnDone.Hidden = true;
                btnDone.TouchUpInside -= ActionDone;
            }
        }

        private void SearchBar_TextChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            string text = e.SearchText.Trim();
            var searchResult = AllContact.Where(x => x.FamilyName.ToUpper().Contains(text.ToUpper()) ||
                        x.GivenName.ToUpper().Contains(text.ToUpper())).ToList();
            ContactTableSource.ItemSource = searchResult;
            contactTable.ReloadData();
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

        }

        public async void GetAllContactsAndPhoneNumbers(Action<IEnumerable<PersonContact>> calback)
        {

            var contacts = new List<PersonContact>();
            try
            {
                await AskContactListPermission();
                PopulateContactList(contacts);
            }
            catch (Exception e)
            {
                LogHelper.LogUserMessage("BADCONTACTLIST", e.Message);
            }
            calback(contacts);
            
        }

        private async Task AskContactListPermission()
        {
            var result = await HasContactsPermission();
            if (!result)
            {
                InvokeOnMainThread(() =>
                {
                    var alertController = UIAlertController.Create("Access to contacts is disable", "Please, go to settings and turn on access to contacts for this app", UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => { NavigationController.PopViewController(true); }));
                    NavigationController.PresentViewController(alertController, true, null);
                    return;
                });
            }
        }

        private void PopulateContactList(List<PersonContact> contacts)
        {

            CNContact[] contactList = new CNContact[0];
            var keysTOFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers };
            var containerId = new CNContactStore().DefaultContainerIdentifier;
            
            NSError containerError = new NSError();
            var containers = new CNContactStore().GetContainers(null, out containerError);
            foreach (var item in containers)
            {
                containerId = item.Identifier;


                NSError unifiedContanctError = new NSError();
                using (var predicate = CNContact.GetPredicateForContactsInContainer(containerId))
                using (var store = new CNContactStore())
                {
                    contactList = store.GetUnifiedContacts(predicate, keysTOFetch, out unifiedContanctError);
                }

                int index = 0;
                foreach (var contact in contactList)
                {
                    if (null != item && contact.PhoneNumbers.Length > 0)
                    {
                        contacts.Add(new PersonContact
                        {
                            ContactID = index,
                            GivenName = contact.GivenName,
                            FamilyName = contact.FamilyName,
                            PhoneNumbers = contact.PhoneNumbers
                        });
                        index++;
                    }
                }
            }
        }

        private async Task<bool> HasContactsPermission()
        {
            try
            {
                // Check permission
                var hasCameraPermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

                // Ask for permissions
                if (hasCameraPermission != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts); // Only shows prompt once
                    hasCameraPermission = results[Permission.Contacts];
                }
                return Equals(hasCameraPermission, PermissionStatus.Granted);
            }
            catch (Exception ex)
            {
                Console.WriteLine("              " + ex.Message);
            }
           
            return false;
        }

        public void UpdateCTALabel(int selectedData)
        {
            if (selectedData  == 0)
            {
                CTALabel.Text = $"Pick {selectedData} people to invite";
                return;
            }
            if (selectedData < targetCount)
            {
                CTALabel.Text = $"{selectedData} selected, pick {targetCount - selectedData} more";
                return;
            }
            CTALabel.Text = $"{selectedData} selected - maximum allowed";
        }

        protected virtual void RegisterForKeyboardNotifications()
        {
            if (keyboardShowObj == null)
            {
                keyboardShowObj = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardDidShowNotification);
                keyboardHideObj = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardWillHideNotification);
            }
        }
        protected virtual void RemoveForKeyboardNotifications()
        {
            if (keyboardShowObj != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardShowObj);
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardHideObj);
                keyboardShowObj = null;
                keyboardHideObj = null;
            }
        }

        

        private void OnKeyboardDidShowNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }
            var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            TableBottomConstraint.Constant = keyboardFrame.Height;
            View.UpdateConstraints();

        }
        private void OnKeyboardWillHideNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }

            TableBottomConstraint.Constant = 0;
            View.UpdateConstraints();
        }
    }
}