using System;
using System.Collections.Generic;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
    Animator controller;
    public AnimType startanimation;

    void Start()
    {
        controller = GetComponent<Animator>();
        controller.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

    public void PlayAnimation(AnimType anim)
    {
        controller.Play(anim.ToString());
    }

}
