using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A Unity Monobehaviour representation of a task
/// </summary>
public class TaskContainer : MonoBehaviour
{
    public GenericTask Task { get; private set;}
    public Text TitleText;
    public Text ContentText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTask(GenericTask _task)
    {
        Task = _task;
        TitleText.text = _task.Title;
        ContentText.text = _task.Display;
    }
}
