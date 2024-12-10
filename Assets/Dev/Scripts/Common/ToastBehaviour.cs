using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class ToastBehaviour : MonoBehaviour
{
    public TextMeshPro toastText;
    public void ONToastCreate(string text)
    {
        toastText.text = text;

        Vector3 pos = transform.position;
        Vector3 newPos = new Vector3(pos.x, pos.y + 50f, pos.z);

        transform.DOMoveY(newPos.y, 2f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
