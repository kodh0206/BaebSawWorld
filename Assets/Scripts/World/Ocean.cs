using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{ 
    [SerializeField] GameManagerScript gameManager;
    public List<Tiles> Oplaces; //여기에 계란과 뱁새생성예정
     public List<string> OBirds;//바다에 담겨져있는 뱁세 이름
    private int setup;
    public List<int> EmptySet=new List<int>{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};
    private int currentbirds=0;
    private int maxbirds=16;
   

    private void Update()
    {

    }
    public void lay_egg()
    {    
        if(gameManager.GetTotalMoney()>=1000  && EmptySet.Count != 0){
         GameObject newEgg =Instantiate(Resources.Load("Egg"),Oplaces[EmptySet[0]].getPlace().position, transform.rotation) as GameObject;
         newEgg.transform.SetParent(Oplaces[EmptySet[0]].transform,false);//계란은 항상 처음 
         gameManager.SetTotalMoney(-1000);
         Oplaces[EmptySet[0]].setBaebsae("Egg");
         newEgg.GetComponent<BaebSae>().setIndex(EmptySet[0]);
         EmptySet.RemoveAt(0);
         gameManager.feverGauge.value+=1;
         gameManager.total_egg+=1;

        
        }
        else{
            Debug.Log("No Money");
            Debug.Log("No Space");
        }
    }

    public int GetNumber(){
        int numbers=0;
        for(int i=0; i<16;i++){
            if(Oplaces[i].getName() != "Empty"){
                numbers+=1;

            }
        }
        return numbers;
    }

   
    
    

    public int getCurrentBirds(){
        return Oplaces.Count;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        setup+=1;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
  
}
