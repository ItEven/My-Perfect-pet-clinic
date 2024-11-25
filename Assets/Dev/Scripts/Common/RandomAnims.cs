using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnims : StateMachineBehaviour
{
    [SerializeField]
    private int _numberOfAnimations;
    private int _randomAnimation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _randomAnimation = Random.Range(0, _numberOfAnimations);
        animator.SetFloat("RandomAnimation", _randomAnimation);
    }
}
