using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestPlace : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    [SerializeField] Forest forest;
    
    private int savedCount;
    private List<string> savedArray;
    void Makebirds(){

         for(int i=0; i<forest.waitingbirds.Count;i++){
      
        GameObject newBirds =Instantiate(Resources.Load(forest.waitingbirds[i].baebSae)
        ,forest.Fplaces[forest.waitingbirds[i].index].getPlace().position, transform.rotation) as GameObject;

         newBirds.transform.SetParent(forest.Fplaces[forest.waitingbirds[i].index].getPlace().transform,false);
         forest.Fplaces[forest.waitingbirds[i].index].setBaebsae(forest.waitingbirds[i].baebSae);
         newBirds.GetComponent<BaebSae>().setIndex(forest.waitingbirds[i].index);
     
         }
  
        
    }
    private void OnEnable()
    {  
      if(forest.waitingbirds.Count!=0){
        Makebirds();

      }
      else{
        Debug.Log("nobirds");
      }
     
        }
    
    // This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
      
      forest.waitingbirds.Clear();//싹다 지우기
    }
    
}

