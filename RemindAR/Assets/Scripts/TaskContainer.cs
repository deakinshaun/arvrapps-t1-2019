using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Unity Monobehaviour representation of a task
/// </summary>
public class TaskContainer : MonoBehaviour
{
    public GenericTask Task { get; private set;}


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
    }
}
