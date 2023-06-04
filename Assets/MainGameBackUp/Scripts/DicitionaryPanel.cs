using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DicitionaryPanel : MonoBehaviour
{
       [SerializeField]
    GameObject DictionaryPanel;

    [SerializeField]
    DictionaryItems dictionarypreviewPrefab;
    [SerializeField]
    RectTransform previewParent;

    [SerializeField]
    GameObject alertIcon;
    [SerializeField]
    Button dicitonarybutton;
    [SerializeField]
    Button closeButton;
    readonly List<DictionaryItems> previews = new List<DictionaryItems>();


    bool isRewardWaiting;



    void Awake()
    {   
        MergeController.UnlockedNewLevel += OnNewLevelUnlocked;
       
       
        dicitonarybutton.onClick.AddListener(OnButtion);
        closeButton.onClick.AddListener(Onclose);
        MergeController.UnlockedNewLevel += OnNewLevelUnlocked;
        IEnumerable<MergePreset> objects = MergeController.Instance.GetPresets();
        for (int i = 0; i < objects.Count(); i++)
        {
            DictionaryItems preview = Instantiate(dictionarypreviewPrefab, previewParent);
            preview.Preset = objects.ElementAt(i);
            previews.Add(preview);
        }
        
    }

    void OnEnable()
    {

        UpdateDictionary();
    }

    void Update()
    {   
        
        
        if(DictionaryPanel.activeSelf)
            UpdateDictionary();
    }



    void OnButtion()
    {   
        DictionaryPanel.SetActive(true);
    }

    void Onclose(){
         DictionaryPanel.SetActive(false);
    }
    void OnNewLevelUnlocked(bool value, int level)
    {
        if (alertIcon.activeSelf != value)
            alertIcon.SetActive(value);

        if (value && level > 1)
            previews[level].HighlightItem();
    }


    void UpdateDictionary()
    {
        IEnumerable<MergePreset> presets = MergeController.Instance.GetPresets();

        int maxOpenItem = MergeController.Instance.MaxOpenLevel+1;
        for (int i = 0; i < presets.Count(); i++)
            previews[i].Preset = i <= maxOpenItem - 1 ? presets.ElementAt(i) : null;

        

  

    }

  

}

