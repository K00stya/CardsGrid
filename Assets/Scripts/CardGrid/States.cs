using System;
using UnityEngine;

namespace CardGrid
{
    public class PlayerCommonState
    {
        public int BestScore;
        public bool InBattle;
        
        public BattleState BattleState = new BattleState();
        
        public Language Language;
        public float Volume;
    }

    public enum Language
    {
        English = 0,
        Russian = 1
    }
    
    #region Battle

    public class BattleState
    {
        public int LevelID;
        public int Score;
        public int Health;
        public int Money;

        public Field Filed = new Field();
        public Inventory Inventory = new Inventory();
    }

    public class Field
    {
        public Card[,] Cells;
    }

    public class Card
    {
        public string name;
        public TypeCard Type;
        public CardGrid Grid;
        public Vector2Int Position;
        public int StartQuantity;
        public int Quantity;

        //NonSerialized don't save in save system
        [NonSerialized]
        public CardGameObject GameObject;
        [NonSerialized]
        public Maps ImpactMap;
        [NonSerialized]
        public GameObject Effect;
    }
    
    public class Inventory
    {
        public Card[,] Items;
    }

    public enum CardGrid
    {
        Field,
        Inventory
    }

    public enum TypeCard
    {
        Enemy,
        Item
    }
    
    #endregion
}