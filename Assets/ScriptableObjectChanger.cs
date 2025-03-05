using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectChanger : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObject;
    [SerializeField] private MapDisplay mapDisplay;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        ChangerScriptableObject(0);
    }

    public void ChangerScriptableObject(int _change)
    {
        currentIndex += _change;
        if (currentIndex < 0) currentIndex = scriptableObject.Length - 1;
        else if (currentIndex > scriptableObject.Length - 1) currentIndex = 0;
        Debug.Log(currentIndex);

        if(mapDisplay != null) mapDisplay.DisplayMap((Map)scriptableObject[currentIndex]);
    }
}
