using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUiContentBox : MonoBehaviour
{
    public Image icon;
    public Text titleText;
    public Text text;
    public Button btn;
    public Transform targetPos;
    CameraController controller;
    private void Start()
    {
        //btn.interactable = false;
        controller = SaveManager.instance.cameraController;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(MoveToTarget);
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
}
