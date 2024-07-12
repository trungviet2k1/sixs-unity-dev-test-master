using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new(0, 10, -10);

    private Transform defaultCamTarget;
    private Vector3 defaultCamOffset;
    private bool isFollowingPlayer = true;
    private Transform currentTarget;

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

        if (!isFollowingPlayer)
        {
            transform.position = ball.position + offset;
            transform.LookAt(ball);
            return;
        }

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