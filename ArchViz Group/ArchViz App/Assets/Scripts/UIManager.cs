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
    [SerializeField]
    public Text DebugText;

    [SerializeField, Space]
    Button AButton;
    [SerializeField]
    Button EButton;
    [SerializeField]
    Button IButton;

    [SerializeField, Space]
    Image AProgress;
    [SerializeField]
    Image EProgress;
    [SerializeField]
    Image IProgress;

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

        ChangeText("Tap on a site", false);
    }

    public void OnLevelProgress(int code)
    {
        //DebugText.text = DebugText.text + "\n"+code;
        switch (code)
        {
            //----Architecture Scene
            case 21:
                ChangeText("Place model on green 'Tick'", false);
                break;
            // Model Placed
            case 22:
                ChangeText("Save your changes", false);
                GameObject.FindGameObjectWithTag("ScalingUI").transform.GetChild(0).gameObject.SetActive(true);
                GameObject.FindGameObjectWithTag("ScalingUI").GetComponent<ScalingUI>().SetModel();
                AProgress.color = Color.yellow;
                ASaveButton.interactable = true;
                break;
            // On Save
            case 23:
                UIManager.instance.ChangeText("Your work is saved", false);
                AProgress.color = Color.green;
                ASaveButton.interactable = false;
                EButton.interactable = enabled;
                break;
            //----Drawing Stage
            case 31:
                GameObject.FindGameObjectWithTag("ScalingUI").transform.GetChild(0).gameObject.SetActive(true);
                GameObject.FindGameObjectWithTag("ScalingUI").GetComponent<ScalingUI>().SetModel();
                ChangeText("Tap on wall to place point", false);
                break;
            case 32:
                ChangeText("Try using the other line", false);
                break;
            // Power/Water Lines Drawn
            case 33:
                ChangeText("Save your progress", false);
                EProgress.color = Color.yellow;
                ESaveButton.interactable = true;
                break;
            // On Saved
            case 34:
                ChangeText("Your work is saved", false);
                EProgress.color = Color.blue;
                ESaveButton.interactable = false;
                IButton.interactable = true;
                break;
            //----Interior Stage
            case 41:
                GameObject.FindGameObjectWithTag("ScalingUI").transform.GetChild(0).gameObject.SetActive(true);
                GameObject.FindGameObjectWithTag("ScalingUI").GetComponent<ScalingUI>().SetModel();
                ChangeText("Place a furniture", false);
                break;
            // Furniture placed
            case 42:
                ChangeText("Paint a wall or save", false);
                ISaveButton.interactable = true;
                EProgress.color = Color.yellow;
                break;
            case 43:
                ChangeText("Save your progress", false);
                break;
            case 44:
                ChangeText("Your work is saved", false);
                IProgress.color = Color.green;
                break;
            default:
                ChangeText("", false);
                break;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.GetChild(2).gameObject.SetActive(false);

        if (transform.GetChild(3).gameObject.activeSelf == true)
            transform.GetChild(3).gameObject.SetActive(false);
        currentStage = level;
        
        //UpdateButtons(level);
        switch (level)
        { 
            case 0:
                ChangeText("Select a site", false);
                break;
            // On map box level load
            case 1:
                ChangeText("Select a Model", false);
                AButton.GetComponent<Image>().color = Color.green;
                break;
            case 2:
                ChangeText("Place the model", false);
                EButton.GetComponent<Image>().color = Color.green;
                AButton.GetComponent<Image>().color = Color.white;
                break;
            case 3:
                ChangeText("Place the model", false);
                IButton.GetComponent<Image>().color = Color.green;
                EButton.GetComponent<Image>().color = Color.white;
                break;
        }
    }

    public void ChangeText(string text, bool important)
    {
        InstText.text = text;
        if (important)
            InstText.color = Color.red;
        else
            InstText.color = Color.green;
    }

    public void ChangeLevel(int i)
    {
        if(currentStage != i)
            SceneManager.LoadScene(i);
    }

    public void Save()
    {
        MainManager.instance.OnSave();
    }
}