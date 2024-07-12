using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("Range")]
    public float rangeRadius = 5f;

    [Header("UI Canvas")]
    public GameObject kickButton;
    public GameObject autoKickButton;

    [Header("Kick Force")]
    public float kickForce;
    public float autoKickCoefficient;

    [Header("Goals")]
    public Transform[] goals;

    [Header("Setup")]
    public float cameraTransitionDelay = 2f;
    public GameObject confettiParticleSystem;

    private Rigidbody ballRigidbody;
    private Transform playerTransform;
    private bool isPlayerInRange = false;
    private bool isInGoal = false;

    private void Start()
    {
        kickButton.SetActive(false);
        autoKickButton.SetActive(true);
        ballRigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        kickButton.GetComponent<Button>().onClick.AddListener(OnKickButtonPressed);
        autoKickButton.GetComponent<Button>().onClick.AddListener(OnAutoKickButtonPressed);
    }

    private void Update()
    {
        if (playerTransform == null || isInGoal) return;

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
        if (playerTransform == null) return;

        Ball farthestBallScript = GetFarthestBall();
        if (farthestBallScript == null || farthestBallScript.isInGoal) return;

        Transform nearestGoal = farthestBallScript.GetNearestGoal();
        if (nearestGoal == null) return;

        AudioManager.Instance.PlayKickBallSound();
        Vector3 direction = (nearestGoal.position - farthestBallScript.transform.position).normalized;
        farthestBallScript.ballRigidbody.AddForce(autoKickCoefficient * kickForce * direction, ForceMode.Impulse);
        Camera.main.GetComponent<CameraFollow>().SwitchTargetTo(farthestBallScript.transform, cameraTransitionDelay);
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