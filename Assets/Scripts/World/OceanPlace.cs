using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanPlace : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Ocean ocean;
    private int savedCount;
    private List<string> savedArray;

    /*
    void Makebirds(){
       if(savedCount != forest.getCurrentBirds())
        {
         for(int i=0; i<forest.FBirds.Count;i++){
        GameObject birds = Instantiate(Resources.Load(forest.Fplaces[i].getName()) ,
        forest.Fplaces[i].getPlace(), 
        transform.rotation) as GameObject;
        birds.transform.SetParent(forest.Fplaces[i].transform,false);//계란은 항상 처음 
        
         }
  
        }
    }
    private void OnEnable()
    {  
      Invoke("Makebirds", 1);
        }
    
    // This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
      savedCount = forest.getCurrentBirds();
      
    }
    */
}
