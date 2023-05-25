using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
// 리셋하고 시작하면 상점이 리셋이 안된다..

public class BrickMarket :
#if UNITY_ADS
    MonoBehaviour, IUnityAdsListener
#else
    MonoBehaviour

#endif
{      
    [SerializeField]
    Image icon;
    [SerializeField]
    Text name;
    [SerializeField]
    Text price;
    [SerializeField]
    Button adsButton;
    [SerializeField]
    Button buyButton;
    [SerializeField]
    GameObject cannotBuy;
  
    [SerializeField]
    GameObject alert;
    [SerializeField]
    GameController game;
    [SerializeField]
    NumberManager numberManager;
    int buytimes=0;
    MergePreset preset;
    int Price =100;

    const string defaultName = "????";

    bool isWatchAdsClicked;
    bool isGetCoinsClicked;

    public event Action<MergePreset> OnPurchase = delegate { };

   
    
    void Start()
    {   buytimes =PlayerPrefs.GetInt("BuyTimes");
 
        buyButton.onClick.AddListener(OnBuyClick);
        adsButton.onClick.AddListener(OnWatchAdsClick);
  
        
        
        Price = int.Parse(PlayerPrefs.GetString("BrickPrice"));
        
        price.text = numberManager.ToCurrencyString(Price);
        
    }

    public void OnBuyClick()
    {   
        if(UserProgress.Current.Coins >Price){
        alert.SetActive(false);
        UserProgress.Current.Coins -= Price;
        game.BuyBrick();
        buytimes+=1;
        Price = 100*((int)Math.Pow(3, buytimes));
    
        price.text = numberManager.ToCurrencyString(Price);

        PlayerPrefs.SetString("BrickPrice",Price.ToString());
        PlayerPrefs.SetInt("BuyTimes",buytimes);
        }
    }

    void OnWatchAdsClick()
    {
        isWatchAdsClicked = true;
#if UNITY_ADS
        Advertisement.Show(PlacementId.RewardedVideo);
#endif
    }


 

    public void HighlightItem()
    {
        alert.SetActive(true);
    }
#if UNITY_ADS
    public void OnUnityAdsReady(string placementId)
    { }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    { }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != PlacementId.RewardedVideo || showResult != ShowResult.Finished)
            return;

        alert.SetActive(false);
        OnPurchase.Invoke(Preset);
    }
#endif
}