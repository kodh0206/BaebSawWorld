using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewItemPreview : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Image icon;
    [SerializeField]
    Text title;
    [SerializeField]
    AudioSource emerge;

    [SerializeField]
    Button exitButton;

    Image raycastImage;
    
    void Start()
    {   exitButton.onClick.AddListener(CloseWindow);
        MergeController.UnlockedNewLevel += UpdatePreview;
        raycastImage = GetComponent<Image>();
    }


    void CloseWindow()
    {
        panel.SetActive(false);
    }
    void UpdatePreview(bool value, int level)
    {
        if (!value) return;
        
        MergePreset preset = MergeController.Instance.GetPresset(level);
        icon.sprite = preset.Icon;
        title.text = preset.Title;

        raycastImage.raycastTarget = true;
        panel.SetActive(true);
         emerge.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        raycastImage.raycastTarget = false;
        panel.SetActive(false);
    }
}