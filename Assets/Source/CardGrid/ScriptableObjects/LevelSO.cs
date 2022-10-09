using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CardGrid
{
    [Serializable]
    public class CardStartInfo
    {
        public AssetReference Card;
        public int Quantity;
    }
    
    [CreateAssetMenu (fileName = "Level ")]
    public class LevelSO : ScriptableObject
    {
        public CardStartInfo[] Column1 = new CardStartInfo[5];
        public CardStartInfo[] Column2 = new CardStartInfo[5];
        public CardStartInfo[] Column3 = new CardStartInfo[5];
        public CardStartInfo[] Column4 = new CardStartInfo[5];
        public CardStartInfo[] Column5 = new CardStartInfo[5];
        public CardStartInfo[] Inventory;

        public List<CardStartInfo[]> Columns;
        
        public void Init()
        {
            Columns = new List<CardStartInfo[]>() { Column1,Column2,Column3,Column4,Column5 };
        }
    }
}
