using System;
using System.Collections.Generic;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
    public Animator controller;
    public AnimType startanimation;


    void Start()
    {
        controller = GetComponent<Animator>();
        PlayAnimation(startanimation);
        controller.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

    public void PlayAnimation(AnimType anim)
    {
        if (controller != null)
        {
            controller.Play(anim.ToString());
        }
        else
        {


        }
    }

    public string GetCurrntAnimState()
    {
        if (controller != null)
        {
            // Get the current state info from the first layer of the Animator
            AnimatorStateInfo currentState = controller.GetCurrentAnimatorStateInfo(0);

            // Return the current animation state's name using its hash
            foreach (AnimType anim in Enum.GetValues(typeof(AnimType)))
            {
                if (Animator.StringToHash(anim.ToString()) == currentState.shortNameHash)
                {
                    return anim.ToString();
                }
            }

            // If no match found, return a default message
            return "Unknown State";
        }

        // If the controller is null, return an error message
        return "Animator Not Found";
    }



}
