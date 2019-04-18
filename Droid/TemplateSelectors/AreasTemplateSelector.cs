using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using SocialLadder.Models.LocalModels.Areas;

namespace SocialLadder.Droid.TemplateSelectors
{
    public class AreasTemplateSelector : IMvxTemplateSelector
    {
        private readonly Dictionary<Type, int> _itemsTypeDictionary = new Dictionary<Type, int>
        {
            [typeof(LocalAreasModel)] = Resource.Layout.areas_collection_template,
            [typeof(LocalCurrentAreaModel)] = Resource.Layout.areas_current_item_collection_template
        };

        public int ItemTemplateId { get; set; }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            return _itemsTypeDictionary[forItemObject.GetType()];
        }
    }
}