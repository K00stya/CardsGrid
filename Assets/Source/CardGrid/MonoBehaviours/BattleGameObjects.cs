using System;
using UnityEngine;

namespace CardGrid
{
    [Serializable]
    public class BattleGameObjects : MonoBehaviour
    {
        public CardGameObject CardPrefab;
        public GridGameObject Field;
        public GridGameObject Inventory;
        public Transform ParentEffects;
        public Transform FieldRotator;
    }
}