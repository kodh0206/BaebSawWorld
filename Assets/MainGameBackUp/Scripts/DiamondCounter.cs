using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiamondCounter : MonoBehaviour
{
    // Start is called before the first frame updateText label;
    Text label;
    void OnProgressUpdate()
    {
        label.text = CounterText.UpdateText(UserProgress.Current.Diamonds);
    }

    void Start()
    {
        label = GetComponent<Text>();

        OnProgressUpdate();
        UserProgress.Current.ProgressUpdate += OnProgressUpdate;
    }

    void OnDestroy()
    {
        UserProgress.Current.ProgressUpdate -= OnProgressUpdate;
    }
}
