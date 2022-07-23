using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace CardGrid
{
    public partial class CardGridGame : MonoBehaviour
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
                MenuUI.GameObject.SetActive(false);
                StartCoroutine(LoadBattle());
            });
            MenuUI.NewBattleButton.onClick.AddListener(() => 
            {
                MenuUI.MainMenu.SetActive(false);
                MenuUI.Levels.SetActive(true);
            });
            MenuUI.BackToMenu.onClick.AddListener(() =>
            {
                MenuUI.Levels.SetActive(false);
                MenuUI.MainMenu.SetActive(true);
            });
            
            for (int i = 0; i < ActiveLevels.Length; i++)
            {
                var levelID = i;
                var level = ActiveLevels[i];
                var button = Instantiate(MenuUI.StartLevel, MenuUI.Levels.transform);
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = level.LevelName;
                button.onClick.AddListener(() =>
                    StartCoroutine(StartNewBattle(levelID)));

                button.interactable = level.Open;
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
            
            ActiveBattleUI();
            
            _inputActive = true;
            _CommonState.InBattle = true;
            _CommonState.BattleState.LevelID = levelID;

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
            BattleUI.GameObject.SetActive(true);
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

        /*
         * I could not find a way to load addressable arrays, this shit is taken from official examples:
         * https://github.com/Unity-Technologies/Addressables-Sample/blob/master/Basic/Basic%20AssetReference/Assets/Scripts/ListOfReferences.cs
         */
        IEnumerator LoadLevelCards(int levelID)
        {
            var loadedLevel = ActiveLevels[levelID];
            _startMaxCellQuantity = loadedLevel.StartMaxCellQuantity;
            _chanceItemOnFiled = loadedLevel.ChanceItemOnFiled;
            
            _loadedEnemies = new List<CardSO>();
            _loadedItems = new List<CardSO>();
            var enemies = loadedLevel.Enemies.Length;
            var items = loadedLevel.Items.Length;
            
            foreach (var card in loadedLevel.Enemies)
            {
                card.LoadAssetAsync<CardSO>().Completed += OnEnemyLoaded;
            }
            
            foreach (var card in loadedLevel.Items)
            {
                card.LoadAssetAsync<CardSO>().Completed += OnItemLoaded;
            }

            yield return new WaitUntil(() => enemies <= 0 && items <= 0);
            
            void OnEnemyLoaded(AsyncOperationHandle<CardSO> obj)
            {
                _loadedEnemies.Add(obj.Result);
                Addressables.Release(obj);
                enemies--;
            }
            
            void OnItemLoaded(AsyncOperationHandle<CardSO> obj)
            {
                _loadedItems.Add(obj.Result);
                Addressables.Release(obj);
                items--;
            }
        }
        
        void UnLoadCards()
        {
            foreach (var monobeh in _cardMonobehsPool.ToArray())
            {
                Destroy(monobeh.gameObject);
            }

            _cardMonobehsPool.Clear();
        }

        Card CreateNewRandomCard()
        {
            CardSO newCard;
            if (Random.Range(0, 1f) > _chanceItemOnFiled)
            {
                newCard = _loadedEnemies[Random.Range(0, _loadedEnemies.Count)];
            }
            else
            {
                return CreateNewRandomItem();
            }

            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            return new Card {name = newCard.Name, Quantity = quantity, StartQuantity = quantity};
        }

        Card CreateNewRandomItem()
        {
            return new Card
            {
                name = _loadedItems[Random.Range(0, _loadedItems.Count)].Name,
                Quantity = Random.Range(1, _startMaxCellQuantity)
            };
        }

        void ReCreateCard(Card card)
        {
            CardSO newCard;
            if (Random.Range(0, 1f) > _chanceItemOnFiled)
            {
                newCard = _loadedEnemies[Random.Range(0, _loadedEnemies.Count)];
            }
            else
            {
                newCard = _loadedItems[Random.Range(0, _loadedItems.Count)];
            }

            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            card.Quantity = quantity;
            card.StartQuantity = quantity;
            card.GameObject.gameObject.SetActive(true);
            SetCommonCardState(card, card.GameObject, newCard);
        }

        CardGameObject SpawnCard(Card card, GridGameObject grid)
        {
            CardSO cardInfo = GetCardSO(card.name);
            CardGameObject cardGameObject = Instantiate(BattleObjects.CardPrefab, grid.transform);
            
            cardGameObject.transform.position = grid.GetCellSpacePosition(card.Position);
            if (card.Quantity <= 0)
            {
                cardGameObject.gameObject.SetActive(false);
            }
            SetCommonCardState(card, cardGameObject, cardInfo);
            _cardMonobehsPool.Add(cardGameObject);

            return cardGameObject;
        }

        void SetCommonCardState(Card card, CardGameObject cardGameObject, CardSO cardInfo)
        {
            card.name = cardInfo.Name;
            card.ImpactMap = cardInfo.ImpactMap;
            card.Effect = cardInfo.Effect;
            
            cardGameObject.Sprite.sprite = cardInfo.Sprite;
            cardGameObject.QuantityText.text = card.Quantity.ToString();
            cardGameObject.DebugPosition.text = card.Position.x.ToString() + card.Position.y.ToString();
        }

        /*
         * Searching among all downloaded cards may not be the best option,
         * but considering that several maps are usually instances of one SO card,
         * this should take up less memory and possibly load faster.
         * But it is impossible to say for sure depends on the weight of sprites, effects, the number of cards,
         * but this in theory works faster than loading sprite and effect for each card separately.
         */
        CardSO GetCardSO(string name)
        {
            foreach (var item in _loadedItems)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }

            foreach (var enemy in _loadedEnemies)
            {
                if (enemy.Name == name)
                {
                    return enemy;
                }
            }

            DebugSystem.DebugLog($"On spawn card SO whit name {name} does not found", DebugSystem.Type.Error);
            return null;
        }

        void GoToMenu()
        {
            if (!_inputActive) return;
            Save();
            MenuUI.Continue.gameObject.SetActive(_CommonState.InBattle);

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