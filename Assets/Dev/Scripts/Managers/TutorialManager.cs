using DG.Tweening;
using DG.Tweening.Core.Easing;
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
    internal bool bIsTutorialRunning = false;
    internal bool Tutorial;

    [Header("YO nig")]
    public GameObject moneyBox;
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

    bool bIsGameRestart;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            bIsGameRestart = false;
            StartCoroutine(StartTutorial());
        }
        else
        {
            bIsGameRestart = true;

        }
        StartCoroutine(CheckHud());




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
    }
    public void StopTyping()
    {
        stopTyping = true;
    }

    int TextCount = 0;
    public string GetText()
    {
        TextCount++;

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
    int TextCountt = 0;
    public string GetTextForLastTut()
    {
        TextCountt++;

        switch (TextCountt)
        {
            case 1: return "The first pet has recovered well done! ";
            case 2: return "It seems you are very talented.";
            case 3: return "However, don’t relax too soon; pets are pouring in continuously..";
            case 4: return "Let’s see if we can treat 5 more pets!.";
            default:
                return null;
        }
    }

    #endregion


    IEnumerator StartTutorial()
    {
        bIsTutorialRunning = true;
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
        yield return new WaitUntil(() => TextCount > 4);
        avtarTextPanel.gameObject.SetActive(false);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Collect the Money!";
        moneyBox.gameObject.SetActive(true);
        MoneyBox money = moneyBox.GetComponent<MoneyBox>();

        yield return new WaitUntil(() => money.totalMoneyInBox <= 0);
        taskManager.hallManager_01.bIsUpgraderActive = true;
        taskManager.hallManager_01.SetUpgredeVisual();
        messageText.text = "Open the Pet Hospital!";
        yield return new WaitUntil(() => taskManager.hallManager_01.bIsUnlock);
        PlayerPrefs.SetInt("Tutorial", 0);
        yield break;

    }

    IEnumerator CheckHud()
    {
        bIsTutorialRunning = true; 
        moneyBoxRect.gameObject.SetActive(false);
        taskBoxRect.gameObject.SetActive(false);
        settingBoxRect.gameObject.SetActive(false);
        overviweBoxRect.gameObject.SetActive(false);
        illnessesBoxRect.gameObject.SetActive(false);
        campasBoxRect.gameObject.SetActive(false);
        messageBoxPanel.gameObject.SetActive(false);
        //FrizPlayer();

        //yield return new WaitUntil(() => !textBox.gameObject.activeInHierarchy);
        //UnFrizPlayer();
        yield return new WaitUntil(() => saveManager.economyManager.PetMoneyCount > 0);
        moneyBoxRect.gameObject.SetActive(true);
        yield return new WaitUntil(() => taskManager.hallManager_01.bIsUnlock);
        //messageBoxPanel.gameObject.SetActive(false);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Build The Reception Desk!";
        yield return new WaitUntil(() => taskManager.receptionManager.bIsUnlock);
        taskBoxRect.gameObject.SetActive(true);
        settingBoxRect.gameObject.SetActive(true);
        messageText.text = "Build the General Diagnosis Room!";
        yield return new WaitUntil(() => taskManager.InspectionRoom.bIsUnlock);
        messageText.text = "Set Up the Pharmacy!";
        yield return new WaitUntil(() => taskManager.pharmacyRoom.bIsUnlock);

        overviweBoxRect.gameObject.SetActive(true);
        illnessesBoxRect.gameObject.SetActive(true);
        campasBoxRect.gameObject.SetActive(true);
        messageBoxPanel.gameObject.SetActive(false);
        if (!PlayerPrefs.HasKey("PatientFollowTutorial"))
        {
           // FrizPlayer();
            StartCoroutine(PatientFollow());

        }
        else
        {
            saveManager.gameManager.playerController.arrowController.target = null;
            saveManager.gameManager.playerController.arrowController.arrowIcon.SetActive(false);
            bIsTutorialRunning = false;
        }
        yield break;
    }

    IEnumerator PatientFollow()
    {

        bIsTutorialRunning = true;
        saveManager.gameManager.playerController.arrowController.target = null;
        saveManager.gameManager.playerController.arrowController.arrowIcon.SetActive(false);
        yield return new WaitUntil(() => taskManager.receptionManager.waitingQueue.patientInQueue.Count > 2);
        if (bIsGameRestart)
        {
            yield return new WaitForSeconds(3f);
        }
        CameraController.Instance.FollowPatient(taskManager.receptionManager.waitingQueue.patientInQueue[0].transform);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Patinet's are coming!";
    
        yield return new WaitUntil(() => !taskManager.receptionManager.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        yield return new WaitForSeconds(2f);
            FrizPlayer();
        avtarTextPanel.gameObject.SetActive(false);
        //messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Take your seat at Reception!";
       // CameraController.Instance.MoveToTarget(taskManager.receptionManager.seat.transform);
        saveManager.gameManager.playerController.arrowController.target = taskManager.receptionManager.seat.transform;
        //yield return new WaitForSeconds(2f);
        UnFrizPlayer();
        yield return new WaitUntil(() => taskManager.receptionManager.bIsPlayerOnDesk);
        messageBoxPanel.gameObject.SetActive(false);
        yield return new WaitUntil(() => taskManager.InspectionRoom.waitingQueue.patientInQueue.Count > 0);
        CameraController.Instance.FollowPatient(taskManager.InspectionRoom.waitingQueue.patientInQueue[0].transform);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => !taskManager.InspectionRoom.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Diagnose the Pets!";
        saveManager.gameManager.playerController.arrowController.target = taskManager.InspectionRoom.bedsArr[0].onTrigger.seat.transform;
        yield return new WaitUntil(() => taskManager.InspectionRoom.bedsArr[0].bIsPlayerOnDesk);
        messageBoxPanel.gameObject.SetActive(false);
        yield return new WaitUntil(() => taskManager.pharmacyRoom.waitingQueue.patientInQueue.Count > 0);
        CameraController.Instance.FollowPatient(taskManager.pharmacyRoom.waitingQueue.patientInQueue[0].transform);
        yield return new WaitUntil(() => !taskManager.pharmacyRoom.waitingQueue.patientInQueue[0].NPCMovement.bIsMoving);
        Patient patient = taskManager.pharmacyRoom.waitingQueue.patientInQueue[0];
        messageBoxPanel.gameObject.SetActive(true);
        messageText.text = "Provide the Medicine! ";
        saveManager.gameManager.playerController.arrowController.target = taskManager.pharmacyRoom.bedsArr[0].onTrigger.seat.transform;
        yield return new WaitUntil(() => taskManager.pharmacyRoom.bedsArr[0].bIsPlayerOnDesk);
        messageBoxPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => patient.NPCMovement.bIsMoving);
        patient.emojisController.PlayEmoji(MoodType.Happy);
        CameraController.Instance.FollowPatient(patient.transform);
        saveManager.gameManager.playerController.arrowController.arrowIcon.SetActive(false);
        saveManager.gameManager.playerController.arrowController.enabled = false;
        yield return new WaitForSeconds(3f);
        StartTextBox(GetTextForLastTut());
        avtarTapBtn.onClick.RemoveAllListeners();
        avtarTapBtn.onClick.AddListener(() =>
        {
            StopTyping();
            StopCoroutine(TypeText());

            StartCoroutine(TypeText(GetTextForLastTut()));
        });
        yield return new WaitUntil(() => TextCountt > 4);
        avtarTextPanel.gameObject.SetActive(false);
        bIsTutorialRunning = false;
        taskManager.OnTaskComplete(4);
        PlayerPrefs.SetInt("PatientFollowTutorial", 0);

        yield return null;
    }

    public void FrizPlayer()
    {

        saveManager.gameManager.playerController.playerControllerData.characterMovement.enabled = false;
        saveManager.gameManager.playerController.enabled = false;
        saveManager.gameManager.playerController.playerControllerData.joystick.gameObject.SetActive(false);
        saveManager.gameManager.playerController.animationController.PlayAnimation(AnimType.Idle);

    }

    public void UnFrizPlayer()
    {
        saveManager.gameManager.playerController.playerControllerData.joystick.gameObject.SetActive(true);
        saveManager.gameManager.playerController.playerControllerData.joystick.OnPointerUp(null);
        saveManager.gameManager.playerController.playerControllerData.characterMovement.enabled = true;
        if (saveManager.gameManager.playerController.animationController.GetCurrntAnimState() == AnimType.Run.ToString())
        {
            saveManager.gameManager.playerController.animationController.PlayAnimation(AnimType.Idle);
        }
    }
}
