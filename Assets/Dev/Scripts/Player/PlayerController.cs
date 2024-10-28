using EasyCharacterMovement;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public AnimationController animationController;
    public FloatingJoystick joystick;
    public Transform moneyCollectPoint;
    public float rotationRate = 540.0f;
    public float maxSpeed = 5;
    public float customAngle = 45f;
    public float Senc = 45f;

    public float acceleration = 20.0f;
    public float deceleration = 20.0f;

    public float groundFriction = 8.0f;
    public float airFriction = 0.5f;

    public float jumpImpulse = 6.5f;
    
    [Range(0.0f, 1.0f)]
    public float airControl = 0.3f;

    public Vector3 gravity = Vector3.down * 9.81f;

    internal CharacterMovement _characterMovement;

    private Vector3 _movementDirection;

    private Vector3 desiredVelocity;

    public bool isDragging;
    public bool bCanDarg;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        HendelMovement();
        HendelAnimtion();
    }
    float horizontal;
    float vertical;
    #region Movement
    public void HendelMovement()
    {


     

        horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
     
        isDragging = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

       


        _movementDirection = Vector3.zero;
        _movementDirection += Vector3.right * horizontal;
        _movementDirection += Vector3.forward * vertical;

        _movementDirection = Quaternion.AngleAxis(customAngle, Vector3.up) * _movementDirection;


        _movementDirection = Vector3.ClampMagnitude(_movementDirection, 1.0f);


        _characterMovement.RotateTowards(_movementDirection, rotationRate * Time.deltaTime * 2f);


        desiredVelocity = _movementDirection * maxSpeed;

        float actualAcceleration = _characterMovement.isGrounded ? acceleration : acceleration * airControl;
        float actualDeceleration = _characterMovement.isGrounded ? deceleration : 0.0f;

        float actualFriction = _characterMovement.isGrounded ? groundFriction : airFriction;

        _characterMovement.SimpleMove(desiredVelocity, maxSpeed, actualAcceleration, actualDeceleration,
            actualFriction, actualFriction, gravity);

    }
    public bool IsMoving()
    {
        return desiredVelocity.magnitude > 0.1f;
    }
    float velocity;
    public float GetVelocity()
    {
        velocity = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;
        return velocity = Mathf.Clamp01(velocity);
    }
    #endregion


    #region HendelAnimtion
    internal bool bhasSit;
    internal bool bIsDiagnosing;
    internal bool bIsTyping;
    public void HendelAnimtion()
    {
        if (IsMoving())
        {
            animationController.PlayAnimation(AnimType.Walk);
            animationController.controller.SetFloat("Velocity", GetVelocity());
        }
        else
        {
            if (bhasSit && !bIsTyping && !bIsDiagnosing)
            {
                animationController.PlayAnimation(AnimType.Sti_Idle);
            }
            else if (bhasSit && bIsTyping)
            {
                animationController.PlayAnimation(AnimType.Typing);
            }
            else if (bhasSit && bIsDiagnosing)
            {
                animationController.PlayAnimation(AnimType.Diagnosing);
            }
            else
            {
                animationController.PlayAnimation(AnimType.Idle);
            }
        }
    }

    #endregion

    #region Intreaction 

    public void StopPlayer()
    {

        _movementDirection = Vector3.zero;
        desiredVelocity = Vector3.zero;
        _characterMovement.SimpleMove(Vector3.zero, 0, 0, 0, groundFriction, airFriction, gravity);
    }

    #endregion

}
