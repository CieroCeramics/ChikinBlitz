using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    private string APP_ID = "ca-app-pub-8752395911071840~3008849712";
    private BannerView bannerAD;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd videoAd;
    // Start is called before the first frame update
    void Start()
    {
      
        RequestBanner();
        RequestInterstitial();
        RequestVideoAD();
        MobileAds.Initialize(APP_ID);

      
    }
    

    void RequestBanner()
    {
        //ca-app-pub-8752395911071840/7878033011
        string banner_ID = "ca-app-pub-3940256099942544/6300978111";
        bannerAD = new BannerView(banner_ID, AdSize.Banner, AdPosition.Bottom);

        //FOR RELEASE

        //AdRequest rAdRequest = new AdRequest.Builder().Build();
        //FOR TESTING
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        //
        bannerAD.LoadAd(adRequest);
    }


    void RequestInterstitial()
    {
        //ca-app-pub-8752395911071840/2927930992
        string Interstitial_ID = "ca-app-pub-3940256099942544/1033173712";
        interstitialAd = new InterstitialAd(Interstitial_ID);

        //FOR RELEASE

        //AdRequest rAdRequest = new AdRequest.Builder().Build();
        //FOR TESTING
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        //
        interstitialAd.LoadAd(adRequest);
    }
    void RequestVideoAD()
    {
        //ca-app-pub-8752395911071840/2927930992
        string video_ID = "ca-app-pub-3940256099942544/5224354917";
        videoAd =  RewardBasedVideoAd.Instance;

        //FOR RELEASE

        //AdRequest rAdRequest = new AdRequest.Builder().Build();
        //FOR TESTING
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        //
        videoAd.LoadAd(adRequest, video_ID);
    }


    public void Display_Banner()
    {
        bannerAD.Show();
    }

    public void Display_Interstitial()
    {
        if(interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
            
        }
    }


    public void Display_Reward_Video()
    {
        if(videoAd.IsLoaded())
        {
            videoAd.Show();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    ///HAndle Events
    ///Interstitial_HandleOnAdLoaded
    public void Interstitial_HandleOnAdLoaded(object sender, EventArgs args)
    {
       
    }

    public void Interstitial_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestInterstitial();
    }

    public void Interstitial_HandleOnAdOpened(object sender, EventArgs args)
    {
        

        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void Interstitial_HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        bannerAD.Destroy();
        SceneManager.LoadScene("Board");

        interstitialAd.Destroy();
    }

    public void Interstitial_HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Display_Banner();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
      
        
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }


    void HandleBannerAdEvents(bool subscribe)
    {
        if (subscribe)
        {
            // Called when an ad request has successfully loaded.
            bannerAD.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerAD.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerAD.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerAD.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerAD.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        }
        else
        {
            // Called when an ad request has successfully loaded.
            bannerAD.OnAdLoaded -= HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerAD.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerAD.OnAdOpening -= HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerAD.OnAdClosed -= HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerAD.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
        }
    }
  public void HandleInterstitialAdEvents(bool subscribe)
    {
        if (subscribe)
        {
            // Called when an ad request has successfully loaded.
            interstitialAd.OnAdLoaded += Interstitial_HandleOnAdLoaded;
            // Called when an ad request failed to load.
            interstitialAd.OnAdFailedToLoad += Interstitial_HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            interstitialAd.OnAdOpening += Interstitial_HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            interstitialAd.OnAdClosed += Interstitial_HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            interstitialAd.OnAdLeavingApplication += Interstitial_HandleOnAdLeavingApplication;
        }
        else
        {
            // Called when an ad request has successfully loaded.
            interstitialAd.OnAdLoaded -= Interstitial_HandleOnAdLoaded;
            // Called when an ad request failed to load.
            interstitialAd.OnAdFailedToLoad -= Interstitial_HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            interstitialAd.OnAdOpening -= Interstitial_HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            interstitialAd.OnAdClosed -= Interstitial_HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            interstitialAd.OnAdLeavingApplication -= Interstitial_HandleOnAdLeavingApplication;
        }
    }
    void OnEnable()
    {
        RequestBanner();
        HandleBannerAdEvents(true);
       
    }
    void OnDisable()
    {
        HandleBannerAdEvents(false);
        HandleInterstitialAdEvents(false);
    }
}
