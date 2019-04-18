using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models
{
    public class LocalAreaModel : AreaModel
    {
        public bool IsSuggestedArea { get; set; }
        public MvxAsyncCommand<object> ImageClickCommand { get; set; }
    }
}
