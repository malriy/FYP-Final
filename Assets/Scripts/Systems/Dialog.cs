using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string speaker;
    public string line;
}

[System.Serializable]
public class Dialog
{
    [SerializeField] List<DialogLine> lines;

    public List<DialogLine> Lines
    {
        get { return lines; }
    }
}
