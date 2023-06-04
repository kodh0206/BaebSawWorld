using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{   
    [SerializeField]
    GameController gameController;
    [SerializeField]
    GameObject Opening;
    [SerializeField]
    GameObject Merge;
    [SerializeField]
    GameObject Touch;
    [SerializeField]
    GameObject Buying;
    [SerializeField]
    GameObject Upgrade;
    [SerializeField]
    GameObject WatchAds;

    public void closeOpening()

    {   Opening.SetActive(false);  
        Merge.SetActive(true);
            Touch.SetActive(true);

    }
        void Start()
    {   
        if(MergeController.Instance.MaxOpenLevel==0){
        
        Opening.SetActive(true);
 
        }
        MergeController.UnlockedNewLevel += UpdatePreview;
      
    }


  
    void UpdatePreview(bool value, int level)
    {   
          
        
        if(level ==1)
        {
            Buying.SetActive(true);
        }
        else if(level ==2)
        {
            Upgrade.SetActive(true);
        }
        else if(level==3)
        {
            WatchAds.SetActive(true);
        }

        }
    }







