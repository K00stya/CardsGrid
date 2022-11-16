using System;
using UnityEngine;

namespace CardGrid
{
    [Serializable]
    public class AchiveLevel
    {
        public int Reward;
        public int MaxProgress;
    }
    
    [CreateAssetMenu]
    public class AchieveSO : ScriptableObject
    {
        public AchiveLevel[] Levels;
        public Loc[] Localizations;
    }
}