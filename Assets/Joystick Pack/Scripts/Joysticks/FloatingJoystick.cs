﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        if (GameManager.Instance.playerController.bhasSit)
        {
            GameManager.Instance.playerController.bhasSit = false;

            GameManager.Instance.playerController.enabled = true;
        }
        if (GameManager.Instance.playerController.bIsDiagnosing)
        {

            GameManager.Instance.playerController.bIsDiagnosing = false;
            GameManager.Instance.playerController.enabled = true;
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}