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

        if (!PlayerPrefs.HasKey("PatientFollowTutorial"))
        {
            StartCoroutine(PatientFollow());

        }


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
            //DOVirtual.DelayedCall(2f, () =>
            //{
            textBox.DOMove(lastPos, 1f).OnComplete(() =>
            {
                StartCoroutine(TypeText(text));
            });
            //});
        }
    }

    private bool stopTyping = false;
    private IEnumerator TypeText(string fullText = null)
    {
        avtarText.text = "";
        if (fullText != null)
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                if (stopTyping)
                {
                    avtarText.text = fullText; // Complete the text if typing is stopped
                    yield break; // Exit the coroutine
                }
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
    public void StopTyping()
    {
        stopTyping = true;
    }

    int TextCount = 0;
    public string GetText()
    {
        TextCount++;
        Debug.LogError(TextCount);
        switch (TextCount)
        {
            case 1: return "Welcome to this beautiful town! The pets here need you.";
            case 2: return "Welcome to this beautiful town! The pets here need you.";
            case 3: return "Today, you’re appointed as the owner of the first pet hospital.";
            case 4: return "Let’s collect the money and unlock the hospital to give every furry friend the care they deserve.";
            default:
                return null;
        }
    }

    #endregion


    IEnumerator StartTutorial()
    {
        StartTextBox(GetText());
        yield return new WaitForSeconds(2);
        avtarTapBtn.onClick.AddListener(() =>
      {
          StopTyping();
          StopCoroutine(TypeText());

          StartCoroutine(TypeText(GetText()));
      });
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Tap To Continue..!";
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
        StartCoroutine(PatientFollow());
        yield break;
    }

    IEnumerator PatientFollow()
    {
        saveManager.gameManager.playerController.arrowController.target = null;
        saveManager.gameManager.playerController.arrowController.arrowIcon.SetActive(false);
        bIsTutorialRunning = true;
        joyStick.gameObject.SetActive(false);
        yield return new WaitUntil(() => taskManager.receptionManager.waitingQueue.patientInQueue.Count > 2);
        CameraController.Instance.FollowPatient(taskManager.receptionManager.waitingQueue.patientInQueue[0].transform);
        StartTextBox("Customer's are waiting for you on Reception ");
        yield return new WaitForSeconds(3.5f);
        avtarTextPanel.gameObject.SetActive(false);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Set on the reception table ";
        CameraController.Instance.MoveToTarget(taskManager.receptionManager.seat.transform);
        joyStick.gameObject.SetActive(true);
        saveManager.gameManager.playerController.arrowController.target = taskManager.receptionManager.seat.transform;
        yield return new WaitUntil(() => taskManager.receptionManager.bIsPlayerOnDesk);
        messageBoxPanel.gameObject.SetActive(false);
        yield return new WaitUntil(() => taskManager.InspectionRoom.waitingQueue.patientInQueue.Count > 0);
        CameraController.Instance.FollowPatient(taskManager.InspectionRoom.waitingQueue.patientInQueue[0].transform);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => !taskManager.InspectionRoom.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Go for diagnose the pet";
        yield return new WaitUntil(() => taskManager.InspectionRoom.bedsArr[0].bIsPlayerOnDesk);
        messageBoxPanel.gameObject.SetActive(false);
        yield return new WaitUntil(() => taskManager.pharmacyRoom.waitingQueue.patientInQueue.Count > 0);
        CameraController.Instance.FollowPatient(taskManager.pharmacyRoom.waitingQueue.patientInQueue[0].transform);
        yield return new WaitUntil(() => !taskManager.pharmacyRoom.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Give some medicine to pet ";
        yield return new WaitUntil(() => taskManager.pharmacyRoom.bedsArr[0].bIsPlayerOnDesk);
        messageBoxPanel.gameObject.SetActive(false);
        yield return new WaitUntil(() => taskManager.pharmacyRoom.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        CameraController.Instance.FollowPatient(taskManager.InspectionRoom.waitingQueue.patientInQueue[0].transform);
        PlayerPrefs.SetInt("PatientFollowTutorial", 0);
        yield return null;




    }
}
