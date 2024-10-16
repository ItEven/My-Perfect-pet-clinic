using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SingleMoneybrick : MonoBehaviour
{
    public float moneyValue;
    public float jumpTime;
    public float jumpHight;
    public float fadeOutTime;
    public float fadeInTime;
    public Renderer renderer;

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
        Quaternion initialRotation = transform.rotation;

        Material material = renderer.material;
        material.color = new Color(material.color.r, material.color.g, material.color.b, 0);

        material.DOFade(1, fadeInTime)
            .OnComplete(() =>
            {
                material.DOFade(0, fadeOutTime);
            });



        transform.DOJump(targetPosition, jumpHight, 1, jumpTime).OnComplete(() =>
        {
            material.DOFade(0, fadeOutTime);
            targetPosition = target.position;
            transform.position = transform.position;
            transform.rotation = initialRotation;
        }).OnComplete(() =>
        {
            transform.SetParent(target.transform);

            DOTween.Kill(this);
            Destroy(gameObject);

        });
    }

    public void StartRandomJump(Transform jumpPos, Transform target)
    {
        transform.SetParent(target.transform);
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        Vector3 targetPosition = target.position;
        Quaternion initialRotation = transform.rotation;

        float randomJumpHeight = Random.Range(jumpHight - 1f, jumpHight + 1f); 
        float randomSideSpread = Random.Range(-2f, 2f);
        Vector3 randomSideOffset = new Vector3(randomSideSpread, 0, randomSideSpread);

        transform.DORotate(new Vector3(360, 360, 360), 1f, RotateMode.FastBeyond360)
    .SetLoops(-1, LoopType.Yoyo);

        transform.DOMove(jumpPos.position + randomSideOffset, jumpTime).OnComplete(() =>
        {

            transform.DOMove(targetPosition + randomSideOffset, jumpTime).OnComplete(() =>
            {
                
                DOTween.Kill(this);
                Destroy(gameObject);

            });

        }).SetEase(Ease.OutBounce);
        
    }
}

