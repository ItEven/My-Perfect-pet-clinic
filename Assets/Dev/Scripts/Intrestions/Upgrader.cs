using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Upgrader : MonoBehaviour
{
    [Header("Details")]
    private int NeedMoney;
    public int currentNeedMoney;
    public float totalTime = 5f;
    public float spwanBetweenTime = 0.05f;
    public float jumpTime = 0.4f;
    public float jumpHight = 2.5f;
    public float fadeOutTime = 0.5f;
    public float fadeInTime = 0.001f;
    public TMP_Text needMoneyText;
    public SpriteRenderer indicationIcon;

    bool bIsPlayerStay;
    public GameObject SingleMoneybrick;
    public UnityEvent OnUpgradeFinish;
    public Transform moneyCollectPonit;
    private PlayerController player;
    private SaveManager saveManager;
    private UiManager uiManager;
    private EconomyManager economyManager;

    [Header("Sprites")]
    public Sprite addSprtie;
    public Sprite arrowSprtie;

    [Header("Arrow")]
    public bool bCanArrowWork = false;
    public bool bCanUsePLayerArrow = false;
    public Transform arrow;
    public Transform newArrow;

    internal int needMoney
    {
        get { return NeedMoney; }
        set
        {
            UpdateInitializers();
            NeedMoney = value;
            needMoneyText.text = $"{uiManager.ScoreShow(needMoney)}";
        }
    }

    private Coroutine takeMoneyCoroutine;

    private void OnEnable() => UpdateInitializers();
    private void OnDisable() => UpdateInitializers();

    private void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        economyManager = saveManager.economyManager;
        uiManager = saveManager.uiManager;
        //needMoney = currentNeedMoney;
        if (bCanArrowWork)
        {
            if (!arrow.gameObject.activeInHierarchy)
            {
                arrow.gameObject.SetActive(true);
            }
        }
    }

    private void Start()
    {
        Arrow();
        if (bCanUsePLayerArrow)
        {
            saveManager.gameManager.playerController.arrowController.gameObject.SetActive(true);
            saveManager.gameManager.playerController.arrowController.SetTarget(transform, 2f);
        }

    }
    public void SetData(int val)
    {
        needMoney = val;

        currentNeedMoney = needMoney;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bCanArrowWork)
            {
                arrow.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arrow.gameObject.SetActive(false);

            if (needMoney > 0 && (float)economyManager.PetMoneyCount > 0)
            {
                player = other.gameObject.GetComponent<PlayerController>();
                if (!player.IsMoving())
                {
                    bIsPlayerStay = true;
                    StartTakeMoney();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bCanArrowWork)
            {
                arrow.gameObject.SetActive(true);
            }
            bIsPlayerStay = false;
        }
    }

    public void SetUpgraderSprite()
    {
        indicationIcon.sprite = arrowSprtie;
        indicationIcon.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    private void StartTakeMoney()
    {
        if (takeMoneyCoroutine == null)
        {
            StartCoroutine(MoneySpwaing());
            takeMoneyCoroutine = StartCoroutine(TakingMoney());
        }

        if (currentNeedMoney == 0)
        {
            OnUpgradeFinish.Invoke();
            StopTakeMoney();
            gameObject.SetActive(false);
        }
    }

    public void StopTakeMoney()
    {
        if (takeMoneyCoroutine != null)
        {
            lastSub = 0f;
            currentNeedMoney = (int)needMoney;
            StopCoroutine(MoneySpwaing());
            StopCoroutine(takeMoneyCoroutine);
            takeMoneyCoroutine = null;
        }
    }

    float lastSub = 0f;
    private IEnumerator TakingMoney()
    {

        if (needMoney <= 0) yield break;
        if (economyManager.PetMoneyCount <= 0) yield break;

        float elapsedTime = 0f;

        while (needMoney > 0)
        {
            arrow.gameObject.SetActive(false);
            if (!bIsPlayerStay || (float)economyManager.PetMoneyCount <= 0)
            {
                StopTakeMoney();
                yield break;

            }
            elapsedTime += Time.deltaTime;
            float percentageComplete = Mathf.Clamp01(elapsedTime / totalTime);


            if (economyManager.PetMoneyCount < needMoney)
            {
                needMoney = Mathf.RoundToInt(Mathf.Lerp(needMoney, needMoney - (float)economyManager.PetMoneyCount, percentageComplete));
            }
            else
            {

                needMoney = Mathf.RoundToInt(Mathf.Lerp(needMoney, 0, percentageComplete));
            }

            var val = currentNeedMoney - needMoney;


            if (economyManager.bCanWeSpendPetMoney(val - lastSub) && (float)economyManager.PetMoneyCount > 0)
            {


                economyManager.SpendPetMoney(val - lastSub);
                lastSub = val;

                if (needMoney <= 0)
                {
                    arrow.gameObject.SetActive(false);
                    OnUpgradeFinish.Invoke();
                    StopTakeMoney();
                    gameObject.SetActive(false);
                    yield break;
                }
            }
            else
            {
                StopTakeMoney();
                yield break;
            }

            yield return null;
        }

        //if (needMoney <= 0)
        //{
        //    OnMoneyTakingFinish.Invoke();
        //    gameObject.SetActive(false);
        //    StopTakeMoney();
        //}
    }

    IEnumerator MoneySpwaing()
    {

        while (true)
        {
            if (!bIsPlayerStay) yield break;
            if (economyManager.PetMoneyCount <= 0) yield break;
            arrow.gameObject.SetActive(false);


            yield return new WaitForSeconds(spwanBetweenTime);
            GameObject brickInstance = Instantiate(SingleMoneybrick, player.moneyCollectPoint.position, Quaternion.identity, player.transform);
            var brick = brickInstance.GetComponent<SingleMoneybrick>();
            brick.jumpTime = jumpTime;
            brick.jumpHight = jumpHight;
            brick.fadeInTime = fadeInTime;
            brick.fadeOutTime = fadeOutTime;
            if (brick != null)
            {
                brick.StartJump(moneyCollectPonit);
            }
            AudioManager.i.OnMoneyDrop();
        }
    }

    public void Arrow()
    {
        arrow.DOMoveY(2.0f, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }
}
