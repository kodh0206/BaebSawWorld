using System;
using UnityEngine;
[Serializable]
public class MergePreset
{
    public Sprite Icon;
    public string Title = "Title";
    public string Description="Description";
    public double RawPrice;
    readonly float priceGrothFactor = 1.2f;
    //초당코인 
    public long CPS;
 
    
    double price;
    public double Price
    {
        get => price == 0 ? RawPrice : price;
        set => price = value;
    }
    //가격업데이트
    public void UpdatePrice()
    {
        Price = price == 0 ? RawPrice : Math.Round(Price * 1.15 / 10) * 10;
    }
}