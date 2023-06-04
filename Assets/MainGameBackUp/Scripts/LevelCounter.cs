using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelCounter : MonoBehaviour
{
    // Start is called before the first frame update
    Text label;

    void OnProgressUpdate()
    {
        label.text = string.Format("LV{0}",UserProgress.Current.UserLevels);
    }

    void Start()
    {
        label = GetComponent<Text>();
        Debug.Log("현재레밸"+UserProgress.Current.UserLevels);
        OnProgressUpdate();
        UserProgress.Current.ProgressUpdate += OnProgressUpdate;
    }

    void OnDestroy()
    {
        UserProgress.Current.ProgressUpdate -= OnProgressUpdate;
    } 
    
}
