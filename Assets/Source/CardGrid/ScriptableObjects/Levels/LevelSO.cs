using System;
using UnityEngine;

[CreateAssetMenu]
public class LevelSO : ScriptableObject
{
    public string LevelName;
    public string NeedTowerToOpen; //enum Type
    public int NeedLevelToOpen;
    [Range(0, 1)] 
    public float ChanceItemOnFiled = 0.1f;
    public float ChanceResourceOnFiled = 0.1f;
    public CardSO[] Cards;
    public CardSO[] Items;

    [NonSerialized]
    public int ArrayTypeIndex;
}
