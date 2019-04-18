using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace SocialLadder.Droid.CustomControls
{
    public class AggRecyclerLayout : MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView
    {
        public AggRecyclerLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            this.SetLayoutManager(new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false));
        }

        public AggRecyclerLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {

            this.SetLayoutManager(new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false));
        }

        public AggRecyclerLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {

            this.SetLayoutManager(new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false));
        }

        public AggRecyclerLayout(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle, adapter)
        {

            this.SetLayoutManager(new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false));
        }

    }
}