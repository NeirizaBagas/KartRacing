using Cinemachine;
using UnityEngine;

public class KartCameraFollow : MonoBehaviour
{
    public Rigidbody kartRigidbody;
    public CinemachineVirtualCamera virtualCamera;
    public LayerMask groundLayer;
    public float heightOffset = 2f;
    public float distanceFromTarget = 5f;
    public float rotationSmoothSpeed = 5f;
    public float positionSmoothSpeed = 10f;
    public float maxSlopeAngle = 60f;

    private Vector3 smoothGroundNormal = Vector3.up;
    private Vector3 velocityPosition;
    private Vector3 velocityNormal;
    private Vector3 lastValidNormal = Vector3.up;
    private CinemachineTransposer transposer;

    void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = kartRigidbody.transform;
        }
    }

    void LateUpdate()
    {
        if (kartRigidbody == null || virtualCamera == null) return;

        Vector3 raycastStart = kartRigidbody.position + Vector3.up * 0.5f;

        RaycastHit hit;
        Vector3 groundNormal = Vector3.up;
        bool validNormalFound = false;

        Vector3[] raycastOffsets = new Vector3[]
        {
            Vector3.zero,
            Vector3.right * 0.5f,
            -Vector3.right * 0.5f,
            Vector3.forward * 0.5f,
            -Vector3.forward * 0.5f
        };

        int hitCount = 0;
        foreach (Vector3 offset in raycastOffsets)
        {
            if (Physics.Raycast(raycastStart + offset, Vector3.down, out hit, 10f, groundLayer))
            {
                groundNormal += hit.normal;
                hitCount++;
                validNormalFound = true;
            }
        }

        if (validNormalFound)
        {
            groundNormal /= hitCount;
            groundNormal.Normalize();
            lastValidNormal = groundNormal;
        }
        else
        {
            groundNormal = lastValidNormal;
        }

        smoothGroundNormal = Vector3.SmoothDamp(
            smoothGroundNormal,
            groundNormal,
            ref velocityNormal,
            1f / rotationSmoothSpeed
        );

        float angle = Vector3.Angle(Vector3.up, smoothGroundNormal);
        if (angle > maxSlopeAngle)
        {
            smoothGroundNormal = Vector3.RotateTowards(
                Vector3.up,
                smoothGroundNormal,
                maxSlopeAngle * Mathf.Deg2Rad,
                0f
            );
        }

        Vector3 forwardDirection = kartRigidbody.velocity.normalized;
        if (kartRigidbody.velocity.magnitude < 0.1f)
        {
            forwardDirection = kartRigidbody.transform.forward;
        }

        Vector3 right = Vector3.Cross(forwardDirection, smoothGroundNormal).normalized;
        Vector3 forward = Vector3.Cross(smoothGroundNormal, right).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(forward, smoothGroundNormal);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmoothSpeed
        );

        Vector3 targetPosition = kartRigidbody.position;
        targetPosition -= forward * distanceFromTarget;
        targetPosition += smoothGroundNormal * heightOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocityPosition,
            1f / positionSmoothSpeed
        );

        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, heightOffset, -distanceFromTarget);
        }
    }

    void OnDrawGizmos()
    {
        if (kartRigidbody != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                kartRigidbody.position,
                kartRigidbody.position + smoothGroundNormal * 2f
            );

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(kartRigidbody.position + Vector3.up * 0.5f, 0.1f);
        }
    }
}