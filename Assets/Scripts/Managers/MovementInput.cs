using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] Joystick joystick;
    [SerializeField] float speed;
    [SerializeField] float desiredRotationSpeed;

    private Animator animator;
    private CharacterController controller;
    private bool useJoystick;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // Quyết định dùng joystick hay WASD tại thời điểm build / compile
#if UNITY_ANDROID
        useJoystick = true;
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE
        useJoystick = false;
#elif UNITY_EDITOR
        // Trong Editor: dùng Application.isMobilePlatform để mô phỏng, hoặc chỉnh tay nếu cần
        useJoystick = Application.isMobilePlatform;
#else
        useJoystick = Application.isMobilePlatform;
#endif

        if (joystick != null)
            joystick.gameObject.SetActive(useJoystick);
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
        float horizontal = 0f;
        float vertical = 0f;

        // Nếu build cho Android (hoặc đang mô phỏng mobile trong Editor) => lấy từ joystick.
        // Ngược lại => dùng WASD / Arrow keys (Input.GetAxis).
        if (useJoystick && joystick != null && joystick.gameObject.activeInHierarchy)
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float magnitude = direction.magnitude;

        animator.SetFloat("Speed", magnitude);

        if (magnitude > 0f)
        {
            PlayerMoveAndRotation(direction);
        }
    }
}