using DG.Tweening;
//using MoreMountains.NiceVibrations;
using Lofelt.NiceVibrations;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    [SerializeField] internal AudioSource AudioSource;


    // public GameAudio BtnPress;
    public AudioClip upgradeClip;
    public AudioClip whoohClip;
    public AudioClip moneyCollectClip;
    public AudioClip moneyDropClip;


    [Header("Settings Panel")]
    [SerializeField] RectTransform SettingsPanel;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button SoundBtn;
    [SerializeField] Button MusicBtn;
    [SerializeField] Button vibrateBtn;
    [SerializeField] Button CloseBTn;

    [Header("setting sprites")]



    public Sprite setting;
    public Sprite _close;

    public int Sound;
    public int Music;
    public int Vibrate;


    public static string MUSIC = "Gr_Music";
    public static string VIBRATE = "Gr_Virate";
    public static string SOUND = "Gr_Sound";

    [Header("uiCHANGES")]
    public Sprite OnImage;
    public Sprite offImage;



    [Header("Sound")]
    public Sprite soundon;
    public Sprite soundoff;
    public Image Soundimg;
    public Image SoundBtnImg;
    public Text SoundText;


    [Header("Music")]
    public Sprite musicon;
    public Sprite musicoff;
    public Image Musicimg;
    public Image MusicBtnImg;
    public Text musictext;


    [Header("Haptic")]
    public Sprite hapticon;
    public Sprite hapticoff;
    public Image Hapticimg;
    public Image HapticBtnImg;
    public Text haptictext;



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
    // Start is called before the first frame update
    void Start()
    {  
        //BtnPress.Play();
       // SetData();
        Check();
        //MMVibrationManager.SetHapticsActive(true);
        //settingsBtn.onClick.AddListener(() => { settingsPan(); BtnPress.Play(); haptic(HapticPatterns.PresetType.LightImpact); });
        //SoundBtn.onClick.AddListener(() => { BgSoundSeting(); BtnPress.Play(); haptic(HapticPatterns.PresetType.LightImpact); });
        //MusicBtn.onClick.AddListener(() => { Musicseting(); BtnPress.Play(); haptic(HapticPatterns.PresetType.LightImpact); });
        //vibrateBtn.onClick.AddListener(() => { hapticfx(); BtnPress.Play(); });
        //CloseBTn.onClick.AddListener(() => { settingsPan(); BtnPress.Play(); haptic(HapticPatterns.PresetType.LightImpact); });
    }

    public void Play(AudioClip clip)
    {
        // if (Sound != 1) return;
        
        AudioSource.PlayOneShot(clip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    public void OnUpgrade()
    {
        AudioSource.PlayOneShot(upgradeClip);
        //AudioSource.PlayOneShot(whoohClip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    public void OnMonenyCollect()
    {
        AudioSource.PlayOneShot(moneyCollectClip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    } 
    public void OnMoneyDrop()
    {
        AudioSource.PlayOneShot(moneyDropClip);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }



    void SetData()
    {
        Sound = PlayerPrefs.GetInt(SOUND, 1);
        Music = PlayerPrefs.GetInt(MUSIC, 1);
        Vibrate = PlayerPrefs.GetInt(VIBRATE, 1);

    }
    internal void haptic(HapticPatterns.PresetType haptic)
    {
        if (Vibrate != 1) return;
        //MMVibrationManager.Haptic(haptic);
        HapticPatterns.PlayPreset(haptic);
    }

    void settingsPan()
    {
       
        if (SettingsPanel.gameObject.activeInHierarchy)
        {
            SettingsPanel.DOAnchorPosX(1500, .25f).OnComplete(() => { SettingsPanel.gameObject.SetActive(false); SettingsPanel.DOAnchorPosX(-1500, 0); });


            //settingsBtn.GetComponent<Image>().sprite = setting;

        }
        else
        {
            SettingsPanel.gameObject.SetActive(true);
            SettingsPanel.DOAnchorPosX(0, .25f);

            //settingsBtn.GetComponent<Image>().sprite = _close;
        }
    }
    void Musicseting()
    {

        if (Music == 0)
        {
            PlayerPrefs.SetInt(MUSIC, 1);
            AudioSource.Play();

            MusicBtnImg.sprite = OnImage;
            Musicimg.sprite = musicon;
            ////  musictext.text = "ON";
            //  musictext.alignment = TextAnchor.MiddleLeft;

        }
        else
        {
            PlayerPrefs.SetInt(MUSIC, 0);
            AudioSource.Stop();


            MusicBtnImg.sprite = offImage;
            Musicimg.sprite = musicoff;
            //musictext.alignment = TextAnchor.MiddleRight;
            //musictext.text = "OFF";


        }
        SetData();
    }

    void hapticfx()
    {
        if (Vibrate == 1)
        {
            PlayerPrefs.SetInt(VIBRATE, 0);
            //MMVibrationManager.SetHapticsActive(false);
            HapticController.hapticsEnabled = false;
            Hapticimg.sprite = hapticoff;
            HapticBtnImg.sprite = offImage;
        }
        else
        {
            PlayerPrefs.SetInt(VIBRATE, 1);
            //MMVibrationManager.SetHapticsActive(true);
            HapticController.hapticsEnabled = true;
            Hapticimg.sprite = hapticon;
            HapticBtnImg.sprite = OnImage;
            haptic(HapticPatterns.PresetType.LightImpact);

        }
        SetData();
    }


    void BgSoundSeting()
    {
        if (Sound == 0)
        {
            PlayerPrefs.SetInt(SOUND, 1);
            Sound = PlayerPrefs.GetInt(SOUND);
            Soundimg.sprite = soundon;
            SoundBtnImg.sprite = OnImage;
        }
        else
        {
            PlayerPrefs.SetInt(SOUND, 0);
            Sound = PlayerPrefs.GetInt(SOUND);
            Soundimg.sprite = soundoff;
            SoundBtnImg.sprite = offImage;
        }
        SetData();

    }

    void Check()
    {
        if (Music == 0)
        {
           // AudioSource.Stop();
           // Musicimg.sprite = musicoff;
            //MusicBtnImg.sprite = offImage;
        }
        if (Sound == 0)
        {
           // Soundimg.sprite = soundoff;
           // SoundBtnImg.sprite = offImage;
        }
        if (Vibrate == 0)
        {
            //MMVibrationManager.SetHapticsActive(false);
           // HapticController.hapticsEnabled = false;
           // Hapticimg.sprite = hapticoff;
           // HapticBtnImg.sprite = offImage;
        }
    }


}


[System.Serializable]
public class GameAudio
{
    public AudioClip clip;
    bool isBeingUsed = false;

    public bool IsFree => !isBeingUsed;

    public void Play()
    {
        AudioManager.i.Play(clip);
        isBeingUsed = true;
    }

    public void Release()
    {
        isBeingUsed = false;
    }
}
