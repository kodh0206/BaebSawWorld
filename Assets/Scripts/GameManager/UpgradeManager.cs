using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update

     [SerializeField] GameManagerScript gameManager;
    private int upgradeLV;
    void UpgradeEggDelivery(int upgradeLV)
    {
        this.upgradeLV +=1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
