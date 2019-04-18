using AdvancedTimer.Forms.Plugin.Abstractions;
using Android.Content;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using SocialLadder.Converters;
using SocialLadder.Droid.Converters;
using SocialLadder.Droid.CustomBindings;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {

        }

        protected override IMvxApplication CreateApp()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            return new SocialLadder.App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomPresenter(AndroidViewAssemblies);
        }

        protected override void FillValueConverters(MvvmCross.Platform.Converters.IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("StringDrawableConverter", new StringToDrawableConverter());
            registry.AddOrOverwrite("InverseVisibilityConverter", new InverseVisibilityValueConverter());
            registry.AddOrOverwrite("DirectVisibilityConverter", new DirectVisibilityValueConverter());
            registry.AddOrOverwrite("DirectHiddenConverter", new DirectHiddenValueConverter());
            registry.AddOrOverwrite("DateTimeToTimeAgoStringConverter", new DateTimeToTimeAgoStringConverter());
            registry.AddOrOverwrite("ListEngagementToCommentsConverter", new ListEngagementToCommentsConverter());
            registry.AddOrOverwrite("ActionsToFooterFeedCellVisibilityConverter", new ActionsToFooterFeedCellVisibilityConverter());
            registry.AddOrOverwrite("CommentsToVisibilityConverter", new CommentsToVisibilityConverter());
            registry.AddOrOverwrite("ColorConverter", new ColorValueConverter());
            registry.AddOrOverwrite("FeedBackgroundVisibilityConverter", new FeedBackgroundVisibilityConverter());
            registry.AddOrOverwrite("LikeToIconConverter", new LikeToIconConverter());
            registry.AddOrOverwrite("PointsToVisibility", new PointsToVisibilityConverter());
            registry.AddOrOverwrite("EngagementListToCommentCountConverter", new EngagementListToCommentCountConverter());
            registry.AddOrOverwrite("ActionDictionaryToReportVisibilityConverter", new ActionDictionaryToReportVisibilityConverter());
            registry.AddOrOverwrite("ColorHexToDrawableConverter", new ColorHexToDrawableValueConverter());
            registry.AddOrOverwrite("RewardsMinPriceToScoreConverter", new RewardsMinPriceToScoreConverter());
            registry.AddOrOverwrite("RewardScoreToStatusImageConverter", new RewardScoreToStatusImageConverter());
            registry.AddOrOverwrite("RewardScoreToVisibilityConverter", new RewardScoreToVisibilityConverter());
            registry.AddOrOverwrite("ChallengeCollectionImageConverter", new ChallengeCollectionImageConverter());
            registry.AddOrOverwrite("BytesToBitmapConverter", new BytesToBitmapValueConverter()); 
            registry.AddOrOverwrite("InverseBoolVisibilityConverter", new InverseBoolVisibilityConverter());
            registry.AddOrOverwrite("ChallengeTypeToActorBackgroundConverter", new ChallengeTypeToActorBackgroundConverter());
            registry.AddOrOverwrite("ActionsToCommentsVisibilityConverter", new ActionsToCommentsVisibilityConverter());
            registry.AddOrOverwrite("ActionsToLikeVisibilityConverter", new ActionsToLikeVisibilityConverter());
            registry.AddOrOverwrite("DescriptionTextItemVisibilityConverter", new DescriptionTextItemVisibilityConverter());
            registry.AddOrOverwrite("DescriptionButtonItemVisibilityConverter", new DescriptionButtonItemVisibilityConverter());
            registry.AddOrOverwrite("EnabledValueConverter", new EnabledValueConverter());
            registry.AddOrOverwrite("StringToColorConverter", new StringToColorConverter());
            registry.AddOrOverwrite("DateTimeToNotificationCreationTimeConverter", new DateTimeToNotificationCreationTimeConverter());
            registry.AddOrOverwrite("NetworkStringToColorConverter", new NetworkStringToColorConverter());
            registry.AddOrOverwrite("MapItemVisibilityValueConverter", new MapItemVisibilityValueConverter());
            registry.AddOrOverwrite("PhotoItemVisibilityValueConverter", new PhotoItemVisibilityValueConverter());
            registry.AddOrOverwrite("LabelItemVisibilityValueConverter", new LabelItemVisibilityValueConverter());
            registry.AddOrOverwrite("TextWithQuoteConverter", new TextWithQuoteConverter());           
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.RegisterType<IAdvancedTimer, AdvancedTimer.Forms.Plugin.Droid.AdvancedTimerImplementation>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            WebViewDataBinding.Register(registry);
            ImageViewParamClickBinding.Register(registry);
            MvxImageViewItemIdBinding.Register(registry);          
            //registry.RegisterPropertyInfoBindingFactory( typeof(BindableWebView), typeof(CustomWebViewBinding), "BindableWebView");
        }
    }
}