﻿using CloudMusic.Actions;
using CloudMusic.Models;
using CloudMusic.ViewModels;
using CloudMusic.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Prism;
using Prism.Ioc;
using Plugin.DownloadManager;
using CloudMusic.Services;
using CloudMusic.Actions.ApiHelper;
using System.Linq;
namespace CloudMusic
{
    public partial class App : PrismApplication
    {
        public static Context Context;
        public App() : this(null)
        {
           
        }
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            try
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
                {
                    System.Collections.Generic.Dictionary<string, string> dictionary = AliyunOTS.GetAppKey("Syncfusion");
                    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(dictionary["LICENSE"]);
                }
            }
            catch (System.Exception ex) { }
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            Xamarin.Essentials.ExperimentalFeatures.Enable(Xamarin.Essentials.ExperimentalFeatures.ShareFileRequest);
#if DEBUG
            if (!HotReloader.Current.IsRunning)
                HotReloader.Current.Start(this);
#endif
            Context = new Context();
            //MainPage = new NavigationPage(new MasterDetailPage1());
            //MainPage = new MyTabbedPage();
            DependencyService.Get<ICookieStore>().Init(CloudMusicApiHelper.apihost);
            var cookielist = DependencyService.Get<ICookieStore>().CurrentCookies.Where(cc => cc.Name != "none").ToList();
            if (cookielist.Count > 0)
                foreach (var q in cookielist)
                    ApiHelper.HttpClient.CloudMusicCookie.Add(q);
            await NavigationService.NavigateAsync("/NavigationPage/MusicHomePage?selectedTab=MusicDiscoverPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BlurImagePage>();
            containerRegistry.RegisterForNavigation<MusicCommentPage, MusicCommentPageViewModel>();
            containerRegistry.RegisterForNavigation<MusicVideoPage, MusicVideoPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchMusicPage, SearchMusicPageViewModel>();
            containerRegistry.RegisterForNavigation<MusicPlayListPage, MusicPlayListPageViewModel>();
            containerRegistry.RegisterForNavigation<MusicHomePage, MusicHomePageViewModel>();
            containerRegistry.RegisterForNavigation<ScrollVideoPage, ScrollVideoPageViewModel>();
            containerRegistry.RegisterForNavigation<UserInfoPage, UserInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<MusicDiscoverPage, MusicDiscoverPageViewModel>();
            containerRegistry.RegisterForNavigation<FriendEventPage, FriendEventPageViewModel>();
            containerRegistry.RegisterForNavigation<RecommendSongsPage, RecommendSongsPageViewModel>();
            containerRegistry.RegisterForNavigation<EmptyPage, EmptyPageViewModel>();
            containerRegistry.RegisterForNavigation<PersonalFMPage, PersonalFMPageViewModel>();
            containerRegistry.RegisterForNavigation<PhoneMusicMangerPage, PhoneMusicMangerPageViewModel>();
            containerRegistry.RegisterForNavigation<MusicAPPLoginPage, MusicAPPLoginPageViewModel>();
            containerRegistry.RegisterForNavigation<BlankPage, BlankPageViewModel>();
            containerRegistry.RegisterForNavigation<SingerPlayListPage, SingerPlayListPageViewModel>();
        }
        protected override void OnStart()
        {
            //FCMPushNotification.Init();
            System.Threading.Tasks.Task.Run(() => {
                MicrosoftAppCenterHandler.Init();
                //await AutoUpdate.GetUpdate();
                FFImageLoading.Config.Configuration.Default.ClearMemoryCacheOnOutOfMemory = true;
                //FFImageLoading.Config.Configuration.Default.DiskCacheDuration = System.TimeSpan.FromDays(5);
                //FFImageLoading.Config.Configuration.Default.FadeAnimationDuration = 50;
                //FFImageLoading.Config.Configuration.Default.FadeAnimationEnabled = true;
            });
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
