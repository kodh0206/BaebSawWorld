﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class StorePanel : MonoBehaviour
{
    [SerializeField]
    GameObject Storepanel;
    [SerializeField]
    GameObject Upgradepanel;
    [SerializeField]
    StoreItem storeItemPreviewPrefab;
    [SerializeField]
    RectTransform previewParent;

    [SerializeField]
    GameObject alertIcon;
    [SerializeField]
    Button rewardButton;
    [SerializeField]
    Text rewardTimer;

    readonly List<StoreItem> previews = new List<StoreItem>();
    double rewardTimerSeconds = 7200;
    DateTime timeToReward;
    bool isRewardWaiting;
    public Button StoreButton;
    public Button UpgradeButton;

    void Awake()
    {   StoreButton.onClick.AddListener(ToStore);
        UpgradeButton.onClick.AddListener(ToUpgarade);
        MergeController.UnlockedNewLevel += OnNewLevelUnlocked;
        MergeController.RewardUsed += OnRewardUsed;
        rewardButton.onClick.AddListener(OnRewardClick);

        IEnumerable<MergePreset> objects = MergeController.Instance.GetPresets();
        for (int i = 1; i < objects.Count(); i++)
        {
            StoreItem preview = Instantiate(storeItemPreviewPrefab, previewParent);
            preview.Preset = objects.ElementAt(i);
            preview.OnPurchase += OnPurchaseComplete;
            previews.Add(preview);
        }
    }

    void OnEnable()
    {
#if UNITY_ADS
        Advertisement.Show(PlacementId.Video);
#endif
        UpdateButtons();
    }

    void Update()
    {   
        if (isRewardWaiting)
            UpdateRewardTimer();
        
        if(Storepanel.activeSelf)
            UpdateButtons();
    }
    public void  ToStore(){
        Storepanel.SetActive(true);
        Upgradepanel.SetActive(false);
    }

    public void ToUpgarade(){
        Storepanel.SetActive(false);
        Upgradepanel.SetActive(true);
    }
    public void ShowStore(bool active)
    {
        UpdateButtons();
        Storepanel.SetActive(active);
        StoreButton.gameObject.SetActive(active);
        UpgradeButton.gameObject.SetActive(active);

        
    }
    public void ExitUpgrade()
    {
        UpdateButtons();
        Storepanel.SetActive(false);
        StoreButton.gameObject.SetActive(false);
        UpgradeButton.gameObject.SetActive(false);
        Upgradepanel.gameObject.SetActive(false);
        
 
    }

    void OnRewardClick()
    {
        MergeController.Instance.OnRewardUsed();
    }

    void OnPurchaseComplete(MergePreset preset)
    {
        MergeController.Instance.Current = preset;
        MergeController.Instance.OnPurchaseCompleted(MergeController.Instance.Current);

        UpdateButtons();
    }
    void OnNewLevelUnlocked(bool value, int level)
    {
        if (alertIcon.activeSelf != value)
            alertIcon.SetActive(value);

        if (value && level > 1)
            previews[level - 2].HighlightItem();
    }

    void OnRewardUsed(int value, BrickType type)
    {
        timeToReward = DateTime.UtcNow + new TimeSpan(2, 0, 0);

        MergeController.Instance.UpdateRewardTimer(timeToReward);
        rewardTimer.gameObject.SetActive(true);
        rewardButton.gameObject.SetActive(false);
        isRewardWaiting = true;
    }

    void UpdateRewardTimer()
    {
        TimeSpan t = timeToReward - DateTime.UtcNow;
        if (t.TotalSeconds <= 0)
        {
            rewardTimer.gameObject.SetActive(false);
            rewardButton.gameObject.SetActive(true);
            isRewardWaiting = false;
            return;
        }

        string countDown = t.ToString(@"hh\:mm\:ss");
        rewardTimer.text = countDown;
    }

    void UpdateButtons()
    {
        IEnumerable<MergePreset> presets = MergeController.Instance.GetPresets();

        int maxOpenItem = MergeController.Instance.MaxOpenLevel;
        for (int i = 1; i < presets.Count(); i++)
            previews[i - 1].Preset = i <= maxOpenItem - 1 ? presets.ElementAt(i) : null;

        DateTime time = MergeController.Instance.RewardTimer;

        if (time > timeToReward)
            timeToReward = time;

        if (timeToReward > DateTime.UtcNow)
        {
            rewardTimer.gameObject.SetActive(true);
            rewardButton.gameObject.SetActive(false);
            isRewardWaiting = true;
        }
        else
        {
            rewardTimer.gameObject.SetActive(false);
            rewardButton.gameObject.SetActive(true);
        }

  

    }

}