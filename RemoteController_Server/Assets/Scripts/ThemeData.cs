using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThemeData
{
    public string id;   // "original", "green", "blue", "dark"

    public float messageR, messageG, messageB, messageA;
    public float backgroundR, backgroundG, backgroundB, backgroundA;
    public float textR, textG, textB, textA;

    public Color MessageColor =>
        new Color(messageR, messageG, messageB, messageA);

    public Color BackgroundColor =>
        new Color(backgroundR, backgroundG, backgroundB, backgroundA);

    public Color TextColor =>
        new Color(textR, textG, textB, textA);
}

[System.Serializable]
public class ThemeCollection
{
    public string activeThemeId;
    public List<ThemeData> themes;
}

