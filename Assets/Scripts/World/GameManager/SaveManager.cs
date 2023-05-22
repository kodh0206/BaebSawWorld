using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
      //게임 상태 저장

    public Ocean ocean;
    public Forest forest;
    public Civ civ;
    [SerializeField] GameManagerScript gameManager; 
    public void SaveStatus(){
        PlayerPrefs.SetString("total_money",Encryption.Encrypts( gameManager.GetTotalMoney().ToString()));
    }


    ////// 저장하기
    void SaveBirds(string [] birds){
        int[] number = new int[100]; // 정수형 배열 생성

        number[2] = 1;

        string strArr = ""; // 문자열 생성

        for (int i = 0; i < number.Length; i++) // 배열과 ','를 번갈아가며 tempStr에 저장
        {
            strArr = strArr + number[i];
            if (i < number.Length - 1) // 최대 길이의 -1까지만 ,를 저장
            {
                strArr = strArr + ",";
            }
        }

        print(strArr); // 0,0,1,0,0....으로 저장된 strArr

        PlayerPrefs.SetString("Data", strArr); // PlyerPrefs에 문자열 형태로 저
    }


   
}
