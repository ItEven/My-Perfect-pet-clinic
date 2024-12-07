using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SupersonicWisdomSDK;

public class LoaderManager : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject LoadingScreen;
    public int sceneNumber;
    public Image slider;
    public Text percentageText;
    public Text needMoneyText;
    public float slidingTime = 2f;

    private float per;
    internal int needMoney
    {
        get { return Mathf.RoundToInt(per); } 
        set
        {
            per = value;
            needMoneyText.text = $"{ScoreShow(needMoney)}"; 
        }
    }

    void Awake()
    {
        // Subscribe
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
        // Then initialize
        SupersonicWisdom.Api.Initialize();
    }

    void OnSupersonicWisdomReady()
    {
        LoadLevel(sceneNumber);
    }
  
    public void LoadLevel(int sceneIndex)
    {

        slider.fillAmount = 0f;

        
        DOTween.To(() => slider.fillAmount, x => slider.fillAmount = x, 1f, slidingTime)
               .OnUpdate(UpdatePercentage) 
               .OnComplete(() => SceneManager.LoadScene(sceneIndex)); 
    }

    private void UpdatePercentage()
    {

        percentageText.text = $"{Mathf.FloorToInt(slider.fillAmount * 100)}%";
    }

   
    private string ScoreShow(int score)
    {
        return score.ToString("N0"); 
    }
}
