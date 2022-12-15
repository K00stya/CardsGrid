using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelsListSO : ScriptableObject
{
    public LevelExplorationSO[] Exploration;
    public LevelExtractionSO[] ResourcesExtraction;
    public LevelBattleSO[] Battle;
}
