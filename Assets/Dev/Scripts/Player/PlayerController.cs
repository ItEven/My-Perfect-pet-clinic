using EasyCharacterMovement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Transform moneyCollectPoint;
    public float rotationRate = 540.0f;

    public float maxSpeed = 5;

    public float acceleration = 20.0f;
    public float deceleration = 20.0f;

    public float groundFriction = 8.0f;
    public float airFriction = 0.5f;

    public float jumpImpulse = 6.5f;

    [Range(0.0f, 1.0f)]
    public float airControl = 0.3f;

    public Vector3 gravity = Vector3.down * 9.81f;

    private CharacterMovement _characterMovement;

    private Vector3 _movementDirection;

    private Vector3 desiredVelocity;

    public bool isDragging;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        HendelMovement();
    }

    #region Movement
    public void HendelMovement()
    {

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Check if the player is dragging
        isDragging = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        // Create a movement direction vector (in world space)
        _movementDirection = Vector3.zero;
        _movementDirection += Vector3.forward * horizontal;
        _movementDirection -= Vector3.right * vertical;

        // Make Sure it won't move faster diagonally
        _movementDirection = Vector3.ClampMagnitude(_movementDirection, 1.0f);

        // Rotate towards movement direction
        _characterMovement.RotateTowards(_movementDirection, rotationRate * Time.deltaTime);

        // Perform movement
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

    #region Intreaction 



    #endregion

}
