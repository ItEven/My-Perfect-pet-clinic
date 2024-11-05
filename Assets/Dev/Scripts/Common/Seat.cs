using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool IsAvailable;
    public AnimType idleAnim;
    public AnimType workingAnim;
    private void OnEnable()
    {
        IsAvailable = true;
    }

    public void SetAlloted()
    {
        IsAvailable = false;
    }

    public void ClearSeat()
    {
        IsAvailable = true;
    }
}
