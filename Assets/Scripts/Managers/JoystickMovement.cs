using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JoystickMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 5f;
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
        if (Time.timeScale > 0)
        {
            HandleMovement();
        }
    }

    void PlayerMoveAndRotation(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), desiredRotationSpeed);
        }
        controller.Move(Time.deltaTime * speed * moveDirection);
    }

    void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float magnitude = direction.magnitude;

        animator.SetFloat("Speed", magnitude);

        if (magnitude > 0f)
        {
            PlayerMoveAndRotation(direction);
        }
    }
}