using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Unity.AI.Navigation;
using Unity.Collections;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;

    public Vector3 playerPosOnLock;
    public Vector3 playerPosOnUnlock;

    [Space(1)]
    [Header("Gloable Veriables")]
    public HallManager hall_01;
    public GameObject singleMoneybrick;
    public float profitMultiplier;


    [Space(1)]
    [Header("Drop objs Veriables")]
    public float dropHeight = 500.0f;
    public float dropDuration = 1.0f;

    void Awake()
    {
        Instance = this;
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;

    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        SetPlayerPos();
    }
    public void SetPlayerPos()
    {
        if (hall_01.bIsUnlock)
        {
            playerController.playerControllerData.characterMovement.rotatingObj.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            playerController.transform.position = playerPosOnUnlock;
        }
        else
        {
            playerController.transform.position = playerPosOnLock;
        }
    }




    #region  Visual Drop Effects

    public void DropObj(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);


            var Scale = obj.transform.localScale;
            Vector3 startPosition = obj.transform.position;
            startPosition.y += dropHeight;
            obj.transform.position = startPosition;
            obj.transform.DOMoveY(obj.transform.position.y - dropHeight, dropDuration)
                           .OnComplete(() =>
                           {
                               obj.transform.localScale = new Vector3(.7f, .7f, .7f);
                               obj.transform.DOScale(Scale, .5f).SetEase(Ease.OutElastic);

                               if (obj.transform.childCount > 0)
                               {
                                   obj.transform.GetChild(0).gameObject.SetActive(true);
                               }
                           });
        }

    }


    public void SetObjectsStates(GameObject[] objects, bool state)
    {
        foreach (var obj in objects)
        {
            if (obj.activeInHierarchy != state)
                obj.SetActive(state);
        }
    }

    public void PlayParticles(ParticleSystem[] particles)
    {
        foreach (var particle in particles)
        {
            particle.Play();
        }
    }
    public void SetObjectsState(GameObject objects, bool state)
    {

        if (objects.activeInHierarchy != state)
            objects.SetActive(state);

    }

    public void PlayParticles(ParticleSystem particles)
    {
        particles.Play();
    }


    #endregion



}
