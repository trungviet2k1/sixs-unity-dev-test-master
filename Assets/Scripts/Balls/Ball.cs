using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("Range")]
    public float rangeRadius = 5f;

    [Header("UI Canvas")]
    public GameObject kickButton;

    [Header("Kick Force")]
    public float kickForce = 10f;

    [Header("Goals")]
    public Transform[] goals;

    [Header("Setup")]
    public float cameraTransitionDelay = 2f;
    public GameObject confettiParticleSystem;

    private Rigidbody ballRigidbody;
    private Transform playerTransform;
    private bool isPlayerInRange = false;

    private void Start()
    {
        kickButton.SetActive(false);
        ballRigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        kickButton.GetComponent<Button>().onClick.AddListener(OnKickButtonPressed);
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
    }

    public void OnKickButtonPressed()
    {
        if (playerTransform == null || !isPlayerInRange) return;

        Transform nearestGoal = GetNearestGoal();
        if (nearestGoal == null) return;

        SoundManager.Instance.PlayKickBallSound();
        Vector3 direction = (nearestGoal.position - transform.position).normalized;
        ballRigidbody.AddForce(direction * kickForce, ForceMode.Impulse);
        Camera.main.GetComponent<CameraFollow>().SwitchTargetTo(transform, cameraTransitionDelay);
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

    private void OnCollisionEnter(Collision collision)
    {
        foreach (Transform goal in goals)
        {
            if (collision.gameObject.CompareTag(goal.tag))
            {
                SoundManager.Instance.PlayScoringGoalSound();
                Instantiate(confettiParticleSystem, collision.contacts[0].point, Quaternion.identity).DestroyAfter(3f);
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