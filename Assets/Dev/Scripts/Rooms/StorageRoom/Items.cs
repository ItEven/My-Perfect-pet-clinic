using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Items : MonoBehaviour
{
    public ItemsTyps Typs;
    public float jumpTime;
    public float jumpHight;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        SetVisual();
    }

    public void SetVisual()
    {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.05f);
        });
    }

    public void StartJumpToStatic(Transform target)
    {


        Vector3 targetPosition = target.position;
        Quaternion initialRotation = transform.rotation;


        transform.DOJump(targetPosition, jumpHight, 1, jumpTime).OnComplete(() =>
        {
            targetPosition = target.position;
            transform.position = transform.position;
            transform.rotation = initialRotation;

        }).OnComplete(() =>
        {
            transform.SetParent(target.transform);
            DOTween.Kill(this);
        });
    }
    public void StartJumpToMoving(Transform target)
    {
        
        transform.parent = target;
        StartCoroutine(Jumping());
        //Vector3 targetPosition = perent.position;
        //Quaternion initialRotation = transform.rotation;


        //transform.DOJump(targetPosition, jumpHight, 1, jumpTime).OnComplete(() =>
        //{
        //    targetPosition = perent.position;
        //    transform.position = transform.position;
        //    transform.rotation = initialRotation;

        //}).OnComplete(() =>
        //{
        //    transform.SetParent(perent.transform);
        //    DOTween.Kill(this);
        //});
    }
    IEnumerator Jumping()
    {
        Vector3 jumpOffset = Vector3.zero;
    
        yield return new WaitForEndOfFrame();

        
        float _jump = Random.Range(3.1f, 5.1f);
        transform.DOLocalJump(Vector3.zero, _jump, 1, 1).OnComplete(() =>
        {          
            DOTween.Kill(this); 
        });
    }


}
