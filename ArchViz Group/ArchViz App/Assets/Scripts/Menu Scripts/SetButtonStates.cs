using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetButtonStates : MonoBehaviour
{
    List<GameObject> children;

    void Start()
    {
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
}
