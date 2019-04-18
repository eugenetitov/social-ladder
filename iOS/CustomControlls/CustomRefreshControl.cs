using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.CustomControlls
{
    public class CustomRefreshControl : UIRefreshControl
    {
        private UIImageView CustomImageView { get; set; }

        public CustomRefreshControl()
        {
            
        }

        public void AddImage(UIImage image)
        {
            this.TintColor = UIColor.Clear;
            CustomImageView = new UIImageView();
            CustomImageView.Image = image;
            this.InsertSubview(CustomImageView, 0);
            CustomImageView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddConstraint(NSLayoutConstraint.Create(CustomImageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1.0f, 0f));
            this.AddConstraint(NSLayoutConstraint.Create(CustomImageView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            this.AddConstraint(NSLayoutConstraint.Create(CustomImageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.1f, 0f));
            this.AddConstraint(NSLayoutConstraint.Create(CustomImageView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, CustomImageView, NSLayoutAttribute.Width, 1.0f, 0f));
            Platform.AnimateRotation(CustomImageView);
        }
    }
}