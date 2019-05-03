using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public GameObject TaskContainerPrefab;
    public List<Transform> TaskContainers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNewTask()
    {
        var _taskContainerObject = Instantiate(TaskContainerPrefab);
        var _taskContainer = _taskContainerObject.GetComponent<TaskContainer>();

        _taskContainer.SetTask(new GenericTask("Test task").AddContent(new TextContent("Test contents")));

        TaskContainers.Add(_taskContainerObject.transform);
    }
}
