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
          
            if (playerController.IsMoving())
            {
                _PlayerAnimator.Play(AnimType.Walk.ToString());
                HandleMovementAnimation();
            }
            else
            {
                _PlayerAnimator.Play(AnimType.Idle.ToString());

            }
        }
    }

    public void HandleMovementAnimation()
    {    

        _PlayerAnimator.SetFloat("Velocity", playerController.GetVelocity());
    }
}
