using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Helpers
{
    public static class Extentions
    {
        public static NSDictionary RemoveNullValues(this NSDictionary dictionary)
        {

            NSMutableDictionary filteredDictionary = new NSMutableDictionary();

            foreach (var key in dictionary.Keys)
            {
                NSObject nsObject = dictionary.ObjectForKey(key);
                if (nsObject.Description == "<null>")
                    continue;
                if (nsObject.GetType() == typeof(NSDictionary))
                {
                    nsObject = ((NSDictionary)nsObject).RemoveNullValues();
                }
                filteredDictionary.Add(key, nsObject);
            }

            return filteredDictionary;
        }
    }
}