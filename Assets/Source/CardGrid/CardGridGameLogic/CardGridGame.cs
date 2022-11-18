using System;
using System.Collections;
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
        public LocalizationSystem Localization;
        public Tutorials Tutorials;
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
        public AudioClip QuestComplete;
        public AudioClip LevelUpSound;
        public AudioClip WinSound;
        public AudioClip DefeateSound;
        public AudioClip ColorSound;
        public AudioClip ShapeSound;
        public AudioClip CompleteAchievementSound;

        const string SaveName = "CardGrid";
        const string CLASSICMODE = "CLASSICMODE";
        const string ONLYWITHCOLOR = "ONLYWITHCOLOR";

        Action PlayerClick;
        int _startMaxCellQuantity;
        float StandardChanceItemOnField = 0.1f;
        float _chanceItemOnFiled;
        PlayerCommonState _CommonState;
        List<CardGameObject> _cardMonobehsPool;
        Camera _camera;
        bool WithShape = false;
        bool WithQuantity = true;
        List<ColorType> ColorTypes = new(5);
        int reawardForCompleteTask = 10;

        void Awake()
        {
            Application.targetFrameRate = 30;
            _camera = Camera.main;
            DebugSystem.Settings = CurrentGameSeetings.Debug;

            int length = BattleObjects.Inventory.SizeX * BattleObjects.Inventory.SizeZ
                         + BattleObjects.Field.SizeX * BattleObjects.Field.SizeZ;
            _cardMonobehsPool = new List<CardGameObject>(length);

#if UNITY_WEBPLAYER
            string redirectUrl = "https://yandex.ru/games";
            string[] domains = { "https://yandex.ru"};
		    string jsarray = "[";
		    foreach (string domain in domains) {
			    jsarray += "'"+domain+"',";
		}
		jsarray += "]";
		Application.ExternalEval("function contains(a, obj) { " +
			                       "var i = a.length; " +
			                       "while (i--) { if (a[i] === obj) { return true;}} " +
			                       "return false;" +
			                      "} "+
			                     "if(contains("+jsarray+", document.location.host)) {} else { document.location='"+redirectUrl+"'; }");
#endif

            foreach (var group in CommonLevelsGroups)
            {
                foreach (var level in group.Levels)
                {
                    if (level != null)
                        level.Init();
                }
            }

            Fade.gameObject.SetActive(true);

            GenerateColorTypesList();
        }

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Yandex.LoadFromYandex();
#endif

#if UNITY_EDITOR
            Load(null);
#endif
        }

        //Yandex
        public void Load(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _CommonState = null;
            }
            else
            {
                DebugSystem.DebugLog("LoadedSave.", DebugSystem.Type.SaveSystem);
                _CommonState = JsonUtility.FromJson<PlayerCommonState>(value);
            }

            if (_CommonState == null)
            {
                _CommonState = new PlayerCommonState();
#if UNITY_WEBGL && !UNITY_EDITOR
                switch (Yandex.GetLang())
                {
                    case "ru":
                        _CommonState.Language = Language.Russian;
                        break;
                    case "be":
                        _CommonState.Language = Language.Russian;
                        break;
                    case "kk":
                        _CommonState.Language = Language.Russian;
                        break;
                    case "uk":
                        _CommonState.Language = Language.Russian;
                        break;
                    case "uz":
                        _CommonState.Language = Language.Russian;
                        break;
                    
                    case "tr":
                        _CommonState.Language = Language.Turk;
                        break;
                    
                    default:
                        _CommonState.Language = Language.English;
                        break;
                }
#endif
                int quantityLevels = 0;

                var levels = (Level[]) typeof(LevelsMaps).GetField("Levels").GetValue(null);
                quantityLevels += levels.Length;

                _CommonState.Levels = new LevelState[quantityLevels];

                int levelIndex = 0;
                for (int i = 0; i < levels.Length; i++)
                {
                    _CommonState.Levels[levelIndex] = new LevelState();
                    _CommonState.Levels[levelIndex].Group = i;

                    var level = levels[i];
                    _CommonState.Levels[levelIndex].CollectColors = level.CollectColors;
                    _CommonState.Levels[levelIndex].NeedSpawnNewRandom = level.NeedSpawnNewRandom;

                    levelIndex++;
                }

                LoadAchievementsStates();
                StartNewBattle(BattleState.CommonLevelID);
                DebugSystem.DebugLog("Save no exist. First active.", DebugSystem.Type.SaveSystem);
            }
            else
            {
                OpenMainMenu();
            }
            
            MenuAudioSource.volume = _CommonState.Volume;
            BattleAudioSource.volume = _CommonState.Volume;
            
            LoadUI();
            SubscribeOnButtons();
            UpdateCommonLocalization();
            
            Fade.CrossFadeAlpha(0 , 1f, false);
            StartCoroutine(FadeOff());
            IEnumerator FadeOff()
            {
                yield return new WaitForSeconds(1f);
                Fade.gameObject.SetActive(false);
            }
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

        void StartNewBattle(int levelID)
        {
            DebugSystem.DebugLog("Start new battle", DebugSystem.Type.Battle);
            StopAllCoroutines();
            DOTween.KillAll();
            
            rewardsLeft = 2;
            _inputActive = true;
            _CommonState.InBattle = true;
            _CommonState.BattleState.LevelID = levelID;
            _CommonState.BattleState.LevelProgress = 0;
            _CommonState.BattleState.NumberLevel = 1;
            _CommonState.BattleState.MaxLevelProgress = 10;
            BattleObjects.FieldRotator.eulerAngles = Vector3.zero;
            for (int i = 0; i < BattleObjects.Field.transform.childCount; i++)
            {
                BattleObjects.Field.transform.GetChild(i).eulerAngles = Vector3.zero;
            }

            ActiveBattleUI();
            LoadLevelCards(levelID);

            if (levelID < BattleState.CommonLevelID)
            {
                if (!WithQuantity && _CommonState.FLClassic)
                {
                    _CommonState.FLClassic = false;
                    ActivateTextTutor();
                }
                else if (WithQuantity && _CommonState.FLQuantity)
                {
                    _CommonState.FLQuantity = false;
                    ActivateTextTutor();
                }

                //Spawn new filed
                SpawnRandomField(out _CommonState.BattleState.Filed.Cells, CardGrid.Field, CreateNewRandomCard, false);

                //Spawn new inventory
                SpawnRandomField(out _CommonState.BattleState.Inventory.Items, CardGrid.Inventory, CreateNewRandomItem,
                    true);
            }
            else
            {
                SpawnField(out _CommonState.BattleState.Filed.Cells, CardGrid.Field, _loadedMap);
                SpawnInventory(out _CommonState.BattleState.Inventory.Items, CardGrid.Inventory, _loadedInventory);
            }

            void SpawnRandomField(out CardState[,] cards, CardGrid gridType, Func<CardState> createCard, bool half)
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
                        CardState cell;
                        if (half && z > 0)
                        {
                            cell = new CardState();
                        }
                        else
                        {
                            cell = createCard();
                            if (x - 1 >= 0 && cards[x - 1, z] != null && cards[x - 1, z].CardSO == cell.CardSO)
                            {
                                cell = createCard();
                            }
                            else if (z - 1 >= 0 && cards[x, z - 1] != null &&  cards[x,z -1].CardSO == cell.CardSO)
                            {
                                cell = createCard();
                            }
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
                case 2:
                    _CommonState.Language = Language.Turk;
                    break;
            }

            UpdateCommonLocalization();
            UpdateAchievementsLanguage();
        }

        void UpdateCommonLocalization()
        {
            Update(Localization.Texts1);
            Update(Localization.Texts2);
            Update(Localization.Texts3);

            void Update(LocText[] loctexts)
            {
                foreach (var loctext in loctexts)
                {
                    SetText(loctext);
                }
            }

            void SetText(LocText loctext)
            {
                foreach (var loc in loctext.Localizations)
                {
                    if (loc.Language == _CommonState.Language)
                    {
                        loctext.View.text = loc.Text;
                        return;
                    }
                }

                DebugSystem.DebugLog($"NOLOC {loctext.View.gameObject.name}, FOR {_CommonState.Language} LAN",
                    DebugSystem.Type.Error);
            }
        }

        void OnApplicationQuit()
        {
            Save();
        }

        private bool _rewarded;
        //Yandex
        public void Rewarded()
        {
            _rewarded = true;
        }

        public void AdRewardClose()
        {
            if (_rewarded)
                StartCoroutine(AddRewardedItems());
            else
                NotRewardPlayer();
        }

        //Yandex
        public void NotRewardPlayer()
        {
            BattleUI.EndItemsReward.gameObject.SetActive(false);
            OpenDefeat(BattleUI.BattleMenu);
        }

        private float SaveReload = 10f;
        float _saveTimer;
        void Save()
        {
            if(_saveTimer < SaveReload) return;
            _saveTimer = 0;
#if UNITY_WEBGL && !UNITY_EDITOR
            string jString = JsonUtility.ToJson(_CommonState);
            Yandex.SaveOnYandex(jString);
#endif
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