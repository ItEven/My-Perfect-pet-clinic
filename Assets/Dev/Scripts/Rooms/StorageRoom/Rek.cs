using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rek : MonoBehaviour
{


    [Header("Rek Details")]
    public bool bIsUnlock;


    [Header("Visuals")]
    public Image worldProgresBar;
    public SpriteRenderer groundCanvas;
    public ParticleSystem roundUpgradePartical;

    [Header("Items Details")]
    public int currnetItemsCount;
    public float itemReFillTime;
    public GameObject itemPrefab;
    public Transform[] itemsPostionArr;
    public Items[] itemsArr;
    private ItemsCarryhandler itemsCarryhandler;
    private bool bIsTaking;


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
        gameManager.PlayParticles(roundUpgradePartical);
        FillItems();
    }

    #endregion


    #region Items Mecheics
    public void FillItems()
    {
        for (int i = 0; i < itemsArr.Length; i++)
        {
            var item = itemsArr[i];
            if (item == null)
            {
                GameObject gameObject = Instantiate(itemPrefab, itemsPostionArr[i].position, Quaternion.identity, itemsPostionArr[i]);
                itemsArr[i] = gameObject.GetComponent<Items>();
            }
        }
        gameManager.PlayParticles(roundUpgradePartical);
        AudioManager.i.OnUpgrade();
        worldProgresBar.fillAmount = 0;



    }

    protected Tween Tw_ItemReFiller;

    public void ReFillItems()
    {
        DOTween.Kill("Refil");
        worldProgresBar.fillAmount = 0;
        worldProgresBar.DOFillAmount(1, itemReFillTime).SetId("Refil").OnComplete(() =>
        {
            FillItems();
            worldProgresBar.fillAmount = 0;
        });
    }

    public void StopGiving()
    {
        if (itemsCarryhandler != null)
        {
            itemsCarryhandler.StartCoroutine();
        }
        bIsTaking = false;
    }




    public void StartTakingItems()
    {
        itemsCarryhandler = gameManager.playerController.itemsCarryhandler;
        bIsTaking = true;
        itemsCarryhandler.TakeItem(this);
    }

    public int GetItemIndex()
    {

        for (int i = 0; i < itemsArr.Length; i++)
        {
            var item = itemsArr[i];
            if (item != null)
            {
                return i;
            }
        }
        return -1;
    }

    public void CheckIfRefillNeed()
    {
        foreach (var item in itemsArr)
        {
            if (item == null)
            {
                ReFillItems();
                break;
            }
        }
    }

    #endregion

    #region Triggers


    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !gameManager.playerController.IsMoving())
        {
            if (!bIsTaking)
            {
                StartTakingItems();
                groundCanvas.gameObject.SetActive(true);
            }
        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopGiving();
            groundCanvas.gameObject.SetActive(false);
        }
    }
    #endregion

}
