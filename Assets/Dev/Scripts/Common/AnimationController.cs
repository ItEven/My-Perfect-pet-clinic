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
        AnimatorStateInfo currentState = controller.GetCurrentAnimatorStateInfo(0);
        string animationName = currentState.ToString();
        return animationName;
    }



}
