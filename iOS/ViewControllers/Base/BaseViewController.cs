using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Base
{
    public partial class BaseViewController<T> : MvxViewController<T> where T : class, IMvxViewModel
    {
        public BaseViewController(IntPtr handle)
        {
            Handle = handle;
        }
    }
}