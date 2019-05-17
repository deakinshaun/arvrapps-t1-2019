using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 

public class UIManager : MonoBehaviour
{
    // Reference to the MapBox UI
    [SerializeField]
    private GameObject ScalingUI;

    // Reference to the Drawing UI
    [SerializeField]
    private GameObject DrawingUI;

    // Reference to the Designing UI
    [SerializeField]
    private GameObject DesigningUI;

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static UIManager instance = null;

    void Awake()
    {
        //Creating a Singleton Pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //ScalingUI = transform.GetChild(1).gameObject;
        //DrawingUI = transform.GetChild(2).gameObject;
        //DesigningUI = transform.GetChild(3).gameObject;
    }

    public void ChangeLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void OnLevelWasLoaded(int level)
    {
        UpdateButtons(level);
        switch (level)
        {
            // If mapbox level was loaded
            case 0:
                ScalingUI.gameObject.SetActive(false);
                DrawingUI.gameObject.SetActive(false);
                DesigningUI.gameObject.SetActive(false);
                break;
            // If drawing level was loaded
            case 1:
                ScalingUI.gameObject.SetActive(false);
                DrawingUI.gameObject.SetActive(true);
                DesigningUI.gameObject.SetActive(false);
                break;
            // If designing level was loaded
            case 3:
                ScalingUI.gameObject.SetActive(false);
                DrawingUI.gameObject.SetActive(false);
                DesigningUI.gameObject.SetActive(true);
                break;
        }
    }

    public void ToggleScalingUI(bool active) { ScalingUI.gameObject.SetActive(active); }
    public void ToggleDrawingUI(bool active) { DrawingUI.gameObject.SetActive(active); }
    public void ToggleDesigningBoxUI(bool active) { DesigningUI.gameObject.SetActive(active); }

    void UpdateButtons(int level)
    {
        for (int i = 0; i < 4; i++)
        {
            Button button = GameObject.FindGameObjectWithTag("SceneButtons").transform.GetChild(i).GetComponent<Button>();
            if (i == level)
            {
                button.interactable = false;
                continue;
            }
            button.interactable = true;
        }
    }
}