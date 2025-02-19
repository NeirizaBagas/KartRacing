using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject startLine;
    public GameObject finishLine;
    public GameObject checkPointLine;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        startLine.SetActive(true);
        finishLine.SetActive(false);
        checkPointLine.SetActive(false);
    }

    public void StartLineTriggered()
    {
        Debug.Log("Start line triggered!");
        startLine.SetActive(false);
        checkPointLine.SetActive(true);
    }

    public void CheckpointLineTriggered()
    {
        Debug.Log("Checkpoint line triggered!");
        finishLine.SetActive(true);
        checkPointLine.SetActive(false);
    }

    public void FinishLineTriggered()
    {
        Debug.Log("Finish line triggered!");
        finishLine.SetActive(false);
        checkPointLine.SetActive(true);

        
    }

    
}
