using DG.Tweening.Core.Easing;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnTrigger : MonoBehaviour
{
    private bool bIsStay = false;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;
    public SpriteRenderer filler;
    public Seat seat;

    [Header("Arrow")]
    public bool bCanArrowWork;
    public bool bCanUsePLayerArrow;
    public Transform arrow;

    private void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEnter.Invoke();
            filler.gameObject.SetActive(true);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onTriggerStay.Invoke();
            filler.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerExit.Invoke();
            filler.gameObject.SetActive(false);
        }
    }


    public void SetupPlayerArrow()
    {
        Arrow();
        ShowArrow();
        if (bCanUsePLayerArrow)
        {
            GameManager.Instance.playerController.arrowController.gameObject.SetActive(true);
            GameManager.Instance.playerController.arrowController.SetTarget(transform, 2f);
        }
    }
    public void ShowArrow()
    {
        if (bCanArrowWork)
        {
            arrow.gameObject.SetActive(true);
        }
    }

    public void HideArrow()
    {
        Debug.Log("YOYO");
        arrow.gameObject.SetActive(false);
    }
    public void Arrow()
    {
        arrow.DOMoveY(2.0f, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }
}
