using UnityEngine;

public class StretchingDecal : MonoBehaviour
{
    public float stretchSpeed = 2f;  // seberapa cepat panjangnya bertambah
    public float maxStretch = 20f;   // maksimal panjang
    private bool stretching = true;

    void Update()
    {
        if (stretching)
        {
            Vector3 scale = transform.localScale;
            scale.x += stretchSpeed * Time.deltaTime;
            if (scale.x >= maxStretch)
            {
                scale.x = maxStretch;
                stretching = false;
            }
            transform.localScale = scale;
        }
    }

    public void StopStretch()
    {
        stretching = false;
    }
}
