using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    // Start is called before the first frame update

    public List<WaitingBirds> waitingbirds;//여기에 진화 처음된 뱁새 존재
    public List<Tiles> Fplaces; //여기에 뱁새생성예정
    [SerializeField] private GameObject layingplaces;
    [SerializeField] GameManagerScript gameManager;
    public List<int> EmptySet=new List<int>{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};
    
 
    
}
