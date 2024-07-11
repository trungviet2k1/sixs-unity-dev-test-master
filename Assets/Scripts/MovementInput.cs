using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    public float Velocity = 5f;
    public float desiredRotationSpeed = 0.1f;
    public float allowPlayerRotation = 0.1f;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float StartAnimatorTime = 0.3f;
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

    void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(inputX, 0, inputZ).normalized;

        float speed = direction.sqrMagnitude;

        if (speed > allowPlayerRotation)
        {
            animator.SetFloat("Speed", speed, StartAnimatorTime, Time.deltaTime);
            MoveAndRotate(direction);
        }
        else
        {
            animator.SetFloat("Speed", speed, StopAnimatorTime, Time.deltaTime);
        }
    }

    void MoveAndRotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;

            Vector3 moveDirection = forward * direction.z + right * direction.x;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), desiredRotationSpeed);
            controller.Move(Time.deltaTime * Velocity * moveDirection);
        }
    }
}