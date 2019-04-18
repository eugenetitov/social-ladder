using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Helpers
{
   public static class ConstraintsGenerator
    {
        public static bool GenerateConstraint(this UIView view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, UIView view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant)
        {
            NSLayoutConstraint constraint = view1.Constraints.Where(c => c.FirstAttribute == attribute1).FirstOrDefault();
            if (constraint != null)
            {
                view1.RemoveConstraint(constraint);
            }
            constraint = NSLayoutConstraint.Create(view1, attribute1, relation, view2, attribute2, multiplier, constant);
            constraint.Active = true;
            view1.AddConstraint(constraint);
            view1.UpdateConstraints();
            
            return true;
        }
    }
}