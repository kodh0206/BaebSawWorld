using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;

//유저들 데이터 조작해서 하는 해킹을 막기위해 불가피하게 짜놓으 암호화 코드
// 암호화 및 해독 코드 다있음

public class Encryption : MonoBehaviour
{
    // Start is called before the first frame update
    static string hash ="123456@abc";
   public static string Encrypts(string input){

    byte[] data =UTF8Encoding.UTF8.GetBytes(input);
    using(MD5CryptoServiceProvider md5 =new MD5CryptoServiceProvider())
    {
        byte[]key =md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
        using(TripleDESCryptoServiceProvider trip =new TripleDESCryptoServiceProvider(){Key=key, Mode=CipherMode.ECB, Padding =PaddingMode.PKCS7})
        {
            ICryptoTransform tr = trip.CreateEncryptor();
            byte[] result = tr.TransformFinalBlock(data,0,data.Length);
            return Convert.ToBase64String(result, 0, result.Length);

        }
    }
   }

   public static string Decrypt(string input){


    byte[]data =Convert.FromBase64String(input);
    using(MD5CryptoServiceProvider md5 =new MD5CryptoServiceProvider())
    {
        byte[]key=md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
        using(TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider(){Key=key,Mode=CipherMode.ECB,Padding= PaddingMode.PKCS7})
        {
            ICryptoTransform tr =trip.CreateDecryptor();
            byte[] result =tr.TransformFinalBlock(data,0,data.Length);
            return UTF8Encoding.UTF8.GetString(result);
        }
    }

   }
}
