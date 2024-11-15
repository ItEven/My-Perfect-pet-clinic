using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Overview : MonoBehaviour
{
    [Header("Main Panel")]
    public RectTransform backgroundPanel;
    public RectTransform panel;
    public Image panelBgImagel;
    public Button btn;
    public Button closeBtn;

    [Header("Hospital")]
    public Image ratingImage;
    public Text currnetPatientCountText;
    public Text successRatePatientCountText;

    [Header("Player")]
    public int maxSpeedLevel;
    public float speed;
    public float speedMultiplair;
    public float UpgradeCost;
    public float upgradeCost
    {
        get { return UpgradeCost; }
        set { UpgradeCost = value; currentUpgraderCostText.text = uiManager.ScoreShow(UpgradeCost); }
    }
    public float UpgradeCostMultiplair;
    public Image speedImage;
    public Text currentUpgraderCostText;
    public Button speedBtn;

    #region Initializers
    
    SaveManager saveManager;
    UiManager uiManager;
    PlayerData playerData;
    HospitalData hospitalData;
    HospitalManager hospitalManager;
    private void OnEnable()
    {
        UpdateInitializers();
    }
    private void OnDisable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        uiManager = saveManager.uiManager;
        playerData = saveManager.gameData.playerData;
        hospitalData = saveManager.gameData.hospitalData;
    }

    #endregion

    public void Start()
    {
        hospitalManager = gameObject.GetComponent<HospitalManager>();
    }



}
