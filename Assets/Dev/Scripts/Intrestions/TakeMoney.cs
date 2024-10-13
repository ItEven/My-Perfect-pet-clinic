using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TakeMoney : MonoBehaviour
{
    [Header("Details")]
    public TextMesh needMoneyText;
    private float NeedMoney;
    public int currentNeedMoney;
    public float totalTime = 5f;
    public GameObject SingleMoneybrick;
    public UnityEvent OnMoneyTakingFinish;

    private PlayerController player;
    private SaveManager saveManager;
    private UiManager uiManager;
    private EconomyManager economyManager;

    internal float needMoney
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
        needMoney = currentNeedMoney;
    }


    public void SetData(RoomHandler RoomHandler)
    {
        roomHandler = RoomHandler;
    }
    RoomHandler roomHandler;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && economyManager.bCanWeSpendPetMoney(needMoney))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            if (player.IsMoving())
            {
               // StartTakeMoney( roomHandler.);
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
            takeMoneyCoroutine = StartCoroutine(TakingMoney());
        }
    }

    private void StopTakeMoney()
    {
        if (takeMoneyCoroutine != null)
        {
            currentNeedMoney = (int)needMoney;
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

                GameObject brickInstance = Instantiate(SingleMoneybrick, player.moneyCollectPoint.position, Quaternion.identity, player.transform);
                var brick = brickInstance.GetComponent<SingleMoneybrick>();

                economyManager.SpendPetMoney(val - lastSub);
                lastSub = val;

                OnMoneyTakingFinish.Invoke();
                if (brick != null)
                {
                    brick.StartJump(transform);
                }

                if (needMoney <= 0)
                {
                    gameObject.SetActive(false);
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
}
