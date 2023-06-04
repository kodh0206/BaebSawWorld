using System;
using UnityEngine;

[Serializable]
public class MergePreset
{
    public Sprite Icon;
    public string Title = "Title";
    public string Description="Description";
    public long RawPrice;
    readonly float priceGrothFactor = 1.2f;

    
    long price;
    public long Price
    {
        get => price == 0 ? RawPrice : price;
        set => price = value;
    }
    //가격업데이트
    public void UpdatePrice()
    {
        Price = price == 0 ? RawPrice : Mathf.RoundToInt(Price * priceGrothFactor);
    }
}