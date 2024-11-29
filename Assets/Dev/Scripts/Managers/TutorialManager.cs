using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("UI recfs")]
    public RectTransform moneyBoxRect;
    public RectTransform taskBoxRect;
    public RectTransform settingBoxRect;
    public RectTransform overviweBoxRect;
    public RectTransform illnessesBoxRect;
    public RectTransform campasBoxRect;
    public RectTransform msgBoxRect;
    public TextMeshProUGUI msgText;

    internal bool Tutorial;
    private void Awake()
    {
        instance = this;
    }

    #region Initializers

    SaveManager saveManager;
    UiManager uiManager;

    private void OnEnable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        uiManager = saveManager.uiManager;
    }

    #endregion

    IEnumerator StartTutorial()
    {
        yield return new WaitUntil(() => Tutorial);
    }
}
