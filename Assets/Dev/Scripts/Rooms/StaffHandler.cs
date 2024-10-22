using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StaffUpgradeProperties
{
    public int Level;
    public int upgradeCost;
    public int exprince;
    public StaffExprinceType type;
    public DiseaseType[] diseases;
    public UpgradeHandler[] nextUpgeades;
}
public class StaffHandler : UpgradeHandler
{
    // Public
    public int level;
    public bool bIsStaffAtSit;
    public Transform sitPos;
    public Transform standPos;
    public StaffUpgradeProperties[] staffUpgrades;

    //private
    internal StaffUpgradeProperties currentProperties;
    NPCMovement npcMovement;


    public override void Start()
    {
        npcMovement = GetComponent<NPCMovement>();
        base.Start();
    }
    public override void loadData()
    {  

        base.loadData();

        if (bIsUnlock)
        {
            transform.position = sitPos.position;
            transform.rotation = sitPos.rotation;
            npcMovement.StopNpc();
            npcMovement.animator.PlayAnimation(AnimType.Talking_01);
            bIsStaffAtSit = true;
        }

    }

    public override void SetTakeMoneyData()
    {
        if (bIsUnlock)
        {

            DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(currentProperties.upgradeCost));
        }
        else
        {
            DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(currentCost));
        }

    }

    public override void OnUnlockAndUpgrade()
    {
        if (bIsUnlock)
        {
            if (currentProperties != null)
            {
                foreach (var item in currentProperties.nextUpgeades)
                {
                    item.bIsUpgraderActive = true;
                    item.SetUpgredeVisual();
                }
            }

            bIsUpgraderActive = false;

            level++;
            roundUpgradePartical.ForEach(X => X.Play());

        }
        else
        {
            base.OnUnlockAndUpgrade();

            if (!bIsStaffAtSit)
            {
                npcMovement.Init();
                npcMovement.MoveToTarget(standPos, () =>
                {
                    bIsStaffAtSit = true;
                    transform.position = sitPos.position;
                    transform.rotation = sitPos.rotation;
                    npcMovement.animator.PlayAnimation(AnimType.Talking_01);
                });

            }


        }
        currentProperties = staffUpgrades[level];

    }



}
