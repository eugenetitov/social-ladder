using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IItemActionService
    {
        MvxAsyncCommand<object> ItemActionCommand { get; }
    }
}
