using UnityEngine;

public class KartCameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    private float speed;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed);
    }

    public void SetTarget(Transform followTarget, Vector3 cameraOffset, float pitchAngle, float followSpeed)
    {
        target = followTarget;
        offset = cameraOffset;
        speed = followSpeed;
        transform.eulerAngles = new Vector3(pitchAngle, 0, 0);
    }
}