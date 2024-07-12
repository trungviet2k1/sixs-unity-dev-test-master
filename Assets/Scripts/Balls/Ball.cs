using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float rangeRadius;

    [Header("UI Canvas")]
    [SerializeField] GameObject kickButton;
    [SerializeField] GameObject autoKickButton;

    [Header("Kick Force")]
    [SerializeField] float kickForce;
    [SerializeField] float autoKickCoefficient;

    [Header("Goals")]
    [SerializeField] Transform[] goals;

    [Header("Setup")]
    [SerializeField] float cameraTransitionDelay;
    [SerializeField] GameObject confettiParticleSystem;

    [Header("Cooldown")]
    [SerializeField] Image coolDownIcon;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] float autoKickCooldownTime = 10f;
    
    private Rigidbody ballRigidbody;
    private Transform playerTransform;
    private float autoKickCooldown = 0f;
    private bool canAutoKick = true;
    private bool isPlayerInRange = false;
    private bool isInGoal = false;

    private void Start()
    {
        coolDownIcon.fillAmount = 0;
        kickButton.SetActive(false);
        autoKickButton.SetActive(true);
        ballRigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        kickButton.GetComponent<Button>().onClick.AddListener(OnKickButtonPressed);
        autoKickButton.GetComponent<Button>().onClick.AddListener(OnAutoKickButtonPressed);

        buttonText = GameObject.Find("ButtonText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= rangeRadius && !isPlayerInRange)
        {
            isPlayerInRange = true;
            kickButton.SetActive(true);
        }
        else if (distance > rangeRadius && isPlayerInRange)
        {
            isPlayerInRange = false;
            kickButton.SetActive(false);
        }

        if (autoKickCooldown > 0)
        {
            autoKickCooldown -= Time.deltaTime;
            coolDownIcon.fillAmount = autoKickCooldown / autoKickCooldownTime;
            autoKickButton.GetComponent<Button>().interactable = false;
            buttonText.text = "Auto Kick \n" + autoKickCooldown.ToString("F1") + "s";
        }
        else
        {
            autoKickCooldown = 0f;
            canAutoKick = true;
            autoKickButton.GetComponent<Button>().interactable = true;
            buttonText.text = "Auto Kick";
        }
    }

    public void OnKickButtonPressed()
    {
        if (playerTransform == null || !isPlayerInRange || isInGoal) return;

        Transform nearestGoal = GetNearestGoal();
        if (nearestGoal == null) return;

        AudioManager.Instance.PlayKickBallSound();
        Vector3 direction = (nearestGoal.position - transform.position).normalized;
        ballRigidbody.AddForce(kickForce * direction, ForceMode.Impulse);
        Camera.main.GetComponent<CameraFollow>().SwitchTargetTo(transform, cameraTransitionDelay);
    }

    public void OnAutoKickButtonPressed()
    {
        if (playerTransform == null || !canAutoKick) return;

        Ball farthestBallScript = GetFarthestBall();
        if (farthestBallScript == null || farthestBallScript.isInGoal) return;

        Transform nearestGoal = farthestBallScript.GetNearestGoal();
        if (nearestGoal == null) return;

        AudioManager.Instance.PlayKickBallSound();
        Vector3 direction = (nearestGoal.position - farthestBallScript.transform.position).normalized;
        farthestBallScript.ballRigidbody.AddForce(autoKickCoefficient * kickForce * direction, ForceMode.Impulse);
        Camera.main.GetComponent<CameraFollow>().SwitchTargetTo(farthestBallScript.transform, cameraTransitionDelay);

        autoKickCooldown = autoKickCooldownTime;
    }

    private Transform GetNearestGoal()
    {
        Transform nearestGoal = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform goal in goals)
        {
            float distance = Vector3.Distance(transform.position, goal.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestGoal = goal;
            }
        }

        return nearestGoal;
    }

    private Ball GetFarthestBall()
    {
        Ball[] allBalls = FindObjectsOfType<Ball>();
        Ball farthestBall = null;
        float longestDistance = 0f;

        foreach (Ball ball in allBalls)
        {
            if (ball.isInGoal) continue;

            float distance = Vector3.Distance(playerTransform.position, ball.transform.position);
            if (distance > longestDistance)
            {
                longestDistance = distance;
                farthestBall = ball;
            }
        }

        return farthestBall;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (Transform goal in goals)
        {
            if (collision.gameObject.CompareTag(goal.tag))
            {
                AudioManager.Instance.PlayScoringGoalSound();
                Instantiate(confettiParticleSystem, collision.contacts[0].point, Quaternion.identity).DestroyAfter(3f);
                isInGoal = true;
                kickButton.SetActive(false);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }
}

public static class GameObjectExtensions
{
    public static void DestroyAfter(this GameObject obj, float seconds)
    {
        Object.Destroy(obj, seconds);
    }
}