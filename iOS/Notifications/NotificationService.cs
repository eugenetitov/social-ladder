using System;
using Foundation;
using UIKit;
using UserNotifications;

namespace SocialLadder.iOS.Notifications
{
    public class NotificationService : UNNotificationServiceExtension
    {
        #region Constructors
        public NotificationService(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
        {
            // Get file URL
            var attachementPath = request.Content.UserInfo.ObjectForKey(new NSString("my-attachment"));
            var url = new NSUrl(attachementPath.ToString());

            // Download the file
            var localURL = new NSUrl("PathToLocalCopy");

            // Create attachment
            var attachmentID = "image";
            var options = new UNNotificationAttachmentOptions();
            NSError err;
            var attachment = UNNotificationAttachment.FromIdentifier(attachmentID, localURL, options, out err);

            // Modify contents
            var content = request.Content.MutableCopy() as UNMutableNotificationContent;
            content.Attachments = new UNNotificationAttachment[] { attachment };

            // Display notification
            contentHandler(content);
        }

        public override void TimeWillExpire()
        {
            // Handle service timing out
        }
        #endregion
    }
}