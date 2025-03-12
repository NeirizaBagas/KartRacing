using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    public Image indicatorImage; // Gambar indikator
    public Color playerColor; // Warna indikator
    public int playerIndex; // Index pemain

    void Start()
    {
        indicatorImage.color = playerColor; // Set warna indikator
    }

    public void MoveToCar(int carIndex)
    {
        // Pindahkan indikator ke posisi mobil yang dipilih
        Transform car = PlayerInputManager.Instance.carList[carIndex].transform;
        transform.position = car.position;
    }
}