using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGrid
{
    [Serializable]
    public class CardStartInfo
    {
        public CardSO Card;
        [Range(1, 10)]
        public int Quantity = 1;
    }
    
    [CreateAssetMenu (fileName = "Level ")]
    public class LevelSO : ScriptableObject
    {
        public TutorialSequence TutorSequence;
        public CardStartInfo[] Column1;
        public CardStartInfo[] Column2;
        public CardStartInfo[] Column3;
        public CardStartInfo[] Column4;
        public CardStartInfo[] Column5;
        public CardStartInfo[] Column6;
        public CardStartInfo[] Inventory;

        public List<CardStartInfo[]> Columns;
        
        public void Init()
        {
            Columns = new List<CardStartInfo[]>() { Column1,Column2,Column3,Column4,Column5,Column6 };
        }
    }
}
