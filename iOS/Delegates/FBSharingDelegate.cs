using System;
using Facebook.ShareKit;
using Foundation;

namespace SocialLadder.iOS.Delegates
{
    public class FBSharingDelegate : ISharingDelegate
    {
        public IntPtr Handle { get; }

        public FBSharingDelegate()
        {
            Console.WriteLine("FBDelegate created");
        }

        public void DidComplete(ISharing sharer, NSDictionary results)
        {
            Console.WriteLine("DidComplete: " + results.DebugDescription);
        }

        public void DidFail(ISharing sharer, NSError error)
        {
            Console.WriteLine("DidFail: " + error.DebugDescription);
        }

        public void DidCancel(ISharing sharer)
        {
            Console.WriteLine("DidCancel");
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }
}