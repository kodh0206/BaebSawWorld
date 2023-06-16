using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeManager : MonoBehaviour
{
[SerializeField]
GameController gameController;
[Header("Chef")]
public Text chefPriceText;
public Text chefLevelText;
public Sprite chefDiamond;
public Image CMoney;
public Button chef;
[Header("Kiosk")]
public Text kioskPriceText;
public Text kioskLevelText;
public Sprite kDiamond;
public Image kMoney;
public Button kiosk;
[Header("SushiQuality")]
public Text sushiqualityPriceText;
public Text sushiLevelText;
public Sprite sDiamond;
public Image sMoney;
public Button sushi;

void Awake()
{
  Load();
  SettingChef();
  SettingKiosk();
  SettingSushi();
  chef.onClick.AddListener(UpgradeChef);
  kiosk.onClick.AddListener(UpgradeKiosk);
  sushi.onClick.AddListener(UpgardeSushi);
  Debug.Log("K 업그레이드 구매횟수"+kioskbuytimes);
  Debug.Log("C 업그레이드 구매횟수"+chefbuytimes);
  Debug.Log("S 업그레이드 구매횟수"+sushibuytimes);
}
  
  int cheflevel;
  int []chefprice= {200,3000,10000,100000,1000000,10000000};
  public int []chefdiamonds ={10,20};
  public int chefbuytimes =0;

public  int kiosklevel;
public int []kioskprice= {500,5000,25000,250000,2500000,250000000};
public int []kioskdiamonds ={10,20,30};

public int kioskbuytimes =0;


  public int sushiprice=5000000;
  public int []sushidiamonds={10,20,30};
  public int sushibuytimes =0;

//데이터 변수이름
string K = "kioskBuytimes";
string C = "chefBuytimes";
string S ="sushiBuytimes";

//셰프 업그레이드
void UpgradeChef(){
    if(UserProgress.Current.Coins > chefprice[chefbuytimes]&&chefbuytimes <6){
      UserProgress.Current.Coins -= chefprice[chefbuytimes];
      gameController.spawnTime -= 1;
      chefbuytimes+=1;
      chefPriceText.text = NumberManager.ToCurrencyString(chefprice[chefbuytimes]);
      chefLevelText.text =string.Format("{0}/8",chefbuytimes);
      // 여기에 text 변환
    }

    //이미지 변경 
    else if(chefbuytimes ==6)
    { 
      UserProgress.Current.Diamonds -= chefdiamonds[chefbuytimes-6];
      gameController.spawnTime -= 1;
      chefbuytimes+=1;
      chefPriceText.text = chefdiamonds[chefbuytimes-6].ToString();
      CMoney.sprite = chefDiamond;

    }


    if(chefbuytimes>6)
    {
      if(UserProgress.Current.Diamonds >chefdiamonds[chefbuytimes-6])
      {
      UserProgress.Current.Diamonds -= chefdiamonds[chefbuytimes-6];
      gameController.spawnTime -= 1;
      chefbuytimes+=1;
      chefPriceText.text = chefdiamonds[chefbuytimes-6].ToString();
    //여기에 text 변환
      }
      else{
        Debug.Log("No CUrrency");
      }
    }


    PlayerPrefs.SetFloat("SpawnTime",gameController.spawnTime);
    Save();
    SettingChef();
  }

void SettingChef(){
  //코인 구매상태
  if(chefbuytimes<6)
  {

  chefPriceText.text =string.Format("{0}",NumberManager.ToCurrencyString(chefprice[chefbuytimes]));
  chefLevelText.text=string.Format("{0}/8",chefbuytimes);
  }
  // 다이아몬드 구매상태
  else
  {
  chefPriceText.text =string.Format("{0}",chefprice[0]);
  chefLevelText.text = "0/8";
  CMoney.sprite = chefDiamond;
  }
}


//셰프 업그레이드 
void UpgradeKiosk(){

if(kioskbuytimes <6){
 if(UserProgress.Current.Coins > kioskprice[kioskbuytimes]){
      UserProgress.Current.Coins -= kioskprice[kioskbuytimes];
      //gameController.spawnTime -= 1;
      kioskbuytimes+=1;
      kioskPriceText.text = NumberManager.ToCurrencyString(kioskprice[kioskbuytimes]);
      kioskLevelText.text =string.Format("{0}/8",kioskbuytimes);
      // 여기에 text 변환
    }

}

    //이미지 변경 
    else if(kioskbuytimes ==6)
    { UserProgress.Current.Diamonds -= kioskdiamonds[kioskbuytimes-6];
      //gameController.spawnTime -= 1;
      kioskbuytimes+=1;
      kioskPriceText.text = kioskdiamonds[kioskbuytimes-6].ToString();
      kMoney.sprite = chefDiamond;
      Save();
    }
    
  else if(kioskbuytimes>=6)
  {
    if(UserProgress.Current.Diamonds > kioskdiamonds[kioskbuytimes-6]&&kioskbuytimes >5 )
    {
      UserProgress.Current.Diamonds -= kioskdiamonds[kioskbuytimes-6];
      //gameController.spawnTime -= 1;
      kioskbuytimes+=1;
      kioskPriceText.text = kioskdiamonds[kioskbuytimes-6].ToString();
    //여기에 text 변환
    }
    PlayerPrefs.SetFloat("SpawnTime",gameController.spawnTime);
    SettingKiosk();
    Save();
    }
  }
  void SettingKiosk(){
  if(kioskbuytimes<6)
  {

  kioskPriceText.text =string.Format("{0}",NumberManager.ToCurrencyString(kioskprice[kioskbuytimes]));
  kioskLevelText.text=string.Format("{0}/9",kioskbuytimes);
  }
  // 다이아몬드 구매상태
  else 
  {
  kioskPriceText.text =string.Format("{0}",kioskdiamonds[kioskbuytimes-6]);
  kioskLevelText.text=string.Format("{0}/9",kioskbuytimes);
  kMoney.sprite = kDiamond;
  }
}
    //처음나오는 초밥
  void UpgardeSushi(){

    if(sushibuytimes ==0){
     if(UserProgress.Current.Coins > sushiprice)
     {
      UserProgress.Current.Coins -= sushiprice;
      gameController.spawnlevel += 1;
      sushibuytimes+=1;
      
      kMoney.sprite = kDiamond;
    }
    }

    if(sushibuytimes >0)
    {
    //재화 스프라이트 변경 
      if(UserProgress.Current.Diamonds >sushidiamonds[sushibuytimes-1]  )
      {
        UserProgress.Current.Diamonds -= sushidiamonds[sushibuytimes-1];
        gameController.spawnlevel += 1;
        sushibuytimes+=1;

      }
    }
    else{
      Debug.Log("No Currency");
    }
    Save();
    PlayerPrefs.SetInt("SpawnLevel",gameController.spawnlevel);
    
    Debug.Log("스시 업그레이드 완료");

    SettingSushi();
  }

  void SettingSushi(){
  if(sushibuytimes<1)
    {

    sushiqualityPriceText.text =NumberManager.ToCurrencyString(sushiprice);
    sushiLevelText.text=string.Format("{0}/4",sushibuytimes);
    }
  // 다이아몬드 구매상태
    else
    {
      kioskPriceText.text =string.Format("{0}",sushidiamonds[sushibuytimes-1]);
      kioskLevelText.text = "0/4";
      sMoney.sprite = kDiamond;
    }
  }

  void Save()
  {
    PlayerPrefs.SetInt(C,chefbuytimes);
    PlayerPrefs.SetInt(K,kioskbuytimes);
    PlayerPrefs.SetInt(S,sushibuytimes);
  }

  void Load(){
    if(PlayerPrefs.HasKey(C)){
    chefbuytimes = PlayerPrefs.GetInt(C);
    kioskbuytimes = PlayerPrefs.GetInt(K);
    sushibuytimes = PlayerPrefs.GetInt(S);
    }
    else{
      chefbuytimes= 0;
      kioskbuytimes= 0;
      sushibuytimes = 0;
    }
  }
}
