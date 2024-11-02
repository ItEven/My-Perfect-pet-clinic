using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnTrigger : MonoBehaviour
{
    private bool bIsStay = false;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public SpriteRenderer filler;
    internal Collider Collider;

    private void Start()
    {
        Collider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEnter.Invoke();
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



}
