using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class RoomUiContentBox : MonoBehaviour
{
    public Image icon;
    public Text titleText;
    public Text text;
    public Button btn;
    public Transform targetPos;
    CameraController controller;

    [Header("Illnesses Sprites")]
    public Sprite[] sprites;
    private void Start()
    {
        //btn.interactable = false;
        controller = SaveManager.instance.cameraController;
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(MoveToTarget);
        }
    }

    public void MoveToTarget()
    {
        if (targetPos != null)
        {
            UiManager.instance.OpneRoomPanel();
            controller.MoveToTarget(targetPos, () =>
            {
                DOVirtual.DelayedCall(2f, () =>
                {
                    controller.MoveToPlayer();
                });
            }, 0f);
        }
    }
    public void SetRoomData(Sprite sprite, string dataTitleText, string dataText, Transform DataTransform)
    {
        icon.sprite = sprite;
        titleText.text = dataTitleText;
        text.text = dataText;
        targetPos = DataTransform;
        btn.interactable = true;
    }

    public void SetIllnessesData(string dataTitleText)
    {   
        int Index = Random.Range(0, sprites.Length);
        icon.sprite = sprites[Index];   
        titleText.text = dataTitleText;
    }
}
