using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private Transform defaultCamTarget;
    private Vector3 defaultCamOffset;
    private bool isFollowingPlayer = true;

    void Start()
    {
        if (offset == Vector3.zero)
        {
            offset = new Vector3(0, 10, -10);
        }

        defaultCamTarget = player;
        defaultCamOffset = offset;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player);
        }
    }

    public void SwitchTargetTo(Transform ball, float delay)
    {
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
        player = newTarget;
        offset = new Vector3(0, 7, 0);

        yield return new WaitForSeconds(delay);

        player = defaultCamTarget;
        offset = defaultCamOffset;
        isFollowingPlayer = true;
    }
}