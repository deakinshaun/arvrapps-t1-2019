using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 

public class UIManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static UIManager instance = null;

    int currentStage = 0;

    [SerializeField]
    private Text InstText;

    [SerializeField, Space]
    Button AButton;
    [SerializeField]
    Button EButton;
    [SerializeField]
    Button IButton;

    [SerializeField, Space]
    Button AProgressButton;
    [SerializeField]
    Button EProgressButton;
    [SerializeField]
    Button IProgressButton;

    [SerializeField, Space]
    Button ASaveButton;
    [SerializeField]
    Button ESaveButton;
    [SerializeField]
    Button ISaveButton;

    Color Disabled = new Color(82, 82, 82, 128);

    private GameObject ArchitectureUI;
    private GameObject ScalingUI;
    private GameObject DrawingUI;
    private GameObject DesigningUI;

    void Awake()
    {
        //Creating a Singleton Pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        ChangeText("Tap on a site");
    }

    public void Save()
    {
        //MainManager.instance.OnConfirmStageExit();
    }

    public void ChangeProgress(float value)
    {
        switch (value)
        {
            case 0:
                break;
            case 1:
                if (!MainManager.instance.ModelSelected)
                {
                    MainManager.instance.ModelSelected = true;
                    ChangeText("Place Model on Tick");
                }
                else
                {
                    AProgressButton.GetComponentInChildren<Image>().color = Color.yellow;
                    ASaveButton.enabled = true;

                    //if()
                }
                break;
        }
    }


    public void ChangeText(string text)
    {
        InstText.text = text;
    }

    public void ChangeLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void OnLevelProgress()
    {
        switch (currentStage)
        {
            case 0:
                break;
            case 1:
                if (!MainManager.instance.ModelSelected)
                {
                    MainManager.instance.ModelSelected = true;
                    ChangeText("Place Model on Tick");
                }
                else
                {
                    AProgressButton.GetComponentInChildren<Image>().color = Color.yellow;
                    ASaveButton.enabled = true;
                }
                break;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        currentStage = level;

        //UpdateButtons(level);
        switch (level)
        {
            case 0:
                
                break;

            case 1:
                ChangeText("Select a model to place");
                AButton.GetComponent<Image>().color = Color.green;
                AProgressButton.GetComponentInChildren<Image>().color = Color.red;
                EProgressButton.GetComponentInChildren<Image>().color = Color.red;
                IProgressButton.GetComponentInChildren<Image>().color = Color.red;
                EButton.interactable = false;
                IButton.interactable = false;
                ASaveButton.interactable = false;
                ESaveButton.interactable = false;
                ISaveButton.interactable = false;
                break;

            case 2:

                break;
  
            case 3:
                
                break;
        }
    }

    void UpdateButtons(int level)
    {
        
    }
}