using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public GameObject[] playerHolder;
    private void Awake()
    {
        if (GameManager.Instance.playerCount == 2)
        {
            playerHolder[0].SetActive(true);
            playerHolder[1].SetActive(true);
        }
        else if (GameManager.Instance.playerCount == 3)
        {
            playerHolder[0].SetActive(true);
            playerHolder[1].SetActive(true);
            playerHolder[2].SetActive(true);
        }
        else if (GameManager.Instance.playerCount == 4)
        {
            playerHolder[0].SetActive(true);
            playerHolder[1].SetActive(true);
            playerHolder[2].SetActive(true);
            playerHolder[3].SetActive(true);
        }
    }
}
