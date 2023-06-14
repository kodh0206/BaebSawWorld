using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class NumberManager 

{


    //단위 자리수 재화 밸런스 고려X
  static readonly string[] currencyUnits = new string[] { "","K","M","B","T", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
   "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", 
   "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", 
   "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", 
   "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", 
   "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };
    public static string ToCurrencyString(double number)
    {
        string zero ="0";
        if(-1d < number && number<1d)
        {
            return zero;
        }

        if(double.IsInfinity(number))
        {
            return "Infinity";
        }

        //부호 출력 문자열
        string significant =(number<0)? "-" : string.Empty;

        //보여줄 숫자
        string showNumber =string.Empty;

        //단위 문자열
        string unityString = string.Empty;

        //패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit =number.ToString("E").Split('+');


        //예외처리
        if(partsSplit.Length <2)
        {
            return zero;
        }

        //지수(자릿수 표현)
        if(!int.TryParse(partsSplit[1], out int exponet))
        {
            Debug.LogWarningFormat("Failed0 ToCurrentString({0}): partsSolit[1] ={1}"
            ,number,partsSplit[1]);
            return zero;
        }

        //몫은 문자열 인덱스
        int quotient = exponet/3;

        //나머지 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponet %3;
        
        //1A는 그냥 표현
        if(exponet <3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }

        else
        {
            var temp =double.Parse(partsSplit[0].Replace("E",""))*System.Math.Pow(10,remainder);
        
        showNumber =temp.ToString("F").Replace(".00","");
        
        }

        unityString =currencyUnits[quotient];

        return string.Format("{0}{1}{2}",significant,showNumber,unityString);





    }
 
}
