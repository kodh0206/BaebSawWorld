using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{   
    [SerializeField]Text moneyindicator;
    [SerializeField] double total_money; //총액수 
    [SerializeField] int diamond; //유료재화 다이아몬드
    [SerializeField] float money_per_seconds;
    [SerializeField] float normal_money =200;//평상시에 들어오는 돈 
    [SerializeField] float fever_money_per_seconds;
    public Slider feverGauge;
    public float fevertime=30;
//////////////////////////////////////////////재화

    //알까는곳 Ocean에서 까기 
    [SerializeField] private SaveManager saveManager;
    public AudioSource feverTimeost;
    public AudioSource mainTheme;
    //지역
    public GameObject ocean;
    public GameObject civ;
    public GameObject forest;
///////////////////////////////////////////////////////////////
//업적작 
    //깐 달걀수
    public int total_egg; 
    //총 합체 횟수
    public int total_merge;
    
//////////////////////////////////////////////////////
//프레임마다 들어오는 재화    
    public NumberManager numberManager; //숫자표현
    private float nextPayMoney =1;
    private float interval =1;


    private double moneyConverter;

    // Start is called before the first frame update
    void Start()
    {
        string current_money =PlayerPrefs.GetString("total_money");
        total_money = Double.Parse(current_money);
        moneyindicator.text = numberManager.ToCurrencyString(total_money);

        money_per_seconds = normal_money;
        fever_money_per_seconds *= normal_money;
    }

    // Update is called once per frame
    void Update()
    {   
        //매 프레임마다 상태값 저장
        

        //돈늘어나는거 1초마다 
        if(Time.time> nextPayMoney){
        total_money += money_per_seconds;
        moneyindicator.text = numberManager.ToCurrencyString(total_money);
        nextPayMoney = Time.time+ interval;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            exitManager();
        }  
        /*
            //피버타임 On
        if(feverGauge.value==100){
            while(feverGauge.value!=0)
            mainTheme.volume=0;
            feverTimeost.Play();
            feverGauge.value -= 3;
            fevertimeMode(true);
        }
            
        else if(feverGauge.value<0){
                feverGauge.value=0;
                fevertimeMode(false);
                mainTheme.volume=1;
            }
            */
    }

    void fevertimeMode(bool isActive){
        if(isActive)
        {
            money_per_seconds =fever_money_per_seconds;
        }

        else{
            
                money_per_seconds = normal_money;
        }
    }

   
   


   //입력
  
    private void exitManager(){
        if(SceneManager.GetActiveScene().buildIndex==0)//메인메뉴
        {   saveManager.SaveStatus();
            Application.Quit();
        }
        else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
        //그리고 시간저장

    }

   


  
    public void OnOcean(){
        //활성화 안돼있으면
        if(ocean.activeSelf==false)
        {
            ocean.SetActive(true);
            civ.SetActive(false);
            forest.SetActive(false);
        
        }
     

    }

    public void OnForest(){

         if(forest.activeSelf==false)
        {
            ocean.SetActive(false);
            civ.SetActive(false);
            forest.SetActive(true);
        
        }

    }

    public void OnCiv(){
         if(civ.activeSelf==false)
        {
            ocean.SetActive(false);
            civ.SetActive(true);
            forest.SetActive(false);
        
        }

    }
    public double GetTotalMoney(){
        return total_money;
    }
    public void SetTotalMoney(double money){
        total_money += money;
    }
    
}
