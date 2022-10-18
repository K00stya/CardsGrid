using UnityEngine;

[CreateAssetMenu]
public class InfiniteLevelSO : ScriptableObject
{
    public string LevelName;
    public int NeedScoreToOpen;
    public bool Open = false;
    [Min(1)] 
    public int StartMaxCellQuantity = 10;
    [Range(0, 1)] 
    public float ChanceItemOnFiled = 0.1f;
    public CardSO[] Enemies;
    public CardSO[] Items;
}
