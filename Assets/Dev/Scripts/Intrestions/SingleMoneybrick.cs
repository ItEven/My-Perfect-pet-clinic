using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingleMoneybrick : MonoBehaviour
{
    public float moneyValue;
    public float jumpTime;
    public float jumpHight;


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {

    //        playerController = other.gameObject.GetComponent<PlayerController>();
    //        StartJump();
    //    }
    //}

    float _jump;

    void Awake()
    {

        //_jump = Random.Range(4.1f, 5.1f);
    }

    public void StartJump(Transform target)
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        Vector3 targetPosition = target.position;
        Quaternion initialRotation = transform.rotation; // Save the initial rotation

        transform.DOJump(targetPosition, jumpHight, 1, jumpTime).OnUpdate(() =>
        {
            targetPosition = target.position;
            transform.position = transform.position; // Update the position without rotation
            transform.rotation = initialRotation;    // Lock to the initial rotation
        }).OnComplete(() =>
        {
            transform.SetParent(target.transform);
            DOTween.Kill(this);
            Destroy(gameObject);
        });
    }

}
