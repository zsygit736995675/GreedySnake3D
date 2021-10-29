using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringAddUnit
{
    public static string addUnit (this string _value) {


        return Numdispose(_value);
    }
    public static string addUnit (this int _value)
    {

        return Numdispose(_value.ToString ());
    }

    /// <summary>
    /// 数字换算
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Numdispose (string num)
    {
        string [] symbol = { "K", "M", "B", "T", "aa", "ab", "ac", "ad" };

        string str1 = string. Empty;

        string str2 = string. Empty;

        if (num. Length > 4)
        {
            int a = (num. Length - 4) / 3;

            str1 = num. Substring(0, (num. Length - (3 * (a + 1))));

            int b = num. Length - (3 * (a + 1));

            str2 = num [ b ]. ToString();

            if (int. Parse(str2) >= 5) str1 = (int. Parse(str1) + 1). ToString();

            if (str1. Length > 3) return str1. Insert(str1. Length - 3, ",") + symbol [ a ];

            return str1 + symbol [ a ];
        }
        //if (num. Length > 3 && num. Length < 7)
        //{
        //    return num. Insert(num. Length - 3, ",");
        //}
        return num;
    }
}
