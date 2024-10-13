using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    public Vector3 playerPos;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetPlayerPos();
    }
    public void SetPlayerPos()
    {
        playerController.transform.position = playerPos;
    }

    #region  Visual Drop Effects

    public void DropObj(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
            float dropHeight = 500.0f;
            float dropDuration = 1.0f;

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
