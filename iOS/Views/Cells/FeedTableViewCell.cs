using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using FFImageLoading;
using Foundation;
using MapKit;
using SocialLadder.Enums;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.CurrentConstants;
using SocialLadder.iOS.CustomControlls;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.PlatformServices;
using SocialLadder.iOS.Services;
using SocialLadder.iOS.Helpers;
using SocialLadder.Models;
using UIKit;
using SocialLadder.iOS.UserInfo;
using SocialLadder.iOS.Interfaces.ViewControllers;

namespace SocialLadder.iOS.Views.Cells
{
    public partial class FeedTableViewCell : FeedBaseTableViewCell
    {
        public static readonly string ClassName = "FeedTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        private bool didClicked;
        private bool profileDidClicked;
        
        private UIViewController ViewController
        {
            get; set;
        }

        public static bool DidGetConstraintsLayout
        {
            get; private set;
        }

        static FeedTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);

        }

        protected FeedTableViewCell(IntPtr handle) : base(handle)
        {
            FeedItem = null;
            didClicked = false;
        }

        public static void GetConstraintsLayout(FeedTableViewCell cell)
        {

        }

        //builds attributed text for a list of users with comments (user name and comments are independently styled)
        //result intended to be used in a sinlge ui control
        //count param used to limit how many user comments to build (for example: so we can list the first 3 comments)
        NSMutableAttributedString BuildAttributedStringForComments(List<FeedEngagementModel> engagementList, int allowedTotal)
        {
            CommentsText.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.033f);
            List<FeedEngagementModel> commentList = new List<FeedEngagementModel>();
            foreach (FeedEngagementModel engagement in engagementList)
            {
                if (commentList.Count == allowedTotal)
                    break;
                if (engagement.EngagementType == "COMMENT")
                    commentList.Add(engagement);
            }
            string text = "";
            string sep = " ";
            string end = "\n";
            for (int i = 0; i < commentList.Count; i++)
            {
                FeedEngagementModel comment = commentList[i];
                text += StringWithEmojiConverter.ConvertEmojiFromServer(comment.UserName);
                text += sep + StringWithEmojiConverter.ConvertEmojiFromServer(comment.Notes);
                if (i + 1 < commentList.Count)
                    text += end;
            }
            int index = 0;
            var attString = new NSMutableAttributedString(text);
            var nameAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black,
                Font = UIFont.FromName("ProximaNova-Bold", UIScreen.MainScreen.Bounds.Width * 0.033f)
            };
            var commentAttributes = new UIStringAttributes
            {
                ForegroundColor = UIColor.Black
            };
            for (int i = 0; i < commentList.Count; i++)
            {
                FeedEngagementModel comment = commentList[i];
                attString.SetAttributes(nameAttributes.Dictionary, new NSRange(index, StringWithEmojiConverter.ConvertEmojiFromServer(comment.UserName).Length));
                index += StringWithEmojiConverter.ConvertEmojiFromServer(comment.UserName).Length;
                int length = sep.Length + (comment.Notes != null ? StringWithEmojiConverter.ConvertEmojiFromServer(comment.Notes).Length : 0);
                if (i + 1 < commentList.Count)
                    length += end.Length;
                attString.SetAttributes(commentAttributes.Dictionary, new NSRange(index, length));
                index += length;
            }
            return attString;
        }

        public override void ApplyStyles()
        {
            if (!FeedTableViewCell.DidGetConstraintsLayout)
                FeedTableViewCell.GetConstraintsLayout(this);

            Layer.BorderWidth = 4;
            Layer.BorderColor = UIColor.FromRGB(250, 250, 250).CGColor;
        }

        #region CustomChangeConstraints
        private void UpdateWebViewSize()
        {
            //try
            //{

            //    if (FeedItem.LayoutSections.Contains(Enums.FeedContentType.HtmlText))
            //    {
            //        ContentWebView.TranslatesAutoresizingMaskIntoConstraints = false;
            //        var constr = ContentWebView.Constraints;
            //        nfloat multiplier = (nfloat)Math.Round((ContentWebView.ScrollView.ContentSize.Width / ContentWebView.ScrollView.ContentSize.Height), 3);
            //        ContentWebView.AddConstraint(FeedConstraints.FeedControlFreeAspectRatio(ContentWebView, multiplier));
            //        ContentWebView.UpdateConstraintsIfNeeded();
            //        this.UpdateConstraintsIfNeeded();
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        private void UpdateControlConstraints(UIView currentView, NSLayoutConstraint currentViewAspectRatioConstraint)
        {
            try
            {
                currentView.Hidden = true;
                currentView.TranslatesAutoresizingMaskIntoConstraints = false;
                var aspectRatio = currentView.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Width && x.SecondAttribute == NSLayoutAttribute.Height).FirstOrDefault();
                if (aspectRatio != null && currentViewAspectRatioConstraint != null)
                {
                    currentView.RemoveConstraint(currentViewAspectRatioConstraint);

                }
                var height = currentView.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Height && x.Relation == NSLayoutRelation.LessThanOrEqual).FirstOrDefault();
                if (height != null)
                {
                    height.Constant = 0f;
                }
                else
                {
                    currentView.AddConstraint(FeedConstraints.FeedControlHeight(currentView));
                }
                currentView.UpdateConstraintsIfNeeded();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddLessControlConstraint(UIView currentView, NSLayoutConstraint currentViewAspectRatioConstraint, NSLayoutConstraint currentViewHeightConstraint)
        {
            try
            {
                currentView.Hidden = false;
                currentView.TranslatesAutoresizingMaskIntoConstraints = false;

                //if (currentViewHeightConstraint != null && currentView.RestorationIdentifier == "TopActionTextId")
                //{
                //    var commentSize = FeedItem.Header.ActionText.StringSize(UIFont.FromName("ProximaNova-Regular", ActorImage.Frame.Height * 0.32f)).ToRoundedCGSize();
                //    var textViewHeight = commentSize.Height;
                //    currentViewHeightConstraint.Constant = textViewHeight;
                //    return;
                //}
                //if (currentViewHeightConstraint != null && currentView.RestorationIdentifier == "ContentTextId")
                //{
                //    var commentSize = FeedItem.Content.Caption.StringSize(UIFont.FromName("ProximaNova-Regular", ActorImage.Frame.Height * 0.4f)).ToRoundedCGSize();
                //    var lineNumber = (nfloat)Math.Ceiling(commentSize.Width / (UIScreen.MainScreen.Bounds.Width * 0.705f));
                //    var textViewHeight = commentSize.Height * lineNumber + UIScreen.MainScreen.Bounds.Width * 0.02f;
                //    currentViewHeightConstraint.Constant = textViewHeight;
                //    return;
                //}
                //currentView.RemoveConstraint(currentViewAspectRatioConstraint);
                var height = currentView.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Height && x.Relation == NSLayoutRelation.LessThanOrEqual).ToArray();//.FirstOrDefault();
                if (height != null)
                {
                    currentView.RemoveConstraints(height);
                }

                var aspectRatio = currentView.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Width && x.SecondAttribute == NSLayoutAttribute.Height).FirstOrDefault();
                if (aspectRatio == null && currentViewAspectRatioConstraint != null)
                {
                    currentView.AddConstraint(currentViewAspectRatioConstraint);
                }
                currentView.UpdateConstraintsIfNeeded();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UpdateContentConstraints(FeedContentType[] content)
        {
            if (content == null)
            {
                content = new Enums.FeedContentType[] { };
            }
            TopActionTextHeight.Constant = 0f;
            if (FeedItem.Header == null || string.IsNullOrEmpty(FeedItem.Header.ActionText))
            {
                UpdateControlConstraints(TopActionText, null);
                //TopActionText.Hidden = true;
            }
            else
            {
                AddLessControlConstraint(TopActionText, null, TopActionTextHeight);
            }
            ContentTextHeight.Constant = 0;
            if (!content.Contains(Enums.FeedContentType.Text))
            {
                UpdateControlConstraints(ContentText, null);
            }
            else
            {
                AddLessControlConstraint(ContentText, null, ContentTextHeight);
            }

            if (!content.Contains(Enums.FeedContentType.HtmlText))
            {
                ResetControlConstraints(ContentWebView, ContentWebViewAspectRatio);
            }
            else
            {
                UpdateWebViewSize();
            }
            if (content.Contains(Enums.FeedContentType.Header))
            {
                AddLessControlConstraint(TopView, TopViewHeight, null);
                AddLessControlConstraint(TopPaddingView, TopPaddingViewAspect, null);
                AddLessControlConstraint(ContentHeaderView, null, null);
            }
            else if (!content.Contains(Enums.FeedContentType.Header))
            {
                UpdateControlConstraints(TopView, TopViewHeight);
                UpdateControlConstraints(ContentHeaderView, null);
                UpdateControlConstraints(TopPaddingView, TopPaddingViewAspect);
            }
            if (content.Contains(Enums.FeedContentType.Image))
            {
                //ResetControlConstraints(ContentImage, ContentImageAspectRatio);
                //ResetControlConstraints(GradientImageView, GradientImageViewAspect);
                nfloat width = 1f;
                nfloat height = 1f;
                FeedContentImageModel imageContent;
                if ((imageContent = FeedItem.BaseContent as FeedContentImageModel) != null)
                {
                    width = imageContent.ImageWidth;
                    height = imageContent.ImageHeight;
                }
                var multiplier = width / height;

                var widthConstaints = ContentImage.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Width).ToArray();
                ContentImage.RemoveConstraints(widthConstaints);
                ContentImage.AddConstraint(FeedConstraints.FeedControlFreeAspectRatio(ContentImage, multiplier));
                ContentImage.UpdateConstraints();
                GradientImageView.AddConstraint(FeedConstraints.FeedControlFreeAspectRatio(GradientImageView, multiplier));
                GradientImageView.UpdateConstraintsIfNeeded();
                ContentImage.Hidden = false;
                GradientImageView.Hidden = true;
            }

            //if (!content.Contains(Enums.FeedContentType.Image))
            //{
            //    var widthConstaints = ContentImage.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Width).ToArray();
            //    ContentImage.RemoveConstraints(widthConstaints);
            //    ContentImage.AddConstraint(FeedConstraints.FeedControlFreeAspectRatio(ContentImage, 1));
            //    ContentImage.UpdateConstraints();
            //    GradientImageView.AddConstraint(FeedConstraints.FeedControlFreeAspectRatio(GradientImageView, 1));
            //    GradientImageView.UpdateConstraintsIfNeeded();
            //    ContentImage.Hidden = true;
            //    GradientImageView.Hidden = true;
            //}

            if (!content.Contains(Enums.FeedContentType.Video))
            {
                UpdateControlConstraints(ContentVideo, ContentVideoAspectRatio);
            }
            else if (ContentVideo.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentVideo, ContentVideoAspectRatio, null);
            }
            if (!content.Contains(Enums.FeedContentType.Map))
            {
                UpdateControlConstraints(ContentMap, ContentMapAspectRatio);
            }
            else if (ContentMap.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentMap, ContentMapAspectRatio, null);
            }

            if (!content.Contains(Enums.FeedContentType.CollectionCheckIn))
            {
                UpdateControlConstraints(ContentCollectionCheckInView, ContentCollectionCheckInViewAspectRatio);
            }
            else if (ContentCollectionCheckInView.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentCollectionCheckInView, ContentCollectionCheckInViewAspectRatio, null);
            }

            if (!content.Contains(Enums.FeedContentType.CollectionAvatars))
            {
                UpdateControlConstraints(ContentCollectionAvatarView, ContentCollectionAvatarViewAspectRatio);
            }
            else if (ContentCollectionAvatarView.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentCollectionAvatarView, ContentCollectionAvatarViewAspectRatio, null);
            }
            if (!content.Contains(Enums.FeedContentType.ProductSold))
            {
                UpdateControlConstraints(ContentProductSold, ContentProductSoldAspectRatio);
            }
            else if (ContentProductSold.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentProductSold, ContentProductSoldAspectRatio, null);
            }

            if (!content.Contains(Enums.FeedContentType.FriendJoined))
            {
                UpdateControlConstraints(ContentFriendJoined, ContentFriendJoinedAspectRatio);
            }
            else if (ContentFriendJoined.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentFriendJoined, ContentFriendJoinedAspectRatio, null);
            }

            if (!content.Contains(Enums.FeedContentType.RewardClaimed))
            {
                UpdateControlConstraints(ContectRewardClaimed, ContectRewardClaimedAspectRatio);
            }
            else if (ContectRewardClaimed.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContectRewardClaimed, ContectRewardClaimedAspectRatio, null);
            }

            if (!content.Contains(Enums.FeedContentType.Share))
            {
                UpdateControlConstraints(ContentShare, ContentShareAspectRatio);
            }
            else if (ContentShare.Frame.Height <= 1)
            {
                AddLessControlConstraint(ContentShare, ContentShareAspectRatio, null);
            }

            if (!content.Contains(FeedContentType.Engagement))
            {
                UpdateControlConstraints(LikesView, LikesViewHeightAspecnRatio);
            }
            else if (LikesView.Frame.Height <= 1)
            {
                AddLessControlConstraint(LikesView, LikesViewHeightAspecnRatio, null);
            }

        }
        #endregion

        #region ResetControls
        public override void Reset()
        {
            didClicked = false;
            profileDidClicked = false;
            FeedItem = new FeedItemModel();
            HideRefreshLoader();
            var textControls = new List<UILabel>() { CommentsText, Actor_Name, Creation_Date, TopActionText, ContentText, PtsText,
                ProductSoldCount, ProductSoldTitle, ContentFriendJoinedUserName, LikesText };
            foreach (var control in textControls)
            {
                control.Text = string.Empty;
            }
            ReadCommentsButton.SetTitle(string.Empty, UIControlState.Normal);
            //ReadCommentButtonHeight.Constant = 0f;
            ReadCommentButtonHeight.Constant = 25f;
            TopActionTextHeight.Constant = 0f;
            ContentTextHeight.Constant = 0f;
            TopActionText.Hidden = false;
            MainBackgroundView.Hidden = true;
            MainBackgroundView.Image = null;
            ContentImageTextCaption.Hidden = true;
            ContentFriendJoinedPhoto.Image = null;
            ContentFriendJoinedBackground.Image = null;
            ProductSoldMainBackground.Image = null;
            ProductSoldUpBackground.Image = null;
            ActorImage.Image = null;

            GradientImageView.Image = UIImage.FromBundle("gradient");
            GradientImageView.Hidden = true;

            MainView.ClipsToBounds = false;
            Creation_Date.TextColor = UIColor.ScrollViewTexturedBackgroundColor;
            ContentImage.Hidden = CommentButton.Hidden = LikeButton.Hidden = true;
            ResetControlConstraints(ContentImage, ContentImageAspectRatio);
            ResetControlConstraints(GradientImageView, GradientImageViewAspect);
            ResetControlConstraints(ContentWebView, ContentWebViewAspectRatio);
            ChangeHeaderControlsToScreenResolution();
        }

        private void ChangeHeaderControlsToScreenResolution()
        {
            var widthScreen = UIScreen.MainScreen.Bounds.Width;
            var contentTopViewHeight = (widthScreen * 4.4f / 30f) - (widthScreen / 42f);
            Actor_NameHeight.Constant = contentTopViewHeight * 0.5f;
            Creation_DateHeight.Constant = contentTopViewHeight * 0.5f;
        }

        private void ResetControlConstraints(UIView view, NSLayoutConstraint _aspectConstraint)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            var heightConstraint = view.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Height).ToArray();//.FirstOrDefault();
            if (heightConstraint != null)
            {
                view.RemoveConstraints(heightConstraint);
            }
            var aspectConstraint = view.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Width && x.SecondAttribute == NSLayoutAttribute.Height).ToArray();//.FirstOrDefault();
            if (aspectConstraint != null)
            {
                view.RemoveConstraints(aspectConstraint);
            }
            if (view.Constraints.Contains(_aspectConstraint))
            {
                view.RemoveConstraint(_aspectConstraint);
            }
            view.UpdateConstraints();
        }
        #endregion


        private void OpenProfileFeed(object sender, EventArgs e)
        {
            //if (false == Platform.IsInternetConnectionAvailable())
            //{
            //    return;
            //}
            //if (profileDidClicked == false)
            //{
            //    if (Source?.ItemSelected?.Target is FeedViewController)
            //    {
            //        Source.SetSpinnerToImg(ActorImage);
            //    }

            //    OnLoadUserProfile(FeedItem.Header.ActorFeedURL, FeedItem.Header.ActorProfileURL);
            //    profileDidClicked = true;
            //}
        }

        protected void OnFileChanged(object sender, FileChangedArgs e)
        {
            if (FeedItem?.Header?.ProfilePicURL != null && String.Equals(e.LocalFile?.Url, FeedItem.Header.ProfilePicURL))
            {
                try
                {
                    ActorImage.Image = UIImage.FromFile(e.LocalFile.Path);
                    return;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.OnFileChanged(sender, {e}): {ex.Message}\n{ex.StackTrace}");
                }
            }
            if ((FeedItem.BaseContent as FeedContentImageModel) != null && String.Equals(e.LocalFile?.Url, (FeedItem.BaseContent as FeedContentImageModel).ImageURL))
            {
                try
                {
                    ContentImage.Image = UIImage.FromFile(e.LocalFile.Path);
                    return;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.OnFileChanged(sender, {e}): {ex.Message}\n{ex.StackTrace}");
                }
            }
            if ((FeedItem.BaseContent as FeedContentProfilePictureModel) != null && String.Equals(e.LocalFile?.Url, (FeedItem.BaseContent as FeedContentProfilePictureModel).ProfilePicURL))
            {
                try
                {
                    ContentImage.Image = UIImage.FromFile(e.LocalFile.Path);
                    return;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.OnFileChanged(sender, {e}): {ex.Message}\n{ex.StackTrace}");
                }
            }

        }
     
        public override void UpdateCellData(FeedItemModel item)
        {           
            FeedItem = item;

            FileCachingService.OnFileChanged -= OnFileChanged;
            FileCachingService.OnFileChanged += OnFileChanged;

            //LocalFile file = null;
            string imageUrl = string.Empty;

            UpdateControls();
            uiProfileButton.TouchUpInside -= OpenProfileFeed;
            uiProfileButton.TouchUpInside += OpenProfileFeed;
            MainView.SendSubviewToBack(MainBackgroundView);
            UpdateContentConstraints(item.LayoutSections);

            if (item.LayoutSections == null)
            {
                return;
            }

            foreach (Enums.FeedContentType section in item.LayoutSections)
            {
                switch (section)
                {
                    case Enums.FeedContentType.Header:
                        //file = FileCachingService.GetFile(item.Header.ProfilePicURL);
                        //if (file != null && String.Equals(file.Url, item.Header.ProfilePicURL))
                        //{
                        //    ActorImage.Image = UIImage.FromFile(file.Path);
                        //    imageUrl = string.Empty;
                        //    file = null;
                        //}
                        CGRect currentFrame = Actor_Name.Frame;
                        // CGSize max = CGSize ActorName.Frame.Width; // CGSizeMake(myLabel.frame.size.width, 500);
                        //CGSize expected = [myString sizeWithFont: myLabel.font constrainedToSize: max lineBreakMode: myLabel.lineBreakMode];
                        //currentFrame.size.height = expected.height;
                        //myLabel.frame = currentFrame;

                        NSString nsString = new NSString(item.Header.Actor + " " + item.Header.ActionText);

                        ImageService.Instance.LoadUrl(item.Header.ProfilePicURL).Into(ActorImage);

                        Actor_Name.Text = SocialLadder.iOS.Helpers.StringWithEmojiConverter.ConvertEmojiFromServer(item.Header.Actor + " " + item.Header.ActionText);

                        Actor_Name.AdjustsFontSizeToFitWidth = true;
                        Actor_Name.LineBreakMode = UILineBreakMode.Clip;


                        Creation_Date.Text = SL.TimeAgo(item.Header.CreationDate.Value);
                        //TopActionText.Text = item.Header.ActionText;
                        break;
                    case Enums.FeedContentType.Text:
                        ContentText.Text = item.TextQuote.TextQuoteText;
                        break;
                    case Enums.FeedContentType.HtmlText:
                        var htmlString = @"<p><span style=""color &#58;#000000;font-size:15px;font-family&#58;&quot;open sans&quot;, arial, sans-serif;text-align&#58;justify;background-color&#58;#ffffff;"">Proin sem urna, aliquet vel placerat quis, semper et mauris. Sed vel nunc lacus. Vestibulum vitae nisl eros. Donec ullamcorper sem nisl, in pellentesque quam tempus vel. Curabitur in felis nec urna placerat finibus. Donec ut nisi eu enim tincidunt luctus. Phasellus eleifend tortor est, nec maximus est aliquet sit amet. Praesent mollis massa id lacinia volutpat. Vestibulum eget odio sit amet enim fringilla pulvinar.</span></p>";
                        var nsdata = NSData.FromString(htmlString);
                        ContentWebView.LoadData(nsdata, "text/html", "UTF-8", new NSUrl(""));
                        break;
                    case Enums.FeedContentType.Image:
                        {
                            FeedContentProfilePictureModel profileContent;
                            FeedContentImageModel imageContent;
                            imageUrl = string.Empty;
                            if ((imageContent = item.BaseContent as FeedContentImageModel) != null)
                            {
                                //imageUrl = imageContent.ImageURL;
                                ImageService.Instance.LoadUrl(imageContent.ImageURL).Into(ContentImage);
                            }
                            else if ((profileContent = item.BaseContent as FeedContentProfilePictureModel) != null)
                            {
                                //imageUrl = profileContent.ProfilePicURL;
                                ImageService.Instance.LoadUrl(profileContent.ProfilePicURL).Into(ContentImage);
                            }
                            //if (!String.IsNullOrEmpty(imageUrl))
                            //{
                            //    file = FileCachingService.GetFile(imageUrl);
                            //}
                            //if (file != null && String.Equals(file.Url, imageUrl))
                            //{
                            //    ContentImage.Image = UIImage.FromFile(file.Path);
                            //}
                            //Need handle, if this is Insta



                            ContentTextHeight.Constant = 0f;
                            TopActionTextHeight.Constant = 20f;
                            Creation_Date.Text = item.Header != null && SL.TimeAgo(item.Header.CreationDate.Value) != null ? SL.TimeAgo(item.Header.CreationDate.Value) : "ago";
                            //CreationDate.TextColor = UIColor.Blue;
                            if (item.LayoutSections.Count() <= 1)
                            {
                                ContentImageTextCaption.Hidden = false;
                                MainView.ClipsToBounds = true;

                                if (item?.BaseContent is FeedContentImageModel)
                                {
                                    var baseContent = (item.BaseContent as FeedContentImageModel);
                                    if (!string.IsNullOrEmpty(baseContent.ImageCaption))
                                    {
                                        ContentImageTextCaption.Text = baseContent.ImageCaption;
                                        GradientImageView.Hidden = false;
                                    }
                                    else
                                    {
                                        ContentImageTextCaption.Text = string.Empty;
                                        GradientImageView.Hidden = true;
                                    }
                                }
                                TopActionTextHeight.Constant = ReadCommentButtonHeight.Constant = 0f;
                            }
                        }
                        break;
                    case Enums.FeedContentType.Video:
                        break;
                    case Enums.FeedContentType.Map:
                        {
                            FeedContentMapModel mapContent = item.BaseContent as FeedContentMapModel;
                            if (mapContent != null)
                            {
                                AddAnnotationToMapView(mapContent.Lat, mapContent.Long, mapContent.LocationName);
                                SmileButton.Hidden = true;
                                PtsText.Hidden = true;
                            }
                        }
                        break;
                    case Enums.FeedContentType.CollectionCheckIn:
                        break;
                    case Enums.FeedContentType.CollectionAvatars:
                        break;
                    case Enums.FeedContentType.ProductSold:
                        {
                            ProductSoldCount.Text = item?.Content?.OfferCost ?? string.Empty;
                            //ProductSoldCount.Text = "2";
                            ProductSoldTitle.Text = "SOLD";

                            if (FeedItem.BaseContent?.TapAction != null)
                            {
                                TextProductSold.Hidden = false;
                                IconProductSold.Hidden = false;
                                ButtonProductSold.Hidden = false;
                                BackgroundProductSold.Hidden = false;
                            }
                            else
                            {
                                TextProductSold.Hidden = true;
                                IconProductSold.Hidden = true;
                                ButtonProductSold.Hidden = true;
                                BackgroundProductSold.Hidden = true;
                            }
                        }
                        break;
                    case Enums.FeedContentType.FriendJoined:
                        {
                            FeedContentProfilePictureModel profileContent;
                            imageUrl = string.Empty;
                            if ((profileContent = item.BaseContent as FeedContentProfilePictureModel) != null)
                            {
                                //imageUrl = profileContent.ProfilePicURL;
                                //file = FileCachingService.GetFile(imageUrl);
                                ImageService.Instance.LoadUrl(profileContent.ProfilePicURL).Into(ContentFriendJoinedPhoto);
                                ContentFriendJoinedUserName.Text = StringWithEmojiConverter.ConvertEmojiFromServer(profileContent.Caption);
                            }

                            if (FeedItem.BaseContent?.TapAction != null)
                            {
                                ButtonInviteFriends.Hidden = false;
                                TextContentFriendJoined.Hidden = false;
                                IconContentFriendJoined.Hidden = false;
                                BackgroundContentFriendJoined.Hidden = false;
                            }
                            else
                            {
                                ButtonInviteFriends.Hidden = true;
                                TextContentFriendJoined.Hidden = true;
                                IconContentFriendJoined.Hidden = true;
                                BackgroundContentFriendJoined.Hidden = true;
                            }
                            //if (file != null && String.Equals(file.Url, imageUrl))
                            //{
                            //    ContentImage.Image = UIImage.FromFile(file.Path); ;
                            //}
                        }
                        break;
                    case Enums.FeedContentType.RewardClaimed:
                        {
                            FeedContentOfferModel offerContent = item.BaseContent as FeedContentOfferModel;
                            MainBackgroundView.Hidden = false;
                            MainBackgroundView.Image = UIImage.FromBundle("RewardClaimedBackground");
                            if (offerContent != null)
                            {
                                ContectRewardClaimedTitle.Text = offerContent.OfferTitle;
                            }
                        }
                        break;
                    case Enums.FeedContentType.Share:
                        break;
                    case Enums.FeedContentType.Engagement:
                        {
                            LineSeparator.Hidden = true;
                            if (item.ActionDictionary != null)
                            {
                                CommentButton.Hidden = !item.ActionDictionary.ContainsKey("Comment");
                                if (item.ActionDictionary.ContainsKey("Boost") || (item.ActionDictionary.ContainsKey("Like")))
                                {
                                    LikeButton.TintColor = item.DidLike ? Colors.FeedCellLikeButtonSelectedColor : Colors.FeedCellLikeButtonUnselectedColor;
                                    LikeButton.Hidden = false;
                                    LikesText.Text = item.Likes + " likes";
                                }



                                PtsText.Text = string.Empty;
                                PointsIconWidthConstraint.Constant = 0f;


                                if (item?.Points != null)
                                {
                                    PtsText.Text = $"{item.Points} pts";
                                    PointsIconWidthConstraint.Constant = 25f;
                                }

                                if ((!item.ActionDictionary.ContainsKey("Comment")) && (!item.ActionDictionary.ContainsKey("Boost")))
                                {
                                    UpdateControlConstraints(LikesView, LikesViewHeightAspecnRatio);
                                }

                                //if (CommentButton.Hidden)
                                //{
                                //    //var test = this.Constraints.Where(x => x.FirstAttribute == NSLayoutAttribute.Height && x.FirstItem == LikesView).ToArray();
                                //    //if (test != null)
                                //    //{
                                //    //    LikesView.RemoveConstraints(test);
                                //    //}
                                //    this.AddConstraint(NSLayoutConstraint.Create(LikesView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 0, 0));
                                //    this.LayoutIfNeeded();
                                //}
                            }
                            if (item.HasComments)
                            {
                                CommentsText.AttributedText = BuildAttributedStringForComments(item.FilteredEngagementList, 3);
                                if (item.FilteredEngagementList.Count > 3)
                                {
                                    ReadCommentButtonHeight.Constant = 25f;
                                    ReadCommentsButton.SetTitle("Read all " + item.FilteredEngagementList.Count + " comments", UIControlState.Normal);
                                }
                                if (item.FilteredEngagementList.Count <= 3)
                                {
                                    ReadCommentButtonHeight.Constant = 0f;
                                }
                            }
                            else
                                ReadCommentButtonHeight.Constant = 0f;
                        }
                        break;
                }
            }

            if (!item.LayoutSections.Contains(Enums.FeedContentType.Engagement))
            {
                ReadCommentButtonHeight.Constant = 0f;
                CommentsText.Text = string.Empty;
            }
            if (item.TextQuote == null)
            {
                return;
            }
            if (item.TextQuote.IsModified)
            {
                ShowRefreshLoader();
                ReadAllComments(FeedItem);
                HideRefreshLoader();
            }
            if (FeedItem.ActionDictionary != null && (FeedItem.ActionDictionary.ContainsKey("Report This") || FeedItem.ActionDictionary.ContainsKey("Flag It")))
            {
                vDotsTop.Hidden = false;
                vDotsTopHeight.Constant = SizeConstants.ScreenWidth * 0.11f;
            }
            else
            {
                vDotsTop.Hidden = true;
                vDotsTopHeight.Constant = 0;
            }

            ContentWebView.Reload();
        }

        #region UpdateControls
        private void UpdateChangedControls()
        {
            if (FeedItem.DidLike)
            {
                LikeButton.TintColor = Colors.FeedCellLikeButtonSelectedColor;
            }
            else
            {
                LikeButton.TintColor = Colors.FeedCellLikeButtonUnselectedColor;
            }
        }

        private void UpdateControls()
        {
            //MainBackgroundView.Image = UIImage.FromBundle("RewardClaimedBackground");
            MainBackgroundView.Image = null;
            //Can be deleted
            ContentImage.Image = UIImage.FromBundle("share-icon_white");

            //CommentButton.SetImage(UIImage.FromBundle("share-icon_white"), UIControlState.Normal);
            //LikeButton.SetImage(UIImage.FromBundle("share-icon_white"), UIControlState.Normal);

            SmileButton.SetImage(UIImage.FromBundle("points-icon_off"), UIControlState.Normal);
            ButtonTwitter.SetImage(UIImage.FromBundle("twitter-logo_gray"), UIControlState.Normal);
            ButtonFacebook.SetImage(UIImage.FromBundle("fb-logo_gray"), UIControlState.Normal);

            ButtonTwitter.Layer.BorderColor = ButtonFacebook.Layer.BorderColor = Colors.FeedCellFBButtonBorderColor.CGColor;
            ButtonTwitter.TintColor = ButtonFacebook.TintColor = UIColor.LightGray;


            ContentFriendJoinedPhoto.Image = UIImage.FromBundle("product_sold_background");

            IconProductSold.Image = UIImage.FromBundle("ticket-icon_on");
            IconContentFriendJoined.Image = UIImage.FromBundle("invite-icon_on");
            IconContentFriendJoined.ContentMode = IconProductSold.ContentMode = UIViewContentMode.ScaleAspectFit;
            IconProductSold.Image = IconProductSold.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IconContentFriendJoined.Image = IconContentFriendJoined.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IconContentFriendJoined.TintColor = IconProductSold.TintColor = Colors.FeedCellButtonLinkColor;
            //ContentFriendJoinedUserName.Layer.MasksToBounds = true;        
            UpdateChangedControls();


            LikeButton.TintColor = CommentButton.TintColor = UIColor.LightGray;

            ContentFriendJoinedBackground.Image = UIImage.FromBundle("contentFriend_background");
            ProductSoldMainBackground.Image = UIImage.FromBundle("product-sold_background");
            ProductSoldUpBackground.Image = UIImage.FromBundle("ticket-sold");
            ContectRewardClaimedRightBackground.Image = UIImage.FromBundle("contect_rewardClaimed_Background");

            MainView.Layer.BorderColor = UIColor.FromRGB(243, 243, 243).CGColor;
            //MainView.ClipsToBounds = true;
            //AddRefreshImage();
        }

        private void UpdateSharedButtons()
        {
            var cornerRadius = (UIScreen.MainScreen.Bounds.Width - 10f) * 0.04f;
            ButtonTwitter.Layer.CornerRadius = ButtonFacebook.Layer.CornerRadius = cornerRadius;
            ButtonTwitter.Layer.BorderWidth = ButtonFacebook.Layer.BorderWidth = 1f;
            var imageInset = new UIEdgeInsets(cornerRadius * 0.5f, cornerRadius * 0.5f, cornerRadius * 0.5f, cornerRadius * 0.5f);
            ButtonTwitter.ImageEdgeInsets = ButtonFacebook.ImageEdgeInsets = imageInset;
        }

        //public void AddRefreshImage()
        //{
        //    RefreshLoader.Image = UIImage.FromBundle("loading-indicator");
        //    Platform.AnimateRotation(RefreshLoader);
        //}

        private void AddAnnotationToMapView(double latitude, double longitude, string title)
        {
            var coordinate = new CLLocationCoordinate2D(latitude, longitude);
            ContentMapView.AddAnnotations(new MKPointAnnotation()
            {
                Title = title,
                Coordinate = coordinate
            });
            CLLocationCoordinate2D mapCenter = coordinate;
            MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 500, 500);
            ContentMapView.CenterCoordinate = mapCenter;
            ContentMapView.Region = mapRegion;
        }
        #endregion

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            ContentFriendJoinedPhoto.Layer.CornerRadius = UIScreen.MainScreen.Bounds.Width * 0.11875f;
            ContentFriendJoinedPhoto.Layer.BorderColor = UIColor.Gray.CGColor;
            ContentFriendJoinedPhoto.Layer.BorderWidth = 1.5f;
            ContectRewardClaimedRightBackground.Layer.CornerRadius = 5f;

            ContentFriendJoinedUserName.Layer.CornerRadius = 5f;

            UpdateSharedButtons();

            TextProductSold.Font = UIFont.FromName("ProximaNova-Regular", TextProductSold.Frame.Height * 0.6f);

            ButtonInviteFriends.ContentMode = UIViewContentMode.ScaleAspectFit;

            Label_ContectReward_Claimed.Font = UIFont.FromName("ProximaNova-Regular", ContectRewardClaimedRightBackground.Frame.Height * 0.1f);
            RewardClaimedCount.Font = ButtonRewardClaimed.Font = UIFont.FromName("ProximaNova-Regular", ContectRewardClaimedRightBackground.Frame.Height * 0.085f);
            ContectRewardClaimedTitle.Font = UIFont.FromName("ProximaNova-Bold", UIScreen.MainScreen.Bounds.Width * 0.055f);
            ContentFriendJoinedUserName.Font = UIFont.FromName("ProximaNova-Regular", ContentFriendJoinedPhoto.Frame.Height * 0.17f);
            TextContentFriendJoined.Font = UIFont.FromName("ProximaNova-Regular", ContentFriendJoinedPhoto.Frame.Height * 0.15f);
            LabelFriendsWelcome.Font = UIFont.FromName("ProximaNova-Bold", ContentFriendJoinedPhoto.Frame.Height * 0.151f);

            Actor_Name.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.035f);
            Creation_Date.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.032f);
            ContentImageTextCaption.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.04f);
            //TopActionText.Font = CreationDate.Font = UIFont.FromName("ProximaNova-Regular", ActorImage.Frame.Height * 0.32f);

            ProductSoldTitle.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.09f);
            ProductSoldCount.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.055f);
            TextProductSold.Font = UIFont.FromName("ProximaNova-Regular", ButtonProductSold.Frame.Height * 0.52f);

            ReadCommentsButton.Font = UIFont.FromName("ProximaNova-Bold", CommentButton.Frame.Height * 0.55f);

            LikesText.Font = PtsText.Font = UIFont.FromName("ProximaNova-Regular", CommentButton.Frame.Height * 0.6f);

            //ContentText.Font = UIFont.FromName("ProximaNova-Regular", ActorImage.Frame.Height * 0.4f);
            TopActionText.Font = ContentText.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.035f);

            MainView.Layer.BorderWidth = 1f;
            MainView.Layer.CornerRadius = 2f;

            //if (FeedItem.LayoutSections.Count() <= 1 && !string.IsNullOrEmpty((FeedItem.BaseContent as FeedContentImageModel)?.ImageCaption))
            //{
            //    GradientLayer.Frame = ContentImage.Frame;
            //    ContentImage.Layer.AddSublayer(GradientLayer);  
            //    //MainView.BringSubviewToFront(ContentImage);
            //    //MainView.AddSubview(ContentImage);
            //}

            ContentView.UpdateConstraints();
            ContentView.SetNeedsUpdateConstraints();
        }

        #region Reload
        //public override void HideRefreshLoader()
        //{
        //    base.HideRefreshLoader();
        //    RefreshView.Hidden = RefreshLoader.Hidden = true;
        //    MainView.SendSubviewToBack(RefreshView);
        //}

        //public override void ShowRefreshLoader()
        //{
        //    base.ShowRefreshLoader();
        //    RefreshView.Hidden = RefreshLoader.Hidden = false;
        //    MainView.BringSubviewToFront(RefreshView);
        //}
        #endregion

        #region Events
        partial void LikeButton_TouchUpInside(UIButton sender)
        {
            if (!FeedItem.DidLike)
            {
                LikeButton.TintColor = Colors.FeedCellLikeButtonSelectedColor;
                FeedItem.DidLike = true;
                FeedItem.Likes++;
                LikesText.Text = FeedItem.Likes + " likes";
                if (FeedItem.ActionDictionary.ContainsKey("Boost"))
                {
                    string url = FeedItem.ActionDictionary["Boost"].ActionParamDict["SubmissionURL"];
                    SL.Manager.PostDialog(url, null, LikePostResponse);
                    return;
                }
                if (FeedItem.ActionDictionary.ContainsKey("Like"))
                {
                    string url = FeedItem.ActionDictionary["Like"].ActionParamDict["SubmissionURL"];
                    SL.Manager.PostDialog(url, null, LikePostResponse);
                    return;
                }
            }
            /*
            else
            {
                LikeButton.TintColor = Colors.FeedCellLikeButtonUnselectedColor;
                FeedItem.DidLike = false;
            }
            */
        }

        void LikePostResponse(UpdatedFeedResponceModel response)
        {

        }

        async partial void CommentButton_TouchUpInside(UIButton sender)
        {
            if (FeedItem.ActionDictionary.ContainsKey("Comment"))
            {
                string caption = FeedItem.ActionDictionary["Comment"].ActionParamDict["Caption"];
                FeedItem.PostedComment = await Platform.ShowAlertTextInput(null, caption, "OK", "Cancel");
                if (string.IsNullOrEmpty(FeedItem.PostedComment))
                {
                    return;
                }
                PostDialog(FeedItem.PostedComment);
            }
        }

        partial void BtnDotsTop_TouchUpInside(UIButton sender)
        {
            if (FeedItem.ActionDictionary.ContainsKey("Report This") || FeedItem.ActionDictionary.ContainsKey("Flag It"))
            {
                UIAlertController alertController = UIAlertController.Create("Actions", null, UIAlertControllerStyle.ActionSheet);

                if (FeedItem.ActionDictionary.ContainsKey("Report This"))
                    alertController.AddAction(UIAlertAction.Create("Report", UIAlertActionStyle.Destructive, (a) => PostReportIt(FeedItem.ActionDictionary["Report This"])));

                if (FeedItem.ActionDictionary.ContainsKey("Flag It"))
                    alertController.AddAction(UIAlertAction.Create("Flag It", UIAlertActionStyle.Default, (a) => PostReportIt(FeedItem.ActionDictionary["Flag It"])));

                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel")));

                (ViewController as UIViewController)?.PresentViewController(alertController, true, null);
            }
        }

        private void PostReportIt(FeedActionModel feedAction)
        {
            ActionHandlerService service = new ActionHandlerService();
            service.HandleActionAsync(feedAction, FeedItem.ActionType);
        }


        private void PostDialog(string comment)
        {
            if (FeedItem.ActionDictionary.ContainsKey("Comment"))
            {
                UserInputModel input = new UserInputModel();
                input.UserInputText = comment;
                input.ButtonResponse = 1;

                string url = FeedItem.ActionDictionary["Comment"].ActionParamDict["SubmissionURL"];

                //CommentPostResponse(new ActionResponseModel() { ResponseCode = 1 });
                SL.Manager.PostDialog(url, input, CommentPostResponse);
            }
        }

        void CommentPostResponse(UpdatedFeedResponceModel response)
        {
            if (response.ResponseCode > 0)
            {
                if (FeedItem.TextQuote == null)
                    FeedItem.TextQuote = new FeedTextQuote();
                if (FeedItem.FilteredEngagementList == null)
                    FeedItem.FilteredEngagementList = new List<FeedEngagementModel>();
                if (!FeedItem.LayoutSections.Contains(Enums.FeedContentType.Engagement))
                {
                    List<Enums.FeedContentType> sections = FeedItem.LayoutSections.ToList();
                    sections.Add(Enums.FeedContentType.Engagement);
                    FeedItem.LayoutSections = sections.ToArray();
                }
                //FeedEngagementModel engagement = new FeedEngagementModel();
                //engagement.EngagementType = "COMMENT";
                //engagement.UserName = SL.Profile.UserName;
                //engagement.Notes = FeedItem.PostedComment;

                //FeedItem.FilteredEngagementList.Add(engagement);

                Source.UpdateComments(FeedItem, FeedItem.PostedComment);

                CommentsText.AttributedText = BuildAttributedStringForComments(FeedItem.FilteredEngagementList, FeedItem.FilteredEngagementList.Count);
                ReadCommentsButton.Hidden = true;
                //Source.UpdateCommentView(FeedItem);

            }
        }

        partial void ReadCommentsButton_TouchUpInside(UIButton sender)
        {
            ReadAllComments(FeedItem);
            Source.UpdateComments(FeedItem);
            //Source.UpdateCommentView(FeedItem);
        }

        public void ReadAllComments(FeedItemModel feedItem)
        {
            CommentsText.AttributedText = BuildAttributedStringForComments(feedItem.FilteredEngagementList, feedItem.FilteredEngagementList.Count);
            ReadCommentButtonHeight.Constant = 0f;
        }

        public void ScrollToItem(NSIndexPath indexPath, UITableViewScrollPosition atScrollPosition, bool animated)
        {
            FeedTableView.ScrollToRow(indexPath, atScrollPosition, animated);
        }

        partial void ContentVideoButtonPlay_TouchUpInside(UIButton sender)
        {
        }

        partial void ButtonProductSold_TouchUpInside(UIButton sender)
        {
            if (didClicked)
            {
                return;
            }

            didClicked = true;
            ActionHandlerService service = new ActionHandlerService();
            service.HandleActionAsync(FeedItem.BaseContent?.TapAction, FeedItem.ActionType);
        }

        partial void btnOpenInvitedProfile_TouchUpInside(UIButton sender)
        {
            if (false == Platform.IsInternetConnectionAvailable())
            {
                return;
            }

            if (FeedItem.BaseContent?.TapAction2?.ActionScreen != "FEED")
            {
                //alert feed profile tap action isn't correct
                return;
            }

            string feedURL = string.Empty;
            string profileURL = string.Empty;

            bool isFeedURLGot = FeedItem.BaseContent?.TapAction2?.ActionParamDict?.TryGetValue("FeedURL", out feedURL) ?? false;
            bool isProfileURLGot = FeedItem.BaseContent?.TapAction2?.ActionParamDict?.TryGetValue("profileURL", out profileURL) ?? false;

            if (feedURL == string.Empty)
            {
                return;
            }

            OnLoadUserProfile(feedURL, feedURL);
        }

        partial void ButtonInviteFriends_TouchUpInside(UIButton sender)
        {
            if (didClicked)
            {
                return;
            }

            if (!didClicked)
            {
                didClicked = true;
                ActionHandlerService service = new ActionHandlerService();
                service.HandleActionAsync(FeedItem.BaseContent?.TapAction, FeedItem.ActionType);
            }
        }

        partial void ButtonTwitter_TouchUpInside(UIButton sender)
        {

        }

        partial void ButtonFacebook_TouchUpInside(UIButton sender)
        {

        }

        partial void ButtonRewardClaimed_TouchUpInside(UIButton sender)
        {

        }
        #endregion

        //public override nfloat GetViewHeight()
        //{
        //    return RefreshView.Frame.Bottom;//headerView.Frame.Height + ContentImage.Frame.Height + CommentButton.Frame.Height + CommentsText.Frame.Height + ReadCommentsButton.Frame.Height;
        //}
    }
}
