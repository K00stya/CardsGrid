using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Storage;
using UnityEditor;
using UnityEngine;
using YG;

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
        public LocalizationSystem Localization;
        public Tutorials Tutorials;
        public BattleGameObjects BattleObjects;
        public CommonGameSettings GameSetings;
        public LevelsListSO Levels;

        public AudioSource BattleAudioSource;
        public AudioSource MusicAudioSource;
        public AudioSource MenuAudioSource;
        public AudioClip QuestComplete;
        public AudioClip LevelUpSound;
        public AudioClip WinSound;
        public AudioClip DefeateSound;
        public AudioClip ColorSound;
        public AudioClip CompleteAchievementSound;

        const string SaveName = "CardGrid";
        const string CLASSICMODE = "CLASSICMODE";
        const string ONLYWITHCOLOR = "ONLYWITHCOLOR";

        Action PlayerClick;
        int _startMaxCellQuantity = 10;
        float StandardChanceItemOnField = 0.1f;
        float _chanceItemOnFiled;
        PlayerCommonState _CommonState;
        List<CardGameObject> _cardMonobehsPool;
        Camera _camera;
        bool WithQuantity = true;
        List<ColorType> ColorTypes = new(5);
        int reawardForCompleteTask = 10;

        void Awake()
        {
            Application.targetFrameRate = 30;
            _camera = Camera.main;
            DebugSystem.Settings = GameSetings.Debug;

            int length = BattleObjects.Inventory.SizeX * BattleObjects.Inventory.SizeZ
                         + BattleObjects.Field.SizeX * BattleObjects.Field.SizeZ;
            _cardMonobehsPool = new List<CardGameObject>(length);

            Fade.gameObject.SetActive(true);

            GenerateColorTypesList();
        }

        private void Start()
        {
            Bridge.advertisement.ShowInterstitial();
            Bridge.storage.Get("CrystalFight", (success, data) =>
            {
                // if (success)
                // {
                //     Load(data);
                // }
                // else
                {
                    Load(null);
                }
            }, GetStorageType());
        }

        void Update()
        {
            CardMove();
            TutorHand();
            GreyItems();
            if (Input.GetMouseButtonDown(0))
            {
                PlayerClick?.Invoke();
            }

            _saveTimer += Time.deltaTime;
        }

        void LateUpdate()
        {
            UpdateBattlePos();
        }

        private float SaveReload = 5f;
        float _saveTimer;

        void Save(bool timer = true)
        {
            if (_saveTimer < SaveReload && timer) return;

            _saveTimer = 0;
            string jString = JsonUtility.ToJson(_CommonState);
#if !UNITY_EDITOR
            Bridge.storage.Set("CrystalFight", jString, null, GetStorageType());
#endif
        }

        StorageType GetStorageType()
        {
            if (Bridge.player.isAuthorized)
                return StorageType.PlatformInternal;
            else
                return StorageType.LocalStorage;
        }

        void StartNewBattle(int levelID)
        {
            DebugSystem.DebugLog("Start new battle", DebugSystem.Type.Battle);
            StopAllCoroutines();
            DOTween.KillAll();
            _impactHighlightCards.Clear();
            TutorHandObj.transform.DOKill();
            TutorHandObj.SetActive(false);

            rewardsLeft = 2;
            _inputActive = true;
            _CommonState.InBattle = true;
            _CommonState.BattleState.LevelID = levelID;
            BattleObjects.FieldRotator.localEulerAngles = Vector3.zero;
            for (int i = 0; i < BattleObjects.Field.ParentCards.childCount; i++)
            {
                BattleObjects.Field.ParentCards.GetChild(i).localEulerAngles = Vector3.zero;
            }

            var level = _CommonState.Levels[levelID];

            switch (level.Type)
            {
                case LevelType.Exploration:
                    LoadExploration((LevelExplorationSO) level.ScrObj);
                    break;
            }
        }

        void ChangeLanguage(int language)
        {
            switch (language)
            {
                case 0:
                    YandexGame.SwitchLanguage("en");
                    _CommonState.Language = Language.English;
                    break;
                case 1:
                    YandexGame.SwitchLanguage("ru");
                    _CommonState.Language = Language.Russian;
                    break;
                case 2:
                    YandexGame.SwitchLanguage("tr");
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
                        if (loctext.View != null)
                        {
                            loctext.View.text = loc.Text;
                        }
                        else
                        {
                            Debug.LogWarning("NoTextMesh");
                        }

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

        //Yandex
        public void NotRewardPlayer()
        {
            BattleUI.EndItemsReward.gameObject.SetActive(false);
            OpenDefeat();
        }

#if UNITY_EDITOR

        [MenuItem("CardGrid/DeleteSave")]
        public static void DeleteSave()
        {
            ES3.DeleteFile();
        }
#endif

        void LoadLanguage()
        {
            switch (Bridge.platform.language)
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
        }

        CardSO GetCardSO(CT card)
        {
            if (card == CT.Empty)
                return null;

            foreach (var enemy in GameSetings.Enemies)
            {
                if (enemy.CardType == card)
                    return enemy;
            }

            foreach (var item in GameSetings.Items)
            {
                if (item.CardType == card)
                    return item;
            }

            DebugSystem.DebugLog($"On spawn card SO whit name {name} does not found", DebugSystem.Type.Error);
            return null;
        }
    }
}