using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGrid
{
    //TODO SEPARATE SAVES?
    public class PlayerCommonState
    {
        public int BestLevelClassic;
        public int BestLevelQuantity;
        public bool InBattle;
        
        public BattleState BattleState = new BattleState();
        
        public Language Language;
        public float Volume = 0.5f;

        public LevelState[] Levels;

        [NonSerialized] 
        public List<TutorCardInfo> CurrentTutorial = new List<TutorCardInfo>();

        public LevelState GetCurrentLevel()
        {
            return Levels[BattleState.GetRealLevelID()];
        }
    }

    public struct LevelState
    {
        public bool IsOpen;
        public int Group;
        public int Stars;
        public bool Complete;
        public (ColorType, int)[] CollectColors;
        public (ShapeType, int)[] CollectShape;
        public bool NeedSpawnNewRandom;
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
        public int NumberLevel = 1;
        public int LevelProgress;
        public int MaxLevelProgress = 10;
        public (ColorType, int)[] CollectColors;

        public Field Filed = new Field();
        public Inventory Inventory = new Inventory();

        public int GetRealLevelID()
        {
            if (LevelID >= CommonLevelID)
                return LevelID - CommonLevelID;
            else
                return LevelID;
        }
    }

    public class Field
    {
        public CardState[,] Cells;
    }

    public class CardState
    {
        public CardSO CardSO;
        public CardGrid Grid;
        public Vector2Int Position;
        public int Quantity;
        public int Chains;

        //NonSerialized don't save in save system
        [NonSerialized]
        public CardGameObject GameObject;
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
        Item,
        Block
    }

    public enum ShapeType
    {
        Rhomb,
        Diamond,
        Hexagonal,
        Octagonal,
        Pearl
    }

    public enum ColorType
    {
        Blue,
        Green,
        Purple,
        Yellow,
        Red
    }

    #endregion
}