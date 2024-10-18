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
    public Transform staffSheat;
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
            npcMovement.MoveToTarget(staffSheat, () =>
            {
                Debug.LogError("Anims Playe");
                //npcMovement.animator.PlayAnimation(AnimType.sit);
            });
        }
    }




    public override void OnUnlockAndUpgrade()
    {
       
        base.OnUnlockAndUpgrade();
        npcMovement.MoveToTarget(staffSheat, () =>
        {
            Debug.LogError("Anims Playe");
            //npcMovement.animator.PlayAnimation(AnimType.sit);
        });
    }

    public void OnReachSheat()
    {

    }

}
