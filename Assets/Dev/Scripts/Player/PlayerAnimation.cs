using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator _PlayerAnimator;
    [SerializeField] PlayerController playerController;

    private void Start()
    {
        _PlayerAnimator = GetComponent<Animator>();

    }
    private void Update()
    {
        if (_PlayerAnimator != null)
        {
            _PlayerAnimator.SetBool("bIsWalking", playerController.IsMoving());
            if (playerController.IsMoving())
            {
                HandleMovementAnimation();
            }
        }
    }

    public void HandleMovementAnimation()
    {
        _PlayerAnimator.SetFloat("Velocity", playerController.GetVelocity());
    }
}
