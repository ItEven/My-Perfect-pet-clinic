using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;


[System.Serializable]
public class SettingData
{
    public bool bIsSoundOn;
    public bool bIsMusicOn;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    [SerializeField] internal AudioSource musicAudioSource;
    [SerializeField] internal AudioSource soundAudioSource;

    public SettingData settingData = new SettingData();
    public AudioClip[] bgAudioClips;
    public AudioClip upgradeClip;
    public AudioClip whoohClip;
    public AudioClip moneyCollectClip;
    public AudioClip moneyDropClip;


    [Header("Setting Panel")]
    public RectTransform settingBackgroundPanel;
    public RectTransform settingPanel;
    public Image settingPanelBgImagel;
    public Button settingBtn;
    public Button settingCloseBtn;


    [Header("Sound")]
    public Button soundBtn;
    public Image soundImg;
    public RectTransform soundCircleImg;
    public RectTransform soundOnText;
    public RectTransform soundOffText;

    [Header("Music")]
    public Button musicBtn;
    public Image musicImg;
    public RectTransform musicCircleImg;
    public RectTransform musicOnText;
    public RectTransform musicOffText;


    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(gameObject);
        }
    
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            LoadData();
        }
        else
        {
            settingData.bIsSoundOn = true;
            settingData.bIsMusicOn = true;
        }
        settingBtn.onClick.AddListener(OpneSettingPanel);
        settingCloseBtn.onClick.AddListener(OpneSettingPanel);
        soundBtn.onClick.AddListener(OnSoundButtunClick);
        musicBtn.onClick.AddListener(OnMusicButtunClick);
        SetData();
        musicAudioSource.Stop();
        Play(GetRandomClip());
    }
    public void SetData()
    {
        SetSoundSetting();
        SetMusicSetting();
    }

    public AudioClip GetRandomClip()
    {
        int index = Random.Range(0, bgAudioClips.Length);
        return bgAudioClips[index];
    }
    public void Play(AudioClip clip)
    {

        musicAudioSource.clip = clip;
        musicAudioSource.Play();
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    public void OnUpgrade()
    {
        if (!settingData.bIsSoundOn) return;
        musicAudioSource.PlayOneShot(upgradeClip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    public void OnMonenyCollect()
    {
        if (!settingData.bIsSoundOn) return;
        musicAudioSource.PlayOneShot(moneyCollectClip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    public void OnMoneyDrop()
    {
        if (!settingData.bIsSoundOn) return;
        musicAudioSource.PlayOneShot(moneyDropClip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }

    public void OpneSettingPanel()
    {
        if (settingBackgroundPanel.gameObject.activeInHierarchy)
        {
            UiManager.instance.ClosePanel(settingBackgroundPanel, settingPanelBgImagel, settingPanel);
        }
        else
        {

            UiManager.instance.OpenPanel(settingBackgroundPanel, settingPanelBgImagel, settingPanel);
        }
    }

    public void OnButtonClick(Image img, RectTransform circle, RectTransform onText, RectTransform OffText, bool bIsOn)
    {
        if (bIsOn)
        {
            img.DOFillAmount(1, 0.2f).SetEase(Ease.InOutBounce);
            circle.DOLocalMoveX(50f, 0.2f).SetEase(Ease.InOutBounce);
            onText.DOScale(1, 0.2f).SetEase(Ease.OutBounce);
            OffText.DOScale(0, 0.2f).SetEase(Ease.InBounce);
        }
        else
        {
            img.DOFillAmount(0, 0.2f).SetEase(Ease.InOutBounce);
            circle.DOLocalMoveX(-50f, 0.2f).SetEase(Ease.InOutBounce);
            onText.DOScale(0, 0.2f).SetEase(Ease.InBounce);
            OffText.DOScale(1, 0.2f).SetEase(Ease.OutBounce);
        }
    }

    #region Sound Setting
    void SetSoundSetting()
    {
        if (settingData.bIsSoundOn)
        {
            OnButtonClick(soundImg, soundCircleImg, soundOnText, soundOffText, true);
            soundAudioSource.Play();
        }
        else
        {
            OnButtonClick(soundImg, soundCircleImg, soundOnText, soundOffText, false);
            soundAudioSource.Stop();
        }
    }

    void OnSoundButtunClick()
    {
        if (settingData.bIsSoundOn)
        {
            settingData.bIsSoundOn = false;
            SetSoundSetting();
        }
        else
        {
            settingData.bIsSoundOn = true;
            SetSoundSetting();
        }
    }
    #endregion

    #region Music Setting

    void SetMusicSetting()
    {
        if (settingData.bIsMusicOn)
        {
            OnButtonClick(musicImg, musicCircleImg, musicOnText, musicOffText, true);
            Play(GetRandomClip());
        }
        else
        {
            OnButtonClick(musicImg, musicCircleImg, musicOnText, musicOffText, false);
            musicAudioSource.Stop();
        }
    }

    void OnMusicButtunClick()
    {
        if (settingData.bIsMusicOn)
        {
            settingData.bIsMusicOn = false;
            SetMusicSetting();
        }
        else
        {
            settingData.bIsMusicOn = true;
            SetMusicSetting();
        }
    }

    #endregion

    #region Data Functions
    public void SaveData()
    {

        string JsonData = JsonUtility.ToJson(settingData);
        PlayerPrefs.SetString("Setting", JsonData);

    }
    public void LoadData()
    {
        string JsonData = PlayerPrefs.GetString("Setting");
        settingData = JsonUtility.FromJson<SettingData>(JsonData);
        SetData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
    #endregion

}


