using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Unity.AI.Navigation;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    
    public Vector3 playerPos;

    [Space(1)]
    [Header("Gloable Veriables")]
    public GameObject singleMoneybrick;
    public NavMeshSurface navMeshSurface;

    [Space(1)]
    [Header("Drop objs Veriables")]
    public float dropHeight = 500.0f;
    public float dropDuration = 1.0f;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        SetPlayerPos();
    }
    public void SetPlayerPos()
    {
        playerController.transform.position = playerPos;
    }

    public void ReBuildNavmesh()
    {
        //if (navMeshSurface != null)
        //{
        //    navMeshSurface.BuildNavMesh();
        //}
        //else
        //{
        //    Debug.LogError("NavMeshSurface is not assigned.");
        //}
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


    #endregion



}
