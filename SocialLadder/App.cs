using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using SocialLadder.Interfaces;
using SocialLadder.Services;
using SocialLadder.ViewModels.Feed;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.More;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder
{
    public class App : MvxApplication
    {

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart();
        }

        public void RegisterAppStart()
        {
            if (!SL.HasProfile)
            {
                RegisterNavigationServiceAppStart<IntroContainerViewModel>();
                return;
            }
            //if (SL.HasAreas && string.IsNullOrEmpty(SL.AreaGUID))
            //{
            //    RegisterNavigationServiceAppStart<AreasCollectionViewModel>();
            //    return;
            //}
            RegisterNavigationServiceAppStart<MainViewModel>();
        }

        public void RegisterContainer()
        {
            //Mvx.RegisterType<IFacebookService, FacebookService>();
        }



    }




}
