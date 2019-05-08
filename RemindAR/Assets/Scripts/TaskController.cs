using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public GameObject TaskContainerPrefab;
    public List<Transform> TaskContainers;
    private bool f_datafull = false;

    // Start is called before the first frame update
    void Start()
    {
        AirtableInterface.Singleton.GetData();
    }

    // Update is called once per frame
    void Update()
    {
        if (AirtableInterface.Singleton.f_dataAcquired && !f_datafull)
        {
            foreach (DatabaseEntry _entry in AirtableInterface.Singleton.Database)
            {
                CreateNewTask(_entry.Title, _entry.Content);
            }

        }
        f_datafull = true;
    }

    void CreateNewTask( string _title, string _content)
    {
        var _location = transform.position + (Vector3.forward * 0.2f * TaskContainers.Count);
        var _taskContainerObject = Instantiate(TaskContainerPrefab, _location, transform.rotation);
        var _taskContainer = _taskContainerObject.GetComponent<TaskContainer>();

        _taskContainer.SetTask(new GenericTask(_title).AddContent(new TextContent(_content)));

        TaskContainers.Add(_taskContainerObject.transform);
    }

    public void RemoveContainers()
    {
        foreach (Transform trans in TaskContainers)
        {
            Destroy(trans.gameObject);
        }
    }
}
