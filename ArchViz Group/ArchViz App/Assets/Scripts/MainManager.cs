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
    int ArchitectureDone = 20;
    [SerializeField]
    int DrawingDone = 30;
    [SerializeField]
    int InteriorDone = 40;

    public int AchitectureDone1 { get => ArchitectureDone; set => ArchitectureDone = value; }
    public int DrawingDone1 { get => DrawingDone; set => DrawingDone = value; }
    public int InteriorDone1 { get => InteriorDone; set => InteriorDone = value; }

    private bool modelSelected;
    [SerializeField]
    Vector3 storedScale;
    public Vector3 StoredScale { get => storedScale; set => storedScale = value; }
    public bool ModelSelected { get => modelSelected; set => modelSelected = value; }

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

    public void OnLevelProgress()
    {
        switch (currentStage)
        {
            case 0:
                break;
            case 1:
                ArchitectureDone++;
                // Model Selected
                if (ArchitectureDone == 21)
                {
                    UIManager.instance.OnLevelProgress(21);
                }
                // Model Placed
                if(ArchitectureDone == 22)
                {
                    UIManager.instance.OnLevelProgress(22);
                }
                // Saved
                if (ArchitectureDone == 23)
                {
                    UIManager.instance.OnLevelProgress(23);
                    // Enable Next Stage
                    // Change colour
                    // Disable save
                }
                break;
            case 2:
                DrawingDone++;
                // Model Placed
                if(DrawingDone == 31)
                {
                    UIManager.instance.OnLevelProgress(31);
                }
                // One type of line drawn 
                if (DrawingDone == 32)
                {
                    UIManager.instance.OnLevelProgress(32);
                }
                // 2nd type of line drawn 
                if (DrawingDone == 33)
                {
                    UIManager.instance.OnLevelProgress(33);
                    // Enable save button
                    // Turn progress bar to yellow
                }
                // Saved
                if(DrawingDone == 34)
                {
                    UIManager.instance.OnLevelProgress(34);
                    // Enable level button
                    // Turn progress bar to blue
                }
                break;
            case 3:
                InteriorDone++;
                // Model Placed
                if(InteriorDone == 41)
                {
                    UIManager.instance.OnLevelProgress(41);
                }
                // Furniture placed
                if (InteriorDone == 42)
                {
                    UIManager.instance.OnLevelProgress(42);
                    // Enable save button
                    // Turn progress bar to yellow
                }
                if (InteriorDone == 43)
                {
                    UIManager.instance.OnLevelProgress(43);
                }
                if (InteriorDone == 44)
                {
                    UIManager.instance.OnLevelProgress(44);
                }
                break;
        }
    }

    public void BeforeLevelLoad()
    {

    }

    public void OnSave()
    {
        switch (currentStage)
        {
            case 1:
                ArchitectureDone = 23;
                UIManager.instance.OnLevelProgress(23);
                break;
            case 2:
                DrawingDone = 34;
                UIManager.instance.OnLevelProgress(34);
                break;
            case 3:
                InteriorDone = 44;
                UIManager.instance.OnLevelProgress(44);
                break;
        }
    }
}