using System;
using UnityEngine;

namespace CardGrid
{
    //TODO SEPARATE SAVES?
    public class PlayerCommonState
    {
        public int BestScore;
        public bool InBattle;
        
        public BattleState BattleState = new BattleState();
        
        public Language Language;
        public float Volume = 0.5f;

        public LevelState[] Levels;
    }

    public struct LevelState
    {
        public bool IsOpen;
        public int Group;
        public int IdInGroup;
        public int Stars;
        public bool Complete;
    }

    public enum Language
    {
        English = 0,
        Russian = 1
    }
    
    #region Battle

    public class BattleState
    {
        public const int CommonLevelID = 100;
        public int LevelID;
        public int LevelStar;
        public int Score;
        public int Health;
        public int Money;

        public Field Filed = new Field();
        public Inventory Inventory = new Inventory();
    }

    public class Field
    {
        public CardState[,] Cells;
    }

    public class CardState
    {
        public string Name;
        public CardSO CardSO;
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
        public CardState[,] Items;
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