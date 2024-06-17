using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private BannerAds _bannerAds;
    private FrontAds _frontAds;
    
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple AdManager is running");
        Instance = this;
        
        MobileAds.Initialize(initStatus => { });

        _bannerAds = GetComponent<BannerAds>();
        _frontAds = GetComponent<FrontAds>();
    }

    public void ShowBannerAds(Scene scene = new Scene(), LoadSceneMode mode = 0)
    {
        _bannerAds.LoadAd();
    }
    
    public void ShowFrontAds()
    {
        //_frontAds.LoadInterstitialAd();
        _frontAds.ShowInterstitialAd();
    }
}
