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
        
        [NonSerialized] 
        public BattleState BattleState = new BattleState();
        
        public Language Language;
        public float Volume = 0.5f;
        
        public LevelState[] Levels;
        public AchieveState[] Achievements;
        public int AchievementsTrophies;

        public bool FirstLaunch = true;
        public bool FLClassic = true;
        public bool FLQuantity = true;

        public LevelState GetCurrentLevel()
        {
            return Levels[BattleState.LevelID];
        }
    }

    public enum LevelType
    {
        Tutor,
        Exploration,
        Extraction,
        Battle
    }

    [Serializable]
    public class LevelState
    {
        public bool IsOpen;
        //public int QuantityCompleted; //future
        public LevelType Type;
        public int Complete;

        [NonSerialized]
        public LevelSO ScrObj;
    }
    
    #region Battle

    public class BattleState
    {
        public int LevelID;

        public Field Filed = new Field();
        public Inventory Inventory = new Inventory();

        [NonSerialized] 
        public List<TutorCardInfo> CurrentTutorial = new List<TutorCardInfo>();
    }

    public class Field
    {
        public CardState[,] Cells;
    }

    public class CardState
    {
        public CardGrid Grid;
        public Vector2Int Position;
        public int StartQuantity = 1;
        public int Quantity = 1;
        public int Block;

        //NonSerialized don't save in save system
        [NonSerialized]
        public CardSO ScrObj;
        [NonSerialized]
        public CardGameObject GameObject;
    }
    
    public class Inventory
    {
        public CardState[,] Items;
    }
    
    [Serializable]
    public class AchieveState
    {
        public string Key;
        public int Level;
        public int Progress = 0;
        public int MaxProgress;
        public int Reward;
        public bool Complete = false;
        
        [NonSerialized]
        public Achivement AchiveGO;
    }

    public enum CardGrid
    {
        Field,
        Inventory
    }

    public enum TypeCard
    {
        Crystal,
        Item,
        Resource,
        Block
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