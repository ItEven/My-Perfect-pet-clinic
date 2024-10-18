using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StaffUpgrade
{
    public int Level;
    public int upgradeCost;
    public int exprince;
    public bool bIsMovingStaff;
    public StaffExprinceType type;
    public DiseaseType[] diseases;
}
public class StaffHandler : UpgradeHandler
{
    // Public
    public bool bIsMovingStaff;
    public bool bIsStaffAtSit;
    public Transform sitPos;
    public Transform standPos;
    public StaffUpgrade[] staffUpgrades;

    //private
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


    public override void OnUnlockAndUpgrade()
    {

        base.OnUnlockAndUpgrade();

        if (!bIsStaffAtSit)
        {
            
            npcMovement.MoveToTarget(standPos, () =>
            {
                bIsStaffAtSit = true;
                npcMovement.animator.PlayAnimation(AnimType.Talking_01);
            });

        }
    }

    public void OnReachSheat()
    {

    }

}
