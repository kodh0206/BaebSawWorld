using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> Fplaces; //여기에 뱁새생성예정
    public List<string> FBirds;//숲에 있는 뱁세 이름들
    [SerializeField]private GameObject layingplaces;
    private int currentbirds =0;

    //수가 바뀔때마다
    public void updateBirds(string baebSae){
        FBirds.Add(baebSae);
        currentbirds =FBirds.Count;
    }

    public void deleteBirds(string baebSae){
        FBirds.Remove(baebSae);
        currentbirds =FBirds.Count;

    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>

    public int getCurrentBirds(){
        return currentbirds;
    }
 
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        Debug.Log(currentbirds);   
    }

    
}
