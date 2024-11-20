using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public event Action OnHoldOff;
    internal bool bIsOnHold;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        bIsOnHold = true;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);

        GameManager.Instance.playerController.enabled = true;
        // if (!bIsOnSeat)
        //  {
        base.OnPointerDown(eventData);
        // }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        bIsOnHold = false;
        OnHoldOff.Invoke();
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}