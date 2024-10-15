using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;



public class Upgrader : MonoBehaviour
{
    [Header("Details")]
    public TMP_Text needMoneyText;
    private int NeedMoney;
    public int currentNeedMoney;
    public float totalTime = 5f;
    public float spwanBetweenTime = 0.05f;
    public float jumpTime = 0.4f;
    public float jumpHight = 2.5f;
    public float fadeOutTime = 0.5f;
    public float fadeInTime = 0.001f;


    public GameObject SingleMoneybrick;
    public UnityEvent OnUpgradeFinish;
    public Transform moneyCollectPonit;
    private PlayerController player;
    private SaveManager saveManager;
    private UiManager uiManager;
    private EconomyManager economyManager;

    internal int needMoney
    {
        get { return NeedMoney; }
        set
        {
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
    }


    public void SetData(int val)
    {
        needMoney = val;

        currentNeedMoney = needMoney;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && economyManager.bCanWeSpendPetMoney(needMoney))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            if (player.IsMoving())
            {
                StartTakeMoney();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTakeMoney();
        }
    }

    private void StartTakeMoney()
    {
        if (takeMoneyCoroutine == null)
        {
            StartCoroutine(MoneySpwaing());
            takeMoneyCoroutine = StartCoroutine(TakingMoney());
            
        }
    }

    private void StopTakeMoney()
    {
        if (takeMoneyCoroutine != null)
        {
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

        float elapsedTime = 0f;

        while (needMoney > 0)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = Mathf.Clamp01(elapsedTime / totalTime);
            needMoney = Mathf.RoundToInt(Mathf.Lerp(needMoney, 0, percentageComplete));
            var val = currentNeedMoney - needMoney;

            if (economyManager.bCanWeSpendPetMoney(val - lastSub))
            {
                Debug.Log("Current Value: " + needMoney);

              
                economyManager.SpendPetMoney(val - lastSub);
                lastSub = val;

                if (needMoney <= 0)
                {
                    gameObject.SetActive(false);
                    OnUpgradeFinish.Invoke();
                    StopTakeMoney();
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
        }
    }

}