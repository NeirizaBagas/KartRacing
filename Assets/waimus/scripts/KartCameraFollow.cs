using UnityEngine;

public class KartCameraFollow : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset;
    private float _speed;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _speed);
    }

    public void SetTarget(Transform target, Vector3 offset, float pitchAngle, float speed)
    {
        _target = target;
        _offset = offset;
        _speed = speed;
        transform.eulerAngles = new Vector3(pitchAngle, 0, 0);
    }
}