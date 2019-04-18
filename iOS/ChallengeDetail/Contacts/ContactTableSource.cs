using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contacts;
using Foundation;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public class ContactTableSource : UITableViewSource
    {
        ContactViewController viewController;
        string[] keys = new string[0];
        SortedDictionary<string,List<PersonContact>> Contacts;
        public Dictionary<int, string> SendContact;
        UITableView contactTable;
        int selectedData = 0;
        

        public IEnumerable<PersonContact> ItemSource
        {
            get => null;
            set
            {
                Contacts = new SortedDictionary<string, List<PersonContact>>();
                foreach (var person in value)
                {
                    if (!string.IsNullOrWhiteSpace(person.GivenName))
                    {
                        byte[] bytes = Encoding.Default.GetBytes(person.GivenName);
                        if (Contacts.ContainsKey(Encoding.UTF8.GetString(bytes, 0, 1)))
                        {
                            Contacts[Encoding.UTF8.GetString(bytes, 0, 1)].Add(person);
                            continue;
                        }
                        else
                        {
                            var list1 = new List<PersonContact>();
                            list1.Add(person);
                            Contacts.Add(Encoding.UTF8.GetString(bytes, 0, 1), list1);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(person.FamilyName))
                    {
                        byte[] bytes = Encoding.Default.GetBytes(person.FamilyName);
                        if (Contacts.ContainsKey(Encoding.UTF8.GetString(bytes, 0, 1)))
                        {
                            Contacts[Encoding.UTF8.GetString(bytes, 0, 1)].Add(person);
                            continue;
                        }
                        else
                        {
                            var list2 = new List<PersonContact>();
                            list2.Add(person);
                            Contacts.Add(Encoding.UTF8.GetString(bytes, 0, 1), list2);
                        }

                    }
                    else
                    {
                        if (Contacts.ContainsKey("#"))
                        {
                            person.GivenName = person.PhoneNumbers[0].Value.StringValue;
                            Contacts["#"].Add(person);
                            continue;
                        }
                        else
                        {
                            var list3 = new List<PersonContact>();
                            person.GivenName = person.PhoneNumbers[0].Value.StringValue;
                            list3.Add(person);
                            Contacts.Add("#", list3);
                        }
                    }
                }
                keys = Contacts.Keys.ToArray();
            }
        }   

        public ContactTableSource(ContactViewController vc, UITableView table) : base()
        {
            SendContact = new Dictionary<int, string>();
            viewController = vc;
            contactTable = table;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (ContactTableViewCell)tableView.DequeueReusableCell(ContactTableViewCell.ClassName);

            if (cell == null)
            {
                return new UITableViewCell();
            }
            cell.UpdateCell(Contacts[keys[indexPath.Section]][indexPath.Row]);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {

            return Contacts[keys[section]].Count;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            // return base.NumberOfSections(tableView);
            return keys.Length;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var contact = Contacts[keys[indexPath.Section]][indexPath.Row];

            if (contact.SelectedContact)
            {
                contact.SelectedContact = false;
                SendContact.Remove(contact.ContactID);
                contactTable.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
                selectedData--;
                viewController.UpdateCTALabel(selectedData);
                return;
            }

            var cell = (ContactTableViewCell)tableView.DequeueReusableCell(ContactTableViewCell.ClassName, indexPath);

            if (contact.PhoneNumbers.Length > 1)
            {
                ShowChooseNumberPopup(cell, contact, indexPath);
                return;
            }

            contact.SelectedContact = true;
            SelectedNumber(contact.ContactID, contact.PhoneNumbers[0]);
            tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return keys[section];
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 45f;
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return keys;
        }

        private void ShowChooseNumberPopup(ContactTableViewCell cell, PersonContact contact, NSIndexPath indexPath)
        {

            var numbers = contact.PhoneNumbers;
            var alertController = UIAlertController.Create("Which number?", "", UIAlertControllerStyle.ActionSheet);

            List<string> stringNumber = new List<string>();
            for (int i = 0; i < numbers.Count(); i++)
            {
                if (numbers[i].Label.Contains("_$!<"))
                {
                    stringNumber.Add($"{numbers[i].Label.Substring(numbers[i].Label.IndexOf('<') + 1, numbers[i].Label.IndexOf('>') - numbers[i].Label.IndexOf('<') - 1)}: " +
                        $"{numbers[i].Value.StringValue}");
                    continue;
                }
                stringNumber.Add(numbers[i].Label + " " + numbers[i].Value.StringValue);

            }

            for (int i = 0; i < stringNumber.Count(); i++)
            {
                alertController.AddAction(UIAlertAction.Create($"{stringNumber[i]}", UIAlertActionStyle.Default,
                    (action) =>
                    {
                        contact.SelectedContact = true;
                        SelectedNumber(contact.ContactID, numbers[stringNumber.IndexOf(action.Title)]);
                        contactTable.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
                    }));
            }

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel sending message")));
            viewController.PresentViewController(alertController, true, null);
        }

        private void SelectedNumber(int id, CNLabeledValue<CNPhoneNumber> number)
        {
            string stringNumber = number.Value.StringValue.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
            if (!SendContact.ContainsKey(id))
            {
                SendContact.Add(id, stringNumber);
                selectedData++;
                viewController.UpdateCTALabel(selectedData);
            }
            //SelectContacts(new string[] { stringNumber });
        }

        //private void UpdateCommentView(PersonContact item)
        //{
        //    var row = ItemsSource.IndexOf(item);
        //    var index = NSIndexPath.FromRowSection(row, 0);
        //    FeedItemModel feedItem = ItemsSource[row];
        //    string cellId = CellType(feedItem);
        //    FeedBaseTableViewCell cell = (FeedBaseTableViewCell)FeedTableView.DequeueReusableCell(cellId, index);
        //    cell.FeedTableView.ReloadRows(new NSIndexPath[] { index }, UITableViewRowAnimation.Automatic);
        //    FeedTableView.SetNeedsUpdateConstraints();
        //}
    }
}