using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        get { return Mathf.RoundToInt(per); } // Converting float to int
        set
        {
            per = value;
            needMoneyText.text = $"{ScoreShow(needMoney)}"; // Assuming ScoreShow is a method for formatting
        }
    }

    private void Start()
    {
        LoadLevel(sceneNumber);
    }

    public void LoadLevel(int sceneIndex)
    {
        // Simulate a loading process for demonstration
        slider.fillAmount = 0f;

        // Tween the fill amount of the slider to full over slidingTime duration
        DOTween.To(() => slider.fillAmount, x => slider.fillAmount = x, 1f, slidingTime)
               .OnUpdate(UpdatePercentage) // Update the percentage text while loading
               .OnComplete(() => SceneManager.LoadScene(sceneIndex)); // Load the scene after the tween
    }

    private void UpdatePercentage()
    {

        percentageText.text = $"{Mathf.FloorToInt(slider.fillAmount * 100)}%";
    }

    // Assuming this method formats the score
    private string ScoreShow(int score)
    {
        return score.ToString("N0"); // Formats the number with commas
    }
}
