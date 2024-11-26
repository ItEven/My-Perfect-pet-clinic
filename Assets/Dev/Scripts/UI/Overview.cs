using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public float ratingAndSuccesRate
    {
        get
        {
            if (hospitalData.patientCount > 0)
            {
                return (hospitalData.failedPatientCount / hospitalData.patientCount) * 100;
            }
            else
            {
                return 0;
            }
        }
    }

    [Header("Player Speed")]
    public int maxSpeedLevel;
    public float speedMultiplair;
    public float UpgradeSpeedCost;
    public float upgradeSpeedCost
    {
        get { return UpgradeSpeedCost; }
        set { UpgradeSpeedCost = value; }
    }
    public float upgradeSpeedCostMultiplair;
    public Image speedImage;
    public RectTransform speedMax;
    public Text currentUpgraderCostText;
    public Button speedBtn;

    [Header("Player Profit")]
    public int maxProfitLevel;
    public float profitMultiplair;
    public float UpgradeProfitCost;
    public float upgradeProfitCost
    {
        get { return UpgradeProfitCost; }
        set { UpgradeProfitCost = value; }
    }
    public float profitUpgradeCostMultiplair;
    public Image profitImage;
    public RectTransform profitMax;
    public Text currentProfitUpgraderCostText;
    public Button profitBtn;


    #region Initializers

    SaveManager saveManager;
    EconomyManager economyManager;
    UiManager uiManager;
    PlayerData playerData;
    HospitalData hospitalData;
    HospitalManager hospitalManager;
    GameManager gameManager;
    private void OnEnable()
    {
        UpdateInitializers();
    }
    private void OnDisable()
    {
        hospitalManager.OnPatientCountUpdate -= UpdateHospitlData;
        economyManager.OnPetMoneyChanged -= UpdateButtons;

    }

    public void UpdateInitializers()
    {
        hospitalManager = gameObject.GetComponent<HospitalManager>();
        saveManager = SaveManager.instance;
        economyManager = saveManager.economyManager;
        gameManager = saveManager.gameManager;
        uiManager = saveManager.uiManager;
        playerData = saveManager.gameData.playerData;
        hospitalData = saveManager.gameData.hospitalData;
        hospitalManager.OnPatientCountUpdate += UpdateHospitlData;
        economyManager.OnPetMoneyChanged += UpdateButtons;
    }

    #endregion

    public void Start()
    {
        btn.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        speedBtn.onClick.RemoveAllListeners();
        profitBtn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OpneOverviewPanel);
        closeBtn.onClick.AddListener(OpneOverviewPanel);
        speedBtn.onClick.AddListener(UpgradeSpeed);
        profitBtn.onClick.AddListener(UpgradeProfit);
        SetUpData();
    }

    public void SetUpData()
    {

        UpdateHospitlData();
        UpdateButtons();
        SetData();


    }
    public void OpneOverviewPanel()
    {
     
        if (backgroundPanel.gameObject.activeInHierarchy)
        {
            UiManager.instance.ClosePanel(backgroundPanel, panelBgImagel, panel);
        }
        else
        {
            UiManager.instance.OpenPanel(backgroundPanel, panelBgImagel, panel);
        }
    }
    private void UpdateHospitlData()
    {
        ratingImage.fillAmount = Mathf.Clamp(ratingAndSuccesRate, 0, 5);
        successRatePatientCountText.text = ratingAndSuccesRate.ToString() + "%";
        currnetPatientCountText.text = hospitalData.patientCount.ToString();
    }

    private void UpdateButtons()
    {
        if (playerData.speedLevel > 0)
        {
            speedBtn.interactable = economyManager.bCanWeSpendPetMoney(upgradeSpeedCost);
        }

        if(playerData.profitLevel > 0)
        {
            profitBtn.interactable = economyManager.bCanWeSpendPetMoney(UpgradeProfitCost);
        }
    }

    public void UpdateUi()
    {
        speedImage.fillAmount = (float)playerData.speedLevel / 4;
        profitImage.fillAmount = (float)playerData.profitLevel / 4;

        if (playerData.speedLevel > 0)
        {
            currentUpgraderCostText.text = uiManager.ScoreShow(upgradeSpeedCost);
        }
        else
        {
            currentUpgraderCostText.text = "Free";
            speedBtn.interactable = true;
        }

        if (playerData.profitLevel > 0)
        {
            currentProfitUpgraderCostText.text = uiManager.ScoreShow(upgradeProfitCost);
        }
        else
        {
            currentProfitUpgraderCostText.text = "Free";
            profitBtn.interactable = true;
        }
        UpdateButtons();
    }
    private void UpgradeSpeed()
    {
        if (playerData.speedLevel > 0)
        {

            if (economyManager.bCanWeSpendPetMoney(upgradeSpeedCost) && playerData.speedLevel < maxSpeedLevel)
            {
                playerData.speedLevel++;
                gameManager.playerController.playerControllerData.maxSpeed += speedMultiplair;
                economyManager.bCanWeSpendPetMoney(upgradeSpeedCost);
                upgradeSpeedCost *= upgradeSpeedCostMultiplair;
                if (playerData.speedLevel >= maxSpeedLevel)
                {
                    speedBtn.gameObject.SetActive(false);
                    speedMax.gameObject.SetActive(true);
                }
                UpdateUi();
            }
        }
        else
        {
            playerData.speedLevel++;
            gameManager.playerController.playerControllerData.maxSpeed += speedMultiplair;
            UpdateUi();

        }
    }
    private void UpgradeProfit()
    {
        if (playerData.profitLevel > 0)
        {
            if (economyManager.bCanWeSpendPetMoney(upgradeProfitCost) && playerData.profitLevel < maxProfitLevel)
            {
                playerData.profitLevel++;
                gameManager.profitMultiplier += profitMultiplair;
                economyManager.bCanWeSpendPetMoney(upgradeProfitCost);
                upgradeProfitCost *= profitUpgradeCostMultiplair;
                if (playerData.profitLevel >= maxProfitLevel)
                {
                    profitBtn.gameObject.SetActive(false);
                    profitMax.gameObject.SetActive(true);
                }
                UpdateUi();
            }
        }
        else
        {
            playerData.profitLevel++;
            gameManager.profitMultiplier += profitMultiplair;
            UpdateUi();

        }
    }

    public void SetData()
    {
        for (int i = 1; i < playerData.profitLevel; i++)
        {
            upgradeProfitCost *= profitUpgradeCostMultiplair;

        }

        for (int i = 1; i < playerData.speedLevel; i++)
        {
            upgradeSpeedCost *= upgradeSpeedCostMultiplair;

        }
        UpdateUi();
    }


}
