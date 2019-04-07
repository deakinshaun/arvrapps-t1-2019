using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContent : GenericContent
{
    public override string Content {get {return textData;} }
    private string textData;

    public TextContent()
    {   
        textData = "";
        return this;
    }

    public TextContent(string text)
    {
        textData = text;
        return this;
    }

    public void Edit(string text)
    {
        textData = text;
    }
}