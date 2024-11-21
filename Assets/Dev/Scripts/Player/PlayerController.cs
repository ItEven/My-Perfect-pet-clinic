using EasyCharacterMovement;
using MoreMountains.Tools;
using UnityEngine;

[System.Serializable]
public class PlayerControllerData
{
    public FloatingJoystick joystick;
    public float rotationRate = 540.0f;
    public float maxSpeed = 5f;
    public float customAngle = 45f;
    public float sensitivity = 45f;

    public float acceleration = 20.0f;
    public float deceleration = 20.0f;

    public float groundFriction = 8.0f;
    public float airFriction = 0.5f;

    public float jumpImpulse = 6.5f;

    [Range(0.0f, 1.0f)]
    public float airControl = 0.3f;

    public Vector3 gravity = Vector3.down * 9.81f;

    internal CharacterMovement characterMovement;

    public bool isDragging;
    public bool canDrag;
}
[System.Serializable]
public class PlayerAnimationBools
{  
   
    public bool bHasCarringItem;
    public bool bHasInjection;
}

[System.Serializable]
public class Equipments
{
    public GameObject injection;
}

public class PlayerController : MonoBehaviour
{
    [Header("Player Data Refrence")]
    public PlayerControllerData playerControllerData;
    public AnimationController animationController;
    public ArrowController arrowController;
    public Transform moneyCollectPoint;

    [Header("Imp Refrence")]
    public ItemsCarryhandler itemsCarryhandler;

    [Header("Animation bool")]
    public PlayerAnimationBools animationBools;

    [Header("Equipments Objects")]
    public Equipments playerEquipments;

    public bool bHaveItems;

    private Vector3 movementDirection;
    private Vector3 desiredVelocity;

    private void Awake()
    {
        playerControllerData.characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    private float horizontal;
    private float vertical;

    #region Movement
    public void HandleMovement()
    {
        if (UiManager.bIsUiOn) return;
        horizontal = playerControllerData.joystick.Horizontal;
        vertical = playerControllerData.joystick.Vertical;

        playerControllerData.isDragging = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        movementDirection = Vector3.zero;
        movementDirection += Vector3.right * horizontal;
        movementDirection += Vector3.forward * vertical;

        movementDirection = Quaternion.AngleAxis(playerControllerData.customAngle, Vector3.up) * movementDirection;
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);

        playerControllerData.characterMovement.RotateTowards(movementDirection, playerControllerData.rotationRate * Time.deltaTime * 2f);

        desiredVelocity = movementDirection * playerControllerData.maxSpeed;

        float actualAcceleration = playerControllerData.characterMovement.isGrounded ? playerControllerData.acceleration : playerControllerData.acceleration * playerControllerData.airControl;
        float actualDeceleration = playerControllerData.characterMovement.isGrounded ? playerControllerData.deceleration : 0.0f;

        float actualFriction = playerControllerData.characterMovement.isGrounded ? playerControllerData.groundFriction : playerControllerData.airFriction;

        playerControllerData.characterMovement.SimpleMove(desiredVelocity, playerControllerData.maxSpeed, actualAcceleration, actualDeceleration, actualFriction, actualFriction, playerControllerData.gravity);
    }

    public bool IsMoving()
    {
        return desiredVelocity.magnitude > 0.1f;
    }

    private float velocity;
    public float GetVelocity()
    {
        velocity = new Vector2(playerControllerData.joystick.Horizontal, playerControllerData.joystick.Vertical).magnitude;
        return velocity = Mathf.Clamp01(velocity);
    }
    #endregion

    #region Animation
    internal bool hasSit;
    internal bool isDiagnosing;
    internal bool isTyping;

    public void HandleAnimation()
    {
        if (IsMoving())
        {
            if (animationBools.bHasCarringItem)
            {

                animationController.PlayAnimation(AnimType.Walk_With_Object);
                animationController.controller.SetFloat("Velocity", GetVelocity());

            }
            else
            {
                animationController.PlayAnimation(AnimType.Run);
                animationController.controller.SetFloat("Velocity", GetVelocity());
            }
        }
        else
        {
            //if (hasSit && !isTyping && !isDiagnosing)
            //{
            //    animationController.PlayAnimation(AnimType.Sti_Idle);
            //}
            //else if (hasSit && isTyping)
            //{
            //    animationController.PlayAnimation(AnimType.Typing);
            //}
            //else if (hasSit && isDiagnosing)
            //{
            //    animationController.PlayAnimation(AnimType.Diagnosing);
            //}
            if (animationBools.bHasCarringItem)
            {
                //animationController.PlayAnimation(AnimType.Idle_With_Object);
                animationController.PlayAnimation(AnimType.Idle);
            }
            else
            {
                animationController.PlayAnimation(AnimType.Idle);
            }
        }
    }
    #endregion

    #region Items Interaction
    public void SetItemState(ItemsTyps items, bool state)
    {
        switch (items)
        {
            case ItemsTyps.injection:                         
                playerEquipments.injection.SetActive(state);
                animationBools.bHasInjection = state;
                break;

        }
        bHaveItems = state;
    }
   
    #endregion
}
