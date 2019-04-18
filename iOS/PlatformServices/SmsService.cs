using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contacts;
using ContactsUI;
using Foundation;
using MessageUI;
using SocialLadder.Interfaces;
using UIKit;

namespace SocialLadder.iOS.PlatformServices
{
    public class SmsService : ISmsService
    {
        public bool SendSmsToNumbers(List<string> numbers, string message)
        {
            try
            {
                var contactPickerController =  new CNContactPickerViewController();
                contactPickerController.PredicateForEnablingContact = NSPredicate.FromValue(true);
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(contactPickerController, true, null);
                // Respond to selection

                var smsController = new MFMessageComposeViewController { Body = message };


                smsController.Finished += (sender, e) =>
                {
                    NSRunLoop.Main.BeginInvokeOnMainThread(() =>
                    {
                        e.Controller.DismissViewController(true, null);
                    });
                };
                var pickerDelegate =  new ContactPickerDelegate();
                pickerDelegate.SelectContacts += ((string[] contactsArr) =>
                {
                    //contacts = contactsArr;


                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(smsController, true, new Action(()
                        =>
                    {
                        ;
                    })
                    );
                  
                });
                contactPickerController.Delegate = pickerDelegate;
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(contactPickerController, true, null);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
       
        public bool SendSmsToWatsApp(string number, string message)
        {
            try
            {
                message = message.Replace("&", "and");
                var uri = new System.Uri($"whatsapp://send?text={message}");

                NSUrl whatsAppUrl = new NSUrl(uri.AbsoluteUri);

                if (UIApplication.SharedApplication.CanOpenUrl(whatsAppUrl))
                {
                    UIApplication.SharedApplication.OpenUrl(whatsAppUrl);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public class ContactPickerDelegate : CNContactPickerDelegate
        {
            public event Action<string[]> SelectContacts;
            public override void ContactPickerDidCancel(CNContactPickerViewController picker)
            {
                Console.WriteLine("User canceled picker");
            }

            //public override void DidSelectContactProperties(CNContactPickerViewController picker, CNContactProperty[] contactProperties)
            //{

            //}

            //public override void DidSelectContactProperty(CNContactPickerViewController picker, CNContactProperty contactProperty)
            //{

            //}

            public override void DidSelectContact(CNContactPickerViewController picker, CNContact contact)
            {

            }

            public override void DidSelectContacts(CNContactPickerViewController picker, CNContact[] contacts)
            {

                var numbers = contacts.Select(x => x.PhoneNumbers.First()).ToList();
                if (numbers.Count == 1)
                {
                    SelectedNumber(numbers[0]);

                }
                if (numbers.Count > 1)
                {
                    ShowChooseNumberPopup(numbers, picker);
                    return;
                }
            }


            private void ShowChooseNumberPopup(List<CNLabeledValue<CNPhoneNumber>> numbers, CNContactPickerViewController picker)
            {
                //_shareTemplateModel = model.ShareTemplate;

                var alertController = UIAlertController.Create("", "What kind of message?", UIAlertControllerStyle.ActionSheet);

                // Add Actions
                string[] stringNumber = new string[numbers.Count()];
                for (int i = 0; i < numbers.Count(); i++)
                {
                    alertController.AddAction(UIAlertAction.Create(numbers[i].Value.StringValue, UIAlertActionStyle.Default, (handle)=> { SelectedNumber(numbers[i]); }));
                }

                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel sending message")));

                // Show the alert
                picker.PresentViewController(alertController, true, null);
            }

            private void SelectedNumber(CNLabeledValue<CNPhoneNumber> number)
            {
                string stringNumber = number.Value.StringValue.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
                SelectContacts(new string[] { stringNumber });
            }

        }
    }
}