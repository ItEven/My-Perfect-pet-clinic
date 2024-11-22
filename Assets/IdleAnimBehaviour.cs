using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class IdleAnimBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _minTimeBored;
    [SerializeField]
    private float _maxTimeBored;
    private float _timeUntilBored;

    [SerializeField]
    private int _numberOfBoredAnimations;

    private bool _isBored;
    private float _idleTime ;
    private int _boredAnimation;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timeUntilBored = Random.Range(_minTimeBored, _maxTimeBored);
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isBored == false)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilBored && stateInfo.normalizedTime % 1 < 0.04f)
            {
                _isBored = true;
                _boredAnimation = Random.Range(0, _numberOfBoredAnimations);


                animator.SetFloat("BoredAnimation", _boredAnimation);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("BoredAnimation", _boredAnimation, 0.4f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (_isBored)
        {
            _boredAnimation--;
        }

        _isBored = false;
        _idleTime = 0;
    }


}
