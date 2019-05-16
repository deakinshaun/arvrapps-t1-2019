using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 

public class UIManager : MonoBehaviour
{
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
    }

    public void ChangeLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void OnLevelWasLoaded(int level)
    {
        List<GameObject> children;
        children = new List<GameObject>();
        foreach (Transform t in transform)
        {
            children.Add(t.gameObject);
        }

        int sceneID = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < children.Count; i++)
        {
            string objectName = "Button" + (i + 1);
            Button button = GameObject.Find(objectName).GetComponent<Button>();
            button.interactable = true;

            if (i == sceneID)
            {
                button.interactable = false;
            }
        }
    }

    //Update is called every frame.
    void Update()
    {

    }
}