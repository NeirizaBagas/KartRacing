using UnityEngine;

public class KartCameraFollow : MonoBehaviour
{
    private Transform _target;
    private float _speed;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, _speed);
    }

    public void SetTarget(Transform target, float pitchAngle, float speed)
    {
        _target = target;
        _speed = speed;
        transform.eulerAngles = new Vector3(pitchAngle, 0, 0);
    }
}