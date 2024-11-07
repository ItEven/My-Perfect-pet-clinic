using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    [Header("Bed Dependencies")]
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    public bool bIsOccupied;
    public Upgrader upGrader;
    public OnTrigger onTrigger;

    [Header("Tranfroms")]
    public Transform petDignosPos;
    public Seat petOwnerSeat;

    [Header("NPC Details")]
    public StaffNPC staffNPC;

    [Header("Visuals")]
    public Image worldProgresBar;
    public GameObject unlockObjs;
    public SpriteRenderer groundCanvas;
    public ParticleSystem roundUpgradePartical;


    #region Initializers
    internal SaveManager saveManager;
    internal GameManager gameManager;

    private void OnEnable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        gameManager = saveManager.gameManager;
    }
    #endregion

    #region Starters
    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        UpdateInitializers();
        SetVisual();
    }

    private void SetVisual()
    {
        if (bIsUnlock)
        {
            gameManager.DropObj(unlockObjs);
            gameManager.PlayParticles(roundUpgradePartical);
            Destroy(upGrader.gameObject);
            LoadNpcData();
        }
    }

    public void LoadNpcData()
    {
        staffNPC.SetMainSeat(onTrigger.seat);

    }


    #endregion
}
