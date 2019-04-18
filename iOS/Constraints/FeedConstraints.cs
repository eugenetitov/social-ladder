using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
    public static class FeedConstraints
    {
        public static NSLayoutConstraint FeedControlHeight(NSObject view)
        {
            var constaint = NSLayoutConstraint.Create(view, NSLayoutAttribute.Height, NSLayoutRelation.LessThanOrEqual, 0f, 0f);
            constaint.Priority = 1000;
            return constaint;
        }

        public static NSLayoutConstraint FeedControlFreeAspectRatio(NSObject view, nfloat multiplier)
        {
            var constaint = NSLayoutConstraint.Create(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, multiplier, 0f);
            constaint.Priority = 800;
            return  constaint;
           // return NSLayoutConstraint.Create(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, multiplier, 0f);
        }

        public static NSLayoutConstraint FeedFooterViewHeight(UIView spinner, UIView view)
        {
            var constaint = NSLayoutConstraint.Create(spinner, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0.25f, 1.0f);
            constaint.Priority = 800;
            return constaint;
        }

        public static NSLayoutConstraint FeedFooterViewWidth(UIView spinner)
        {
            var constaint = NSLayoutConstraint.Create(spinner, NSLayoutAttribute.Width, NSLayoutRelation.Equal, spinner, NSLayoutAttribute.Height, 1.0f, 0.0f);
            constaint.Priority = 800;
            return constaint;
        }

        public static NSLayoutConstraint FeedFooterViewX(UIView spinner, UIView view)
        {
            var constaint = NSLayoutConstraint.Create(spinner, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1.0f, 0.0f);
            constaint.Priority = 800;
            return constaint;
        }

        public static NSLayoutConstraint FeedFooterTopToView(UIView spinner, UIView view)
        {
            var constaint = NSLayoutConstraint.Create(spinner, NSLayoutAttribute.Top, NSLayoutRelation.Equal, view, NSLayoutAttribute.Top, 1.0f, 0.0f);
            constaint.Priority = 800;
            return constaint;
        }

        public static NSLayoutConstraint FeedFooterViewY(UIView spinner, UIView view)
        {
            var constaint = NSLayoutConstraint.Create(spinner, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1.0f, 0.0f);
            constaint.Priority = 800;
            return constaint;
        }
    }
}