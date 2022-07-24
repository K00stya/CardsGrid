using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CardGrid
{
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
        public MenuUI MenuUI;
        public BattleUI BattleUI;
        public CommonGameSettings CurrentGameSeetings;
        public AudioSource AudioSource;
        public LevelSO[] ActiveLevels;

        const string SaveName = "CardGrid";

        int _startMaxCellQuantity;
        float _chanceItemOnFiled;
        PlayerCommonState _CommonState;
        List<CardGameObject> _cardMonobehsPool;
        Button[] LevelsButtons;
        Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
            DebugSystem.Settings = CurrentGameSeetings.Debug;

            int length = BattleObjects.Inventory.SizeX * BattleObjects.Inventory.SizeZ
                         + BattleObjects.Field.SizeX * BattleObjects.Field.SizeZ;
            _cardMonobehsPool = new List<CardGameObject>(length);
        }

        void Start()
        {
            SubscribeOnButtons();
            
            //Try load save
            if (ES3.KeyExists(SaveName))
            {
                LoadSave();
            }
            //New save
            else
            {
                _CommonState = new PlayerCommonState();
                DebugSystem.DebugLog("Save no exist. First active.", DebugSystem.Type.SaveSystem);
            }
            
            //OpenMenu
            MenuUI.GameObject.SetActive(true);
        }

        void SubscribeOnButtons()
        {
            MenuUI.VolumeSlider.onValueChanged.AddListener(value => { _CommonState.Volume = value; });
            MenuUI.LanguageDropdown.onValueChanged.AddListener(language => { ChangeLanguage(language); });
            BattleUI.OpenMenu.onClick.AddListener(() => { GoToMenu(); });
            BattleUI.ToMenuOnDeafeat.onClick.AddListener(() => { GoToMenu(); });
            MenuUI.Continue.onClick.AddListener(() =>
            {
                ActiveBattleUI();
                StartCoroutine(LoadBattle());
            });
            MenuUI.NewBattleButton.onClick.AddListener(() =>
            {
                OpenLevelMenu();
            });
            MenuUI.BackToMenu.onClick.AddListener(() =>
            {
                MenuUI.Levels.SetActive(false);
                MenuUI.MainMenu.SetActive(true);
            });

            SetLevelButtons();
        }
        
        IEnumerator LoadBattle()
        {
            BattleState playerBattleState = _CommonState.BattleState;

            yield return StartCoroutine(LoadLevelCards(playerBattleState.LevelID));

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

        void OpenLevelMenu()
        {
            for (int i = 0; i < ActiveLevels.Length; i++)
            {
                if (ActiveLevels[i].Open || _CommonState.BestScore >= ActiveLevels[i].NeedScoreToOpen)
                {
                    LevelsButtons[i] .interactable = true;
                }
                else
                {
                    LevelsButtons[i] .interactable = false;
                }
            }
            
            MenuUI.MainMenu.SetActive(false);
            MenuUI.Levels.SetActive(true);
        }

        void SetLevelButtons()
        {
            LevelsButtons = new Button[ActiveLevels.Length];
            for (int i = 0; i < ActiveLevels.Length; i++)
            {
                var levelID = i;
                var level = ActiveLevels[i];
                var button = Instantiate(MenuUI.StartLevel, MenuUI.Levels.transform);
                LevelsButtons[i] = button;
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text =
                    !level.Open ? $"{level.LevelName} ({level.NeedScoreToOpen})" : level.LevelName;
                
                button.onClick.AddListener(() =>
                    StartCoroutine(StartNewBattle(levelID)));
            }
        }

        void LoadSave()
        {
            _CommonState = ES3.Load<PlayerCommonState>(SaveName);
            if (_CommonState == null)
            {
                DebugSystem.DebugLog("Save can't be load. New save active.", DebugSystem.Type.SaveSystem);
                _CommonState = new PlayerCommonState();
                MenuUI.Continue.gameObject.SetActive(false);
            }
            else
            {
                DebugSystem.DebugLog("Loaded exist save", DebugSystem.Type.SaveSystem);
                MenuUI.Continue.gameObject.SetActive(_CommonState.InBattle);
            }

            AudioSource.volume = _CommonState.Volume;
            MenuUI.VolumeSlider.SetValueWithoutNotify(_CommonState.Volume);
            MenuUI.BestScore.text = _CommonState.BestScore.ToString();
            MenuUI.LanguageDropdown.SetValueWithoutNotify((int) _CommonState.Language);
        }

        IEnumerator StartNewBattle(int levelID)
        {
            DebugSystem.DebugLog("Start new battle", DebugSystem.Type.Battle);
            
            _inputActive = true;
            _CommonState.InBattle = true;
            _CommonState.BattleState.LevelID = levelID;
            _CommonState.BattleState.Score = 0;
            
            ActiveBattleUI();

            yield return StartCoroutine(LoadLevelCards(levelID));

            //Spawn new filed
            Spawn(out _CommonState.BattleState.Filed.Cells, CardGrid.Field, CreateNewRandomCard);

            //Spawn new inventory
            Spawn(out _CommonState.BattleState.Inventory.Items, CardGrid.Inventory, CreateNewRandomItem);

            void Spawn(out Card[,] cards, CardGrid gridType, Func<Card> createCard)
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

                cards = new Card[gridGameObject.SizeX, gridGameObject.SizeZ];
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
        }

        void ActiveBattleUI()
        {
            MenuUI.Levels.SetActive(false);
            MenuUI.GameObject.SetActive(false);
            BattleUI.Defeat.SetActive(false);

            BattleUI.Score.text = _CommonState.BattleState.Score.ToString();
            BattleUI.GameObject.SetActive(true);
        }

        void GoToMenu()
        {
            if (!_inputActive) return;
            Save();
            MenuUI.Continue.gameObject.SetActive(_CommonState.InBattle);
            BattleUI.ToMenuOnDeafeat.gameObject.SetActive(false);
            BattleUI.GameObject.SetActive(false);
            MenuUI.GameObject.SetActive(true);
            MenuUI.MainMenu.SetActive(true);
            MenuUI.BestScore.text = _CommonState.BestScore.ToString();
            UnLoadCards();
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
            DebugSystem.DebugLog("Save on pause/out", DebugSystem.Type.SaveSystem);
            Debug.Log(_CommonState);
            ES3.Save(SaveName, _CommonState);
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