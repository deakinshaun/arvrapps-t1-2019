using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  A data representation of a generic task
/// </summary>

public class GenericTask 
{
    List<GenericContent> contents;

    public string Display{ 
        get 
        {
            string output = "";
            foreach (GenericContent G in contents)
            {
                output += G.Content + "/n";
            }
            return output;
        }
    }
}
