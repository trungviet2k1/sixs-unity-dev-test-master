using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    public float velocity = 5f;
    public float desiredRotationSpeed = 0.3f;
    public float allowPlayerRotation = 0.1f;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float StartAnimatorTime = 0.1f;
    [Range(0, 1f)]
    public float StopAnimatorTime = 0.15f;

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
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        float speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (speed > allowPlayerRotation)
        {
            animator.SetFloat("Speed", speed, StartAnimatorTime, Time.deltaTime);
            PlayerMoveAndRotation(new Vector3(InputX, 0f, InputZ));
        }
        else
        {
            animator.SetFloat("Speed", speed, StopAnimatorTime, Time.deltaTime);
        }
    }
}