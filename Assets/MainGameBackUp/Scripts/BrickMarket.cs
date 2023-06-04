using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

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
    //[SerializeField]
    //Button adsButton;
    [SerializeField]
    Button buyButton;
    [SerializeField]
    GameObject cannotBuy;
  
    [SerializeField]
    GameObject alert;
    [SerializeField]
    GameController game;
    [SerializeField]
    
    int buytimes=0;
    MergePreset preset;
    Int64 Price =100;
    bool isbuy=false;
    bool isWatchAdsClicked;
    bool isGetCoinsClicked;

    public event Action<MergePreset> OnPurchase = delegate { };

   
    void Update(){
        Debug.Log("Buytimes"+buytimes);
    }
    void Awake()
    {   
       
        Load();
        
        price.text = NumberManager.ToCurrencyString(Price);
        


        buyButton.onClick.AddListener(OnBuyClick);
        //adsButton.onClick.AddListener(OnWatchAdsClick);
        
    }

    public void OnBuyClick()
    {   
        if(UserProgress.Current.Coins >Price&& !isbuy){
        alert.SetActive(false);
        UserProgress.Current.Coins -= Price;
        isbuy=true;
        game.BuyBrick();
        buytimes+=1;
        Price = 100*((int)Math.Pow(3, buytimes));
        
        price.text = NumberManager.ToCurrencyString(Price);

        Save();
        Nobuy();
        Invoke("Yesbuy",1f);
        //UserProgress.Current.Diamonds +=1;
        }
    }

    void OnWatchAdsClick()
    {
        isWatchAdsClicked = true;
#if UNITY_ADS
        Advertisement.Show(PlacementId.RewardedVideo);
#endif
    }
    void Save()
    {   PlayerPrefs.SetString("BrickPrice",Price.ToString());
        PlayerPrefs.SetInt("BuyTimes",buytimes);

    }
    void Load()
    {
        buytimes =PlayerPrefs.GetInt("BuyTimes");
        Price = Int64.Parse(PlayerPrefs.GetString("BrickPrice"));
    }
    void Nobuy(){
        cannotBuy.SetActive(true);
        
    }
    void Yesbuy(){
        cannotBuy.SetActive(false);
        isbuy=false;
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