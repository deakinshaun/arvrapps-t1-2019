using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static MainManager instance = null;

    private int currentStage;

    [SerializeField]
    Vector3 storedScale;


    public Vector3 StoredScale { get => storedScale; set => storedScale = value; }

    void Awake()
    {
        //Creating a Singleton Pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void onModelPlaced()
    {
        // Enable the Scaling UI
        GameObject.FindGameObjectWithTag("ScalingUI").gameObject.SetActive(true);

        // Change the text
        if (currentStage == 1)
        {
            UIManager.instance.ChangeText("Use the UI to inspect");
        }
        else if (currentStage == 2)
        {
            UIManager.instance.ChangeText("Tap on walls to draw power lines");
        }
        else
        {
            UIManager.instance.ChangeText("Choose an object to place inside the house");
        }
    }

    public void OnConfirmStageExit()
    {

    }

    private void OnLevelWasLoaded(int level)
    {
        currentStage = level;
        switch (level)
        {
            // If mapbox level was loaded
            case 0:
                
                break;
            // If drawing level was loaded
            case 1:
                
                break;
            // If designing level was loaded
            case 3:
            
                break;
        }
    }
}