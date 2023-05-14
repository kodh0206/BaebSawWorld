using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{   
    [SerializeField]List<BaebSae> farms;//수용되어있는 게임 오브젝트
    [SerializeField]Text moneyindicator;
    [SerializeField] double total_money;
    //알까는곳 Ocean에서 까기 
    [SerializeField] private SaveManager saveManager;
    
    public GameObject ocean;
    public GameObject civ;
    public GameObject forest;
    ///여기까지 입력

    private int maxbirds =16; //한번에 수용할수 있는 새 갯수
    private int currentbirds = 0; //현재 새 
    
    public NumberManager numberManager; //숫자표현
    private float nextPayMoney =1;
    private float interval =1;
    private float money_per_seconds =200; // 초당 들어오는 재화 
  
    private double moneyConverter;

    // Start is called before the first frame update
    void Start()
    {
        string current_money =PlayerPrefs.GetString("total_money");
        total_money = Double.Parse(current_money);
        moneyindicator.text = numberManager.ToCurrencyString(total_money);
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

        Debug.Log(maxbirds);
    }


    //알까기 
    

   
   


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

    public int getMaxbirds(){
        return currentbirds;
    }

    public void setMaxbirds(int number){
        currentbirds+=1;
    }
    public double GetTotalMoney(){
        return total_money;
    }

    public void SetTotalMoney(double money){
        total_money += money;
    }
}
