using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGrid
{
    [Serializable]
    public class TutorCardInfo
    {
        public bool AnyItem = true;
        public Vector2Int ItemPosition;
        public Vector2Int FieldPosition;
    }
    
    [CreateAssetMenu (fileName = "TutorSequence ")]
    public class TutorialSequence : ScriptableObject
    {
        public List<TutorCardInfo> Cards;
    }
}