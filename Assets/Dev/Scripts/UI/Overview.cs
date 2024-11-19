using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Overview : MonoBehaviour
{
    //[Header("Main Panel")]
    //public RectTransform backgroundPanel;
    //public RectTransform panel;
    //public Image panelBgImagel;
    //public Button btn;
    //public Button closeBtn;

   
    //[Header("Hospital")]
    //public Image ratingImage;
    //public Text currnetPatientCountText;
    //public Text successRatePatientCountText;
    //public float ratingAndSuccesRate
    //{
    //    get { return (hospitalData.failedPatientCount / hospitalData.patientCount) * 100; }
    //}
   
 

    //[Header("Player Speed")]
    //public int maxSpeedLevel;
    //public float speedMultiplair;
    //public float UpgradeSpeedCost;
    //public float upgradeSpeedCost
    //{
    //    get { return UpgradeProfitCost; }
    //    set { UpgradeProfitCost = value; currentUpgraderCostText.text = uiManager.ScoreShow(UpgradeProfitCost); }
    //}
    //public float upgradeSpeedCostMultiplair;
    //public Image speedImage;
    //public RectTransform speedMax;
    //public Text currentUpgraderCostText;
    //public Button speedBtn;  
    
    //[Header("Player Profit")]
    //public int maxProfitLevel;
    //public float profitMultiplair;
    //public float UpgradeProfitCost;
    //public float upgradeProfitCost
    //{
    //    get { return UpgradeProfitCost; }
    //    set { UpgradeProfitCost = value; currentProfitUpgraderCostText.text = uiManager.ScoreShow(UpgradeProfitCost); }
    //}
    //public float profitUpgradeCostMultiplair;
    //public Image profitImage;
    //public RectTransform profitMax;
    //public Text currentProfitUpgraderCostText;
    //public Button profitBtn;


    //#region Initializers

    //SaveManager saveManager;
    //EconomyManager economyManager;
    //UiManager uiManager;
    //PlayerData playerData;
    //HospitalData hospitalData;
    //HospitalManager hospitalManager;
    //GameManager gameManager;
    //private void OnEnable()
    //{
    //    UpdateInitializers();
    //}
    //private void OnDisable()
    //{
    //    hospitalManager.OnPatientCountUpdate -= UpdateHospitlData;
    //    UpdateInitializers();
    //}

    //public void UpdateInitializers()
    //{
    //    hospitalManager = gameObject.GetComponent<HospitalManager>();
    //    saveManager = SaveManager.instance;
    //    economyManager = saveManager.economyManager;
    //    gameManager = saveManager.gameManager;
    //    uiManager = saveManager.uiManager;
    //    playerData = saveManager.gameData.playerData;
    //    hospitalData = saveManager.gameData.hospitalData;
    //    hospitalManager.OnPatientCountUpdate += UpdateHospitlData;
    //}

    //#endregion

    //public void Start()
    //{
    //}

    //public void SetUpData()
    //{ 


    //    UpdateHospitlData();
    //    UpdateButtons();
    //}

    //private void UpdateHospitlData()
    //{
    //    ratingImage.fillAmount = Mathf.Clamp(ratingAndSuccesRate, 0, 5);
    //    successRatePatientCountText.text = ratingAndSuccesRate.ToString() + "%";
    //    currnetPatientCountText.text = hospitalData.patientCount.ToString();
    //}

    //private void UpdateButtons()
    //{
    //    speedBtn.interactable = economyManager.bCanWeSpendPetMoney(upgradeSpeedCost);
    //    profitBtn.interactable = economyManager.bCanWeSpendPetMoney(UpgradeProfitCost);
    //}
   
    //private void UpgradeSpeed()
    //{
    //    if(economyManager.bCanWeSpendPetMoney(upgradeSpeedCost) && playerData.speedLevel < maxSpeedLevel)
    //    {
    //        playerData.speedLevel++;
    //        gameManager.playerController.playerControllerData.maxSpeed += speedMultiplair;
    //        economyManager.bCanWeSpendPetMoney(upgradeSpeedCost);
    //        if(playerData.speedLevel >= maxSpeedLevel)
    //        {
    //            btn.gameObject.SetActive(false);
    //            speedMax.gameObject.SetActive(true);
    //        }
    //    }
    //    else
    //    {
    //        btn.gameObject.SetActive(false);
    //        speedMax.gameObject.SetActive(true);
    //    }
    //}
    //private void UpgradeProfit()
    //{
    //    if (economyManager.bCanWeSpendPetMoney(upgradeProfitCost) && playerData.profitLevel < maxProfitLevel)
    //    {
    //        playerData.profitLevel++;
    //        gameManager.playerController.playerControllerData.maxSpeed += speedMultiplair;
    //        economyManager.bCanWeSpendPetMoney(upgradeSpeedCost);
    //        if (playerData.speedLevel >= maxSpeedLevel)
    //        {
    //            btn.gameObject.SetActive(false);
    //            speedMax.gameObject.SetActive(true);
    //        }
    //    }
    //    else
    //    {
    //        btn.gameObject.SetActive(false);
    //        speedMax.gameObject.SetActive(true);
    //    }
    //}


}
