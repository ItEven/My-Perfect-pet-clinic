using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCarryhandler : MonoBehaviour
{
    [Header("Items Details")]
    public int currntCount;
    public int maxItemCarryCapicty;
    public float itemTakingDilay = 0.2f;
    public Transform[] itemsPostionArr;
    public Items[] itemsArr;
    Rek Rek;
    public void TakeItem(Rek rek)
    {
        Rek = rek;
        if (currntCount < maxItemCarryCapicty)
        {
            Debug.LogError("yo wapp");
            GameManager.Instance.playerController.animationBool.bHasCarringItem = true;
            StartCoroutine(TakingItems());
        }
    }
    Coroutine coroutine;

    public void StartCoroutine()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(TakingItems());
        }
    }
    IEnumerator TakingItems()
    {
        for (int i = 0; i < maxItemCarryCapicty; i++)
        {
            if (currntCount < maxItemCarryCapicty)
            {

                currntCount++;
                int index = Rek.GetItemIndex();
                var item = Rek.itemsArr[index];

                if (item != null)
                {
                    item.StartJumpToMoving(itemsPostionArr[i]);
                    itemsArr[i] = item;
                    Rek.itemsArr[index] = null;
                }

                if (currntCount >= maxItemCarryCapicty)
                {
                    Rek.CheckIfRefillNeed();
                    StopCoroutine();
                    break;
                }
            }
            else
            {
                StopCoroutine();
                break;
            }
            yield return new WaitForSeconds(itemTakingDilay);
        }

        // Reset the coroutine reference on completion
        coroutine = null;

    }

    public void StopCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(TakingItems());
        }
    }




}
