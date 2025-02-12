using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer
{
    public List<DialogueNode> start;
}

[Serializable]
public class DialogueNode
{
    public int ID;
    public string name;
    public string text;
    public List<DialogueChoice> choices;
}

[Serializable]
public class DialogueChoice
{
    public string text;
    public int next;
}
