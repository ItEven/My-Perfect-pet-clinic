using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public static bool bIsUiOn;
    public float commenDgDuraction;


    [Header("HUD")]
    public RectTransform hudPanel;


    [Header("Loder Panel")]
    public RectTransform loderPnael;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void Start()
    {
        //DOVirtual.DelayedCall(2f, () =>
        //{
        //    loderPnael.gameObject.SetActive(false);
        //});
        //settingBtn.onClick.AddListener
        loderPnael.gameObject.SetActive(false);
    }

    public void OpenPanel(RectTransform backgroundPanel, Image bgImg, RectTransform mainPanel)
    {
        if (bIsUiOn) return;
        hudPanel.gameObject.SetActive(false);
        backgroundPanel.gameObject.SetActive(true);

        //Material material = bgImg.material;
        //material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
        bgImg.DOFade(0.9f, commenDgDuraction * 2);
        mainPanel.localScale = Vector3.zero;
        mainPanel.gameObject.SetActive(true);
        mainPanel.DOScale(Vector3.one, commenDgDuraction).SetEase(Ease.OutBounce);
        bIsUiOn = true;
    }

    public void ClosePanel(RectTransform backgroundPanel, Image bgImg, RectTransform mainPanel)
    {
        //Material material = bgImg.material;
        bgImg.DOFade(0f, commenDgDuraction).OnComplete(() =>
        {
            backgroundPanel.gameObject.SetActive(false);
        });
        mainPanel.DOScale(Vector3.zero, commenDgDuraction).SetEase(Ease.OutSine).OnComplete(() =>
        {
            mainPanel.gameObject.SetActive(false);
            hudPanel.gameObject.SetActive(true);
            bIsUiOn = false;
        });
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
