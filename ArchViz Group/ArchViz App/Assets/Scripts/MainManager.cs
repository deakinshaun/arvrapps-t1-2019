using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class MainManager : MonoBehaviour
{
    private bool changesConfirmed;

    [SerializeField]
    Vector3 storedScale;

    // 

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static MainManager instance = null;

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

    // Coroutine for Mapbox level
    IEnumerator MapboxLevel()
    {
        while (!changesConfirmed)
        {

        }
        return null;
    }

    // Coroutine for Mapbox level
    IEnumerator DrawingLevel()
    {
        // Conditions for the Drawing scene which need to be met to progress to next scene
        bool EPointPlaced = false;
        bool WPointPlaced = false;


        while (!changesConfirmed)
        {

        }

        // Attach the model to this object

        // Change Level

        return null;
    }

    // Coroutine for Mapbox level
    IEnumerator DesigningLevel()
    {
        // Conditions for the Drawing scene which need to be met to progress to next scene
        bool FurniturePlaced = false;
        bool WallPainted = false;

        while (!changesConfirmed)
        {

        }
        return null;
    }


    private void OnLevelWasLoaded(int level)
    {
        changesConfirmed = false;

        switch (level)
        {
            // If mapbox level was loaded
            case 0:
                StartCoroutine(MapboxLevel());
                break;
            // If drawing level was loaded
            case 1:
                StartCoroutine(DrawingLevel());
                break;
            // If designing level was loaded
            case 3:
                StartCoroutine(DesigningLevel());
                break;
        }
    }
}