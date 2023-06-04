using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DictionaryItems : MonoBehaviour
{
    // Start is called before the first frame update
     [SerializeField]
    Image icon;
    [SerializeField]
    Text name;
    [SerializeField]
    Text Description;

    [SerializeField]
    GameObject alert;

    [SerializeField]
    Sprite unKnownIcon;

    MergePreset preset;
   
    const string defaultName = "????";
    const string defalutScription ="????";

   
    public MergePreset Preset
    {
        get => preset;
        set
        {
            preset = value;
            UpdatePreview();
        }
    }



    void Start()
    {
        MergeController.Purchased += OnPreviewUpdate;
    }
   


    void OnPreviewUpdate(int level, BrickType type)
    {
        UpdatePreview();
    }

    void UpdatePreview()
    {
        if (preset == null)
        {
            icon.sprite = unKnownIcon;
            name.text = defaultName;
            Description.text= defalutScription;
            SetItemUnlock(false);
            return;
        }

        icon.sprite = preset.Icon;
        name.text = preset.Title;
        Description.text = preset.Description;
        
        SetItemUnlock(true);
        

    }

    void SetItemUnlock(bool unlock)
    {
       
    
        
    }

     public void HighlightItem()
    {
        alert.SetActive(true);
    }


  

   
}
