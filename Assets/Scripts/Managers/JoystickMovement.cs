using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JoystickMovement : MonoBehaviour
{
    public Joystick joystick;
    public float velocity = 5f;
    public float desiredRotationSpeed = 0.3f;

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    void PlayerMoveAndRotation(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), desiredRotationSpeed);
        }
        controller.Move(Time.deltaTime * velocity * moveDirection);
    }

    void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (horizontal != 0f || vertical != 0f)
        {
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            animator.SetFloat("Speed", direction.magnitude);
            PlayerMoveAndRotation(direction);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}