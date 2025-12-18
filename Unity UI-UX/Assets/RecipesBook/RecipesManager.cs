using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Recipe
{
    [Header("Variables")]
    public string Name;
    [TextArea(3, 10)]
    public string Content;
}

