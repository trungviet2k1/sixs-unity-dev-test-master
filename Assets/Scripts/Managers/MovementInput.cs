using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    public float speed = 5f;
    public float desiredRotationSpeed = 0.3f;
    public float allowPlayerRotation = 0.1f;

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
        controller.Move(Time.deltaTime * speed * moveDirection);
    }

    void HandleMovement()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        float speed = new Vector2(InputX, InputZ).sqrMagnitude;

        animator.SetFloat("Speed", speed);

        if (speed > allowPlayerRotation)
        {
            PlayerMoveAndRotation(new Vector3(InputX, 0f, InputZ));
        }
    }
}