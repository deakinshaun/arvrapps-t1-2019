using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectureManager : MonoBehaviour
{

    public void OnModelSelected()
    {
        Debug.Log("Function Being Called");
        if(MainManager.instance.AchitectureDone1 == 20)
            MainManager.instance.OnLevelProgress();
    }
    
}
