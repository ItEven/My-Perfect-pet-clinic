using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;
    public SpriteRenderer filler;
    private bool bIsStay = false;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTrigger.Invoke();
            filler.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            filler.gameObject.SetActive(false);
        }
    }



}
