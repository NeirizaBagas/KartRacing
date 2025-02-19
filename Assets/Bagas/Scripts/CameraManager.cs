using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    public Transform player1; // Objek Player 1
    public Transform player2; // Objek Player 2
    public CinemachineVirtualCamera vcamPlayer1; // Cinemachine Virtual Camera untuk Player 1
    public CinemachineVirtualCamera vcamPlayer2; // Cinemachine Virtual Camera untuk Player 2
    public Camera cameraPlayer1; // Kamera utama untuk Player 1
    public Camera cameraPlayer2; // Kamera utama untuk Player 2

    private void Start()
    {
        // Atur Follow dan Look At untuk Player 1
        vcamPlayer1.Follow = player1;
        vcamPlayer1.LookAt = player1;

        // Atur Follow dan Look At untuk Player 2
        vcamPlayer2.Follow = player2;
        vcamPlayer2.LookAt = player2;

        // Atur Viewport Rect untuk Player 1
        cameraPlayer1.rect = new Rect(0, 0, 0.5f, 1);

        // Atur Viewport Rect untuk Player 2
        cameraPlayer2.rect = new Rect(0.5f, 0, 0.5f, 1);

        // Atur prioritas CinemachineVirtualCamera
        vcamPlayer1.Priority = 10;
        vcamPlayer2.Priority = 11;
    }
}