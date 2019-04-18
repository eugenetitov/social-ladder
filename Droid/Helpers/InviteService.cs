using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using SocialLadder.Droid.CustomListeners;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Models.LocalModels.Challenges;
using static Android.Content.PM.PackageManager;

namespace SocialLadder.Droid.Helpers
{
    public class InviteService
    {
        #region Fields
        public readonly int _request_code_pick_contact = 1;
        public readonly int _max_pick_contact = 10;
        private readonly Context _context;
        public MvxFragment Fragment { get; set; }
        public bool NeedMultiselect { get; set; }
        #endregion

        public InviteService(MvxFragment fragment, bool needMultiselect = true)
        {
            _context = Application.Context;
            Fragment = fragment;
            NeedMultiselect = needMultiselect;
            PermissionHelper.AddPermission(Fragment.Activity, new List<string> { Manifest.Permission.ReadExternalStorage, Manifest.Permission.ReadContacts, Manifest.Permission.WriteContacts });
            //PermissionHelper.AddPermission(Fragment.Activity, new List<string> { Manifest.Permission.ReadExternalStorage, Manifest.Permission.ReadContacts, Manifest.Permission.WriteContacts, Manifest.Permission.SendSms });
        }

        public void ShowMenu(ImageButton button)
        {
            var popupMenu = new PopupMenu(Fragment.Activity, button);
            popupMenu.Menu.Add(ChallengesConstants.WhatsApp);
            popupMenu.Menu.Add(ChallengesConstants.SMS);
            popupMenu.Menu.Add(ChallengesConstants.Cancel);
            popupMenu.MenuItemClick += OnMenuItemClicked;
            popupMenu.Show();
        }

        public Task<bool> ShareLink(string title, string extraText)
        {
            Intent sendIntent = new Intent(Intent.ActionSend);
            sendIntent.SetType("text/plain");
            sendIntent.PutExtra(Intent.ExtraText, extraText);

            Intent receiver = new Intent(_context, new InviteIntentReceiver().Class);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(_context, 0, receiver, PendingIntentFlags.UpdateCurrent);
            var chooserIntent = Intent.CreateChooser(sendIntent, title ?? string.Empty, pendingIntent.IntentSender);
            
            IList<ResolveInfo> resInfo = _context.PackageManager.QueryIntentActivities(sendIntent, 0);
            List<LabeledIntent> intentList = new List<LabeledIntent>();
            for (int i =0; i < resInfo.Count; i++)
            {
                ResolveInfo ri = resInfo.ElementAt(i);
                string packageName = ri.ActivityInfo.PackageName;
                if(packageName.Contains("com.google.android.apps.messaging"))
                {
                    Intent intent = new Intent();
                    intent.SetComponent(new ComponentName(packageName, ri.ActivityInfo.Name));
                    intent.SetAction(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, extraText);
                    intentList.Add(new LabeledIntent(intent, packageName, ri.LoadLabel(_context.PackageManager), ri.Icon));
                }
                else if (packageName.Contains("com.whatsapp"))
                {
                    Intent intent = new Intent();
                    intent.SetComponent(new ComponentName(packageName, ri.ActivityInfo.Name));
                    intent.SetAction(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, extraText);
                    intentList.Add(new LabeledIntent(intent, packageName, ri.LoadLabel(_context.PackageManager), ri.Icon));
                }
                else if (packageName.Contains("com.google.android.apps.docs"))
                {
                    Intent intent = new Intent();
                    intent.SetComponent(new ComponentName(packageName, ri.ActivityInfo.Name));
                    intent.SetAction(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, extraText);
                    intentList.Add(new LabeledIntent(intent, packageName, ri.LoadLabel(_context.PackageManager), ri.Icon));
                }
            }
            LabeledIntent[] extraIntents = intentList.ToArray();


            chooserIntent.PutExtra(Intent.ExtraInitialIntents, extraIntents);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);

            _context.StartActivity(chooserIntent);
            return Task.FromResult(true);
        }

        public async Task<List<LocalContactModel>> GetContacts()
        {
            if (!PermissionHelper.CheckPermissions(Fragment.Activity, new List<string> { Manifest.Permission.ReadContacts, Manifest.Permission.WriteContacts }))
            {
                return null;
            }
            var contactList = new List<LocalContactModel>();
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            string[] projection = { ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                ContactsContract.CommonDataKinds.Phone.Number };
            await Task.Run(() => {
                var cursor = Android.App.Application.Context.ContentResolver.Query(uri, projection, null, null, null);

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        var muser = new LocalContactModel();
                        var number = cursor.GetString(cursor.GetColumnIndex(projection[2]));

                        muser.Number = number;
                        muser.Name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                        if (!string.IsNullOrEmpty(number) && !contactList.Any(x => x.Number == muser.Number))
                            contactList.Add(muser);
                    } while (cursor.MoveToNext());
                }
            });
            var OrderContactList = contactList.OrderBy(item => item.Name);
            foreach (var item in OrderContactList)
            {
                Console.WriteLine($"{item.Name} {item.Number}");
            }
            return contactList;
        }

        public void OnMenuItemClicked(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            var label = e.Item.TitleFormatted.ToString();
            if (label == ChallengesConstants.WhatsApp)
            {
                if (!AppInstalled(Fragment.Context, "com.whatsapp"))
                {
                    AlertDialog.Builder alertDialog = new AlertDialog.Builder(Fragment.Context);
                    alertDialog.SetMessage("Unable to find the WhatsApp on your device. This challenge requires WhatsApp.");
                    alertDialog.SetPositiveButton("OK", (s, args) =>
                    {
                    });
                    Dialog dialod = alertDialog.Create();
                    dialod.Show();

                }
                else
                    (Fragment.ViewModel as IChallengeInviteService).UseWatsAppToSendInviteContact("");
            }
            if (label == ChallengesConstants.SMS)
            {
                if (NeedMultiselect)
                {
                    (Fragment.ViewModel as IChallengeInviteService).UseSmsToSendInviteContact(null);
                }
                if (!NeedMultiselect)
                {
                    var pickContactIntent = new Intent(Intent.ActionPick, ContactsContract.Contacts.ContentUri);
                    pickContactIntent.SetType(ContactsContract.CommonDataKinds.Phone.ContentType);
                    Fragment.StartActivityForResult(pickContactIntent, 0);
                }     
            }
            if (label == ChallengesConstants.Cancel)
            {
                (Fragment.ViewModel as IChallengeInviteService).CancelInviteAction();
            }
        }

        public void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode != (int)Result.Ok)
            {
                (Fragment.ViewModel as IChallengeInviteService).CancelInviteAction();
            }
            try
            {
                Android.Net.Uri contactsURI = data.Data;
                string userNumber = string.Empty;
                string[] queryFields = new string[] { ContactsContract.CommonDataKinds.Phone.Number };

                ICursor cursor = Application.Context.ContentResolver.Query(contactsURI, queryFields, null, null, null);

                if (cursor != null && cursor.MoveToFirst())
                {
                    int numberIndex = cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number);
                    userNumber = cursor.GetString(numberIndex);
                }
                (Fragment.ViewModel as IChallengeInviteService).UseSmsToSendInviteContact(new List<string> { string.IsNullOrEmpty(userNumber) ? string.Empty : userNumber });
                cursor.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private bool AppInstalled(Context c, string targetPackage)
        {
            PackageManager pm = c.PackageManager;
            try
            {
                PackageInfo info = pm.GetPackageInfo(targetPackage, PackageInfoFlags.MetaData);
            }
            catch (NameNotFoundException e)
            {
                return false;
            }
            return true;
        }
    }
}