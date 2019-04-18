// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    [Register ("NetworksViewController")]
    partial class NetworksViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnPrivacyPolicy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnTermsService { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DoneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView DoneButtonStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DoneButtonView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView FacebookConnectedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FacebookLoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView InstagramConnectedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton InstagramLoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LogoutButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView prb_Value { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView ProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ProgressBarandNextButtonView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView ProgressBarNextButtonStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ScoreFill { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ScoreImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TwitterConnectedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TwitterLoginButton { get; set; }

        [Action ("BtnPrivacyPolicy_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnPrivacyPolicy_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnTermsService_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnTermsService_TouchUpInside (UIKit.UIButton sender);

        [Action ("DoneButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DoneButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("FacebookLoginButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FacebookLoginButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("InstagramLoginButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void InstagramLoginButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("LogoutButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LogoutButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("NextButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NextButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TwitterLoginButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TwitterLoginButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnPrivacyPolicy != null) {
                BtnPrivacyPolicy.Dispose ();
                BtnPrivacyPolicy = null;
            }

            if (BtnTermsService != null) {
                BtnTermsService.Dispose ();
                BtnTermsService = null;
            }

            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }

            if (DoneButtonStackView != null) {
                DoneButtonStackView.Dispose ();
                DoneButtonStackView = null;
            }

            if (DoneButtonView != null) {
                DoneButtonView.Dispose ();
                DoneButtonView = null;
            }

            if (FacebookConnectedImage != null) {
                FacebookConnectedImage.Dispose ();
                FacebookConnectedImage = null;
            }

            if (FacebookLoginButton != null) {
                FacebookLoginButton.Dispose ();
                FacebookLoginButton = null;
            }

            if (InstagramConnectedImage != null) {
                InstagramConnectedImage.Dispose ();
                InstagramConnectedImage = null;
            }

            if (InstagramLoginButton != null) {
                InstagramLoginButton.Dispose ();
                InstagramLoginButton = null;
            }

            if (LogoutButton != null) {
                LogoutButton.Dispose ();
                LogoutButton = null;
            }

            if (NextButton != null) {
                NextButton.Dispose ();
                NextButton = null;
            }

            if (prb_Value != null) {
                prb_Value.Dispose ();
                prb_Value = null;
            }

            if (ProgressBar != null) {
                ProgressBar.Dispose ();
                ProgressBar = null;
            }

            if (ProgressBarandNextButtonView != null) {
                ProgressBarandNextButtonView.Dispose ();
                ProgressBarandNextButtonView = null;
            }

            if (ProgressBarNextButtonStackView != null) {
                ProgressBarNextButtonStackView.Dispose ();
                ProgressBarNextButtonStackView = null;
            }

            if (ScoreFill != null) {
                ScoreFill.Dispose ();
                ScoreFill = null;
            }

            if (ScoreImage != null) {
                ScoreImage.Dispose ();
                ScoreImage = null;
            }

            if (TwitterConnectedImage != null) {
                TwitterConnectedImage.Dispose ();
                TwitterConnectedImage = null;
            }

            if (TwitterLoginButton != null) {
                TwitterLoginButton.Dispose ();
                TwitterLoginButton = null;
            }
        }
    }
}