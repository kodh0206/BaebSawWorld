using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
각 월드별 작동원리
패널이 켜지면 리스트에 있는 

*/


public class Civ : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Transform> Cplaces;
    public string[] Cbirds = new string[16]; //생성된 뱁새
    
    private int currentbirds;
    private int maxbirds =16;




    
}
