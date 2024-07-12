using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset = new();

    private Transform defaultCamTarget;
    private Vector3 defaultCamOffset;
    private Transform currentTarget;

    [HideInInspector] public bool isFollowingPlayer = true;

    void Start()
    {
        defaultCamTarget = player;
        defaultCamOffset = offset;
        currentTarget = player;
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            transform.position = currentTarget.position + offset;
            transform.LookAt(currentTarget);
        }
    }

    public void SwitchTargetTo(Transform ball, float delay)
    {
        StopAllCoroutines();
        StartCoroutine(WaitAndSwitchTarget(ball, delay));
    }

    private IEnumerator WaitAndSwitchTarget(Transform newTarget, float delay)
    {
        isFollowingPlayer = false;
        currentTarget = newTarget;
        offset = new Vector3(0, 7, 0);

        yield return new WaitForSeconds(delay);

        currentTarget = defaultCamTarget;
        offset = defaultCamOffset;
        isFollowingPlayer = true;
    }
}