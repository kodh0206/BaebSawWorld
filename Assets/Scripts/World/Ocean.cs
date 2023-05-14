using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{ 
    [SerializeField] GameManagerScript gameManager;
    public List<Transform> Oplaces; //여기에 계란과 뱁새생성예정
     public List<string> OBirds;//바다에 담겨져있는 뱁세 이름
    private int currentbirds=0;
    private int maxbirds=16;
    public void lay_egg()
    {    
        if(gameManager.GetTotalMoney()>=1000  && currentbirds<maxbirds){
         GameObject newEgg =Instantiate(Resources.Load("Egg"),Oplaces[currentbirds].position, transform.rotation) as GameObject;
         newEgg.transform.SetParent(Oplaces[currentbirds].transform,false);//계란은 항상 처음 
         gameManager.SetTotalMoney(-1000);
         currentbirds+=1;
        updateBirds("Egg");
        }
        else{
            Debug.Log("No Money");
            Debug.Log("No Space");
        }
    }
    public void updateBirds(string baebSae){
        OBirds.Add(baebSae);
        currentbirds =OBirds.Count;
        Debug.Log("바다 총 길이:"+currentbirds);
    }

    public void deleteBirds(string baebSae){
        OBirds.Remove(baebSae);
        currentbirds =OBirds.Count;
        Debug.Log("바다 총 길이:"+currentbirds);

    }

    public void hatchEgg(){
        OBirds.Add("CellLv1");
        OBirds.Remove("Egg");
        currentbirds =OBirds.Count;
        Debug.Log("바다 총 길이:"+currentbirds);
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    //실행됐을때


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>

    public int getCurrentBirds(){
        return currentbirds;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
  
}
