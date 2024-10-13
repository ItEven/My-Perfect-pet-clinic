using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;


    private void Awake()
    {
        instance = this;
    }


    public string ScoreShow(double Score)
    {
        string result;
        string[] ScoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (Score < 999)
                break;
            else Score =/* Math.Floor*/(Score / 100d) / 10d;

        if (Score == Math.Floor(Score))
            result = Score.ToString() + " " + ScoreNames[i];
        else result = Score.ToString("F1") + " " + ScoreNames[i];
        return result;
    }

}
