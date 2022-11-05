using System;
using TMPro;
using UnityEngine;

public enum Language
{
    English = 0,
    Russian = 1
}

[Serializable]
public class Loc
{
    public Language Language;
    [TextArea]
    public string Text;
}

[Serializable]
public class LocText
{
    public TextMeshProUGUI View;
    public Loc[] Localizations;
}

public class LocalizationSystem : MonoBehaviour
{
    [Header("Manu")]
    public LocText[] Texts1;
    [Header("Battle")]
    public LocText[] Texts2;
    [Header("Tutorials")]
    public LocText[] Texts3;
}
