using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace CardGrid
{
    [Serializable]
    public class LevelsGroup
    {
        public LevelSO[] Levels = new LevelSO[5];
        public int QuantityStarsToOpen;
    }
    
    /*
     * The game code is divided into several files.
     * This file is the main script of the game, which is responsible
     * for the beginning/end of the game, saving other common things.
     * 
     * The name of this part corresponds to the name of the entire script
     * in order to add it as a component of an object on the object in Unity
     * and establish a connection with the rest of the Monobehaviours,
     * which have almost no logic and are only a connecting link between the engine and the game logic.
     * If it is assumed that the main external Monobehaviour have a different lifetime from the main script,
     * can be use the dependency container without difficulties.
     * As part of this small example, it is assumed that public dependencies are set through the inspector.
     */
    public partial class CardGridGame : MonoBehaviour //Common
    {
        public BattleGameObjects BattleObjects;
        public CommonGameSettings CurrentGameSeetings;
        public LevelsGroup[] CommonLevelsGroups;
        public InfiniteLevelSO[] InfiniteLevels;
        
        public CardSO[] Enemies;
        public CardSO[] Items;
        public ParticleSystem[] Effects;
        
        public AudioSource BattleAudioSource;
        public AudioSource MusicAudioSource;
        public AudioSource MenuAudioSource;
        public AudioClip WinSound;
        public AudioClip DefeateSound;
        public AudioClip ColorSound;
        public AudioClip ShapeSound;

        const string SaveName = "CardGrid";

        int _startMaxCellQuantity;
        float _chanceItemOnFiled;
        PlayerCommonState _CommonState;
        List<CardGameObject> _cardMonobehsPool;
        Camera _camera;

        void Awake()
        {
            Application.targetFrameRate = 30;
            _camera = Camera.main;
            DebugSystem.Settings = CurrentGameSeetings.Debug;

            int length = BattleObjects.Inventory.SizeX * BattleObjects.Inventory.SizeZ
                         + BattleObjects.Field.SizeX * BattleObjects.Field.SizeZ;
            _cardMonobehsPool = new List<CardGameObject>(length);

            foreach (var group in CommonLevelsGroups)
            {
                foreach (var level in group.Levels)
                {
                    if(level != null)
                        level.Init();
                }
            }
        }

        void Start()
        {
            Debug.Log(Screen.width);
            Debug.Log(Screen.height);
            //Try load save
            // if (ES3.KeyExists(SaveName) && !CurrentGameSeetings.NewSaveOnStart)
            // {
            //     LoadSave();
            // }
            //New save
            //else
            {
                _CommonState = new PlayerCommonState();
                int quantityLevels = 0;
                
                // for (int i = 0; i < CommonLevelsGroups.Length; i++)
                // {
                //     quantityLevels += CommonLevelsGroups[i].Levels.Length;
                // }
                
                //V2
                var levels = (Level[])typeof(LevelsMaps).GetField("Levels").GetValue(null);
                quantityLevels += levels.Length;

                _CommonState.Levels = new LevelState[quantityLevels];

                int levelIndex = 0;
                for (int i = 0; i < levels.Length; i++)
                {
                    _CommonState.Levels[levelIndex] = new LevelState();
                    _CommonState.Levels[levelIndex].Group = i;
                    //_CommonState.Levels[levelIndex].IdInGroup = levels[i].Group;
                    levelIndex++;
                }
                
                // int levelIndex = 0;
                // for (int i = 0; i < CommonLevelsGroups.Length; i++)
                // {
                //     for (int j = 0; j < CommonLevelsGroups[i].Levels.Length; j++)
                //     {
                //         _CommonState.Levels[levelIndex] = new LevelState();
                //         _CommonState.Levels[levelIndex].Group = i; //NumberGroup
                //         _CommonState.Levels[levelIndex].IdInGroup = j;
                //         levelIndex++;
                //     }
                // }

                DebugSystem.DebugLog("Save no exist. First active.", DebugSystem.Type.SaveSystem);
            }

            SubscribeOnButtons();
            
            OpenMainMenu();
        }
        
        void LoadBattle()
        {
            BattleState playerBattleState = _CommonState.BattleState;

            LoadLevelCards(playerBattleState.LevelID);

            foreach (var cell in playerBattleState.Filed.Cells)
            {
                var monobeh = SpawnCard(cell, BattleObjects.Field);
                cell.GameObject = monobeh;
                monobeh.CardState = cell;
            }

            foreach (var item in playerBattleState.Inventory.Items)
            {
                var monobeh = SpawnCard(item, BattleObjects.Inventory);
                item.GameObject = monobeh;
                monobeh.CardState = item;
            }
        }

        void LoadSave()
        {
            _CommonState = ES3.Load<PlayerCommonState>(SaveName);

            if (_CommonState == null)
            {
                DebugSystem.DebugLog("Save can't be load. New save active.", DebugSystem.Type.SaveSystem);
                _CommonState = new PlayerCommonState();
            }
            else
            {
                DebugSystem.DebugLog("Loaded exist save", DebugSystem.Type.SaveSystem);
            }

            LoadUI();

            MenuAudioSource.volume = _CommonState.Volume;
            BattleAudioSource.volume = _CommonState.Volume;
        }

        void StartNewBattle(int levelID)
        {
            DebugSystem.DebugLog("Start new battle", DebugSystem.Type.Battle);
            StopAllCoroutines();
            DOTween.KillAll();
            
            _inputActive = true;
            _CommonState.InBattle = true;
            _CommonState.BattleState.LevelID = levelID;
            _CommonState.BattleState.Score = 0;
            
            ActiveBattleUI();

            LoadLevelCards(levelID);

            if (levelID < BattleState.CommonLevelID)
            {
                //Spawn new filed
                SpawnRandomField(out _CommonState.BattleState.Filed.Cells, CardGrid.Field, CreateNewRandomCard);

                //Spawn new inventory
                SpawnRandomField(out _CommonState.BattleState.Inventory.Items, CardGrid.Inventory, CreateNewRandomItem);
            }
            else
            {
                SpawnField(out _CommonState.BattleState.Filed.Cells, CardGrid.Field, _loadedMap);
                SpawnInventory(out _CommonState.BattleState.Inventory.Items, CardGrid.Inventory, _loadedInventory);
            }

            void SpawnRandomField(out CardState[,] cards, CardGrid gridType, Func<CardState> createCard)
            {
                GridGameObject gridGameObject;
                if (gridType == CardGrid.Field)
                {
                    gridGameObject = BattleObjects.Field;
                }
                else
                {
                    gridGameObject = BattleObjects.Inventory;
                }

                cards = new CardState[gridGameObject.SizeX, gridGameObject.SizeZ];
                for (int z = 0; z < gridGameObject.SizeZ; z++)
                {
                    for (int x = 0; x < gridGameObject.SizeX; x++)
                    {
                        //Get card
                        var cell = createCard();
                        cell.Grid = gridType;
                        cell.Position = new Vector2Int(x, z);
                        cards[x, z] = cell;
                        var monobeh = SpawnCard(cell, gridGameObject);
                        cell.GameObject = monobeh;
                        monobeh.CardState = cell;
                    }
                }
            }

            void SpawnField(out CardState[,] cards, CardGrid gridType, List<Queue<CardState>> mapField)
            {
                GridGameObject gridGameObject;
                if (gridType == CardGrid.Field)
                {
                    gridGameObject = BattleObjects.Field;
                }
                else
                {
                    gridGameObject = BattleObjects.Inventory;
                }

                cards = new CardState[gridGameObject.SizeX, gridGameObject.SizeZ];
                
                cards = new CardState[gridGameObject.SizeX, gridGameObject.SizeZ];
                for (int z = gridGameObject.SizeZ - 1; z >= 0; z--)
                {
                    for (int x = 0; x < gridGameObject.SizeX; x++)
                    {
                        //Get card
                        CardState cell;
                        if (x < mapField.Count && mapField[x].Count > 0 && mapField[x].Peek() != null)
                        {
                            var card = mapField[x].Dequeue();
                            cell = card;
                        }
                        else
                        {
                            cell = new CardState();
                        }
                        
                        cell.Grid = gridType;
                        cell.Position = new Vector2Int(x, z);
                        cards[x, z] = cell;
                        var monobeh = SpawnCard(cell, gridGameObject);
                        cell.GameObject = monobeh;
                        monobeh.CardState = cell;
                    }
                }
            }
            
            void SpawnInventory(out CardState[,] cards, CardGrid gridType, Queue<CardState> mapField)
            {
                GridGameObject gridGameObject;
                if (gridType == CardGrid.Field)
                {
                    gridGameObject = BattleObjects.Field;
                }
                else
                {
                    gridGameObject = BattleObjects.Inventory;
                }

                cards = new CardState[gridGameObject.SizeX, gridGameObject.SizeZ];
                
                cards = new CardState[gridGameObject.SizeX, gridGameObject.SizeZ];
                for (int z = 0; z < gridGameObject.SizeZ; z++)
                {
                    for (int x = 0; x < gridGameObject.SizeX; x++)
                    {
                        CardState cell;
                        if (mapField.Count > 0)
                        {
                            var card = mapField.Dequeue();
                            cell = card;
                        }
                        else
                        {
                            cell = new CardState();
                        }
                        
                        //Get card
                        cell.Grid = gridType;
                        cell.Position = new Vector2Int(x, z);
                        cards[x, z] = cell;
                        var monobeh = SpawnCard(cell, gridGameObject);
                        cell.GameObject = monobeh;
                        monobeh.CardState = cell;
                    }
                }
            }
        }

        void ChangeLanguage(int language)
        {
            switch (language)
            {
                case 0:
                    _CommonState.Language = Language.English;
                    break;
                case 1:
                    _CommonState.Language = Language.Russian;
                    break;
            }
        }

        void OnApplicationQuit()
        {
            Save();
        }

        void Save()
        {
            //DebugSystem.DebugLog("Save on pause/out", DebugSystem.Type.SaveSystem);
            //Debug.Log(_CommonState);
            //ES3.Save(SaveName, _CommonState);
        }

        #if UNITY_EDITOR
        
        [MenuItem("CardGrid/DeleteSave")]
        public static void DeleteSave()
        {
            ES3.DeleteFile();
        }
        
        #endif
    }
}