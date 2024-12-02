using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("UI recfs")]
    public RectTransform joyStick;
    public RectTransform moneyBoxRect;
    public RectTransform taskBoxRect;
    public RectTransform settingBoxRect;
    public RectTransform overviweBoxRect;
    public RectTransform illnessesBoxRect;
    public RectTransform campasBoxRect;



    [Header("TextBoxes")]
    public RectTransform avtarTextPanel;
    public RectTransform textBox;
    public TextMeshProUGUI avtarText;
    public Button avtarTapBtn;

    [Header("Message box")]
    public RectTransform messageBoxPanel;
    public Text messageText;
    internal bool bIsTutorialRunning;
    internal bool Tutorial;
    private void Awake()
    {
        instance = this;
    }

    #region Initializers

    SaveManager saveManager;
    UiManager uiManager;
    HospitalManager hospitalManager;
    TaskManager taskManager;

    private void OnEnable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        uiManager = saveManager.uiManager;
        hospitalManager = saveManager.hospitalManager;
        taskManager = saveManager.taskManager;
    }

    #endregion

    private void Start()
    {
        StartCoroutine(CheckHud());
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            StartCoroutine(StartTutorial());
        }

        avtarTapBtn.onClick.AddListener(() =>
        {
            StopCoroutine(TypeText(GetText()));

            StartCoroutine(TypeText(GetText()));
        });
    }
    #region TextBox
    public void StartTextBox(string text)
    {
        if (!avtarTextPanel.gameObject.activeInHierarchy)
        {
            avtarText.text = "";

            avtarTextPanel.gameObject.SetActive(true);
            Vector3 lastPos = textBox.transform.position;
            Vector3 newPosition = textBox.transform.position;
            newPosition.x = -1500f;
            textBox.transform.position = newPosition;
            DOVirtual.DelayedCall(2f, () =>
            {
                textBox.DOMove(lastPos, 1f).OnComplete(() =>
                {
                    StartCoroutine(TypeText(text));
                });
            });
        }
    }

    private IEnumerator TypeText(string fullText)
    {
        avtarText.text = "";
        if (fullText != null)
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                avtarText.text += fullText[i];
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            avtarTextPanel.gameObject.SetActive(false);
            messageBoxPanel.gameObject.SetActive(true);
            messageText.text = "Collect Money !";
        }
    }

    int TextCount = 0;
    public string GetText()
    {
        TextCount++;
        switch (TextCount)
        {
            case 1: return "Welcome to this beautiful town! The pets here need you.";
            case 2: return "Today, you’re appointed as the owner of the first pet hospital.";
            case 3: return "Let’s collect the money and unlock the hospital to give every furry friend the care they deserve.";
            default:
                return null;
        }
    }

    #endregion


    IEnumerator StartTutorial()
    {
        StartTextBox(GetText());

        yield return new WaitUntil(() => taskManager.hallManager_01.bIsUnlock);
        PlayerPrefs.SetInt("Tutorial", 0);
        yield break;

    }

    IEnumerator CheckHud()
    {
        moneyBoxRect.gameObject.SetActive(false);
        taskBoxRect.gameObject.SetActive(false);
        settingBoxRect.gameObject.SetActive(false);
        overviweBoxRect.gameObject.SetActive(false);
        illnessesBoxRect.gameObject.SetActive(false);
        campasBoxRect.gameObject.SetActive(false);
        messageBoxPanel.gameObject.SetActive(false);
        joyStick.gameObject.SetActive(false);

        yield return new WaitUntil(() => !textBox.gameObject.activeInHierarchy);
        joyStick.gameObject.SetActive(true);
        yield return new WaitUntil(() => saveManager.economyManager.PetMoneyCount > 0);
        messageBoxPanel.gameObject.SetActive(false);
        moneyBoxRect.gameObject.SetActive(true);
        yield return new WaitUntil(() => taskManager.receptionManager.bIsUnlock);
        taskBoxRect.gameObject.SetActive(true);
        settingBoxRect.gameObject.SetActive(true);
        yield return new WaitUntil(() => taskManager.InspectionRoom.bIsUnlock);
        overviweBoxRect.gameObject.SetActive(true);
        illnessesBoxRect.gameObject.SetActive(true);
        campasBoxRect.gameObject.SetActive(true);
        yield break;
    }

    IEnumerator PatientFollow()
    {
        bIsTutorialRunning = true;      
        joyStick.gameObject.SetActive(true);
        yield return new WaitUntil(() => taskManager.receptionManager.waitingQueue.patientInQueue.Count > 2);
        CameraController.Instance.FollowPatient(taskManager.receptionManager.waitingQueue.patientInQueue[0].transform);
        yield return new WaitUntil(() => !taskManager.receptionManager.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        StartTextBox("Customer's are waiting for you on Reception ");
        yield return new WaitForSeconds(2f);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Set on the reception table ";
        CameraController.Instance.MoveToTarget(taskManager.receptionManager.seat.transform);



    }
}
