using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform place;
    [SerializeField] string baebSae ="Empty";
    public int index;

    public float cashflow;//타일에서 생성하는 재화 
    public Transform getPlace(){
        return place;
    }

    public void setBaebsae(string baebSae){
        this.baebSae = baebSae;
    }

    public void deleteBaebsae(){
        baebSae = "";
    }

    public int getIndex(){
        return index;
    }

    public string getName(){
        return baebSae;
    }

    public void setIndex(int index){
        this.index =index;
    }

    public void setCashFlow(float cashflow){
        this.cashflow =cashflow;
    }
}
