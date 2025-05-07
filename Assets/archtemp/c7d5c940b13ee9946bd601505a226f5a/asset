using UnityEngine;

public class RibbonWobble : MonoBehaviour
{
    public float wobbleSpeed = 3f;
    public float wobbleAmount = 0.2f;

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        transform.localPosition = initialPos + new Vector3(0, wobble, 0);
    }
}
