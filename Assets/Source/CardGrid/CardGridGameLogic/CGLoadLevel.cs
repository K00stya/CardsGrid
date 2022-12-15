using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGrid
{
    public partial class CardGridGame
    {
        private void Load(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "null")
            {
                _CommonState = null;
            }
            else
            {
                DebugSystem.DebugLog("LoadedSave.", DebugSystem.Type.SaveSystem);
                try
                {
                    _CommonState = JsonUtility.FromJson<PlayerCommonState>(value);
                }
                catch (Exception e)
                {
                    _CommonState = null;
                }
            }

            LevelSO[] levels = new LevelSO[QuantityLevels()];
            LoadLevelsSO(levels);

            if (_CommonState == null)
            {
                _CommonState = new PlayerCommonState();
                _CommonState.Levels = new LevelState[QuantityLevels()];
                for (int i = 0; i < levels.Length; i++)
                {
                    _CommonState.Levels[i] = CreateLevelState(levels[i]);
                }

                LoadLanguage();
                LoadAchievementsStates();
                StartNewBattle(0); //0
                DebugSystem.DebugLog("Save no exist. First active.", DebugSystem.Type.SaveSystem);
            }
            else if (_CommonState.Levels.Length < QuantityLevels())
            {
                LoadGameResave(levels);
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

            Fade.CrossFadeAlpha(0, 1f, false);
            StartCoroutine(FadeOff());
            LoadPools();

            IEnumerator FadeOff()
            {
                yield return new WaitForSeconds(1f);
                Fade.gameObject.SetActive(false);
            }
        }
        
        void LoadExploration(LevelExplorationSO level)
        {
            LoadFieldMap(GetRandomMap(level.Field), level);
            //var inventory =
            
        }
        
        TextAsset GetRandomMap(TextAsset[] maps)
        {
            return maps[Random.Range(0,maps.Length)];
        }

        void LoadFieldMap(TextAsset field, LevelSO level)
        {
            var fieldData = JsonUtility.FromJson<MapData>(field.text);
            _CommonState.BattleState.Filed.Cells = SpawnFiled(fieldData.Cards, level);
        }

        CardState[,] SpawnFiled(CT[] typesMap, LevelSO levelSO)
        {
            GridGameObject gridGameObject = BattleObjects.Field;
            CardGrid gridType = CardGrid.Field;

            int i = 0;
            CardState[,] cards = new CardState[gridGameObject.SizeX, gridGameObject.SizeZ];
            for (int y = 0; y < gridGameObject.SizeZ; y++)
            {
                for (int x = 0; x < gridGameObject.SizeX; x++, i++)
                {
                    CardState cardState = new CardState();

                    if (typesMap[i] == CT.Empty)
                    {
                        if (levelSO.Cards.Length > 0)
                        {
                            cardState.ScrObj = levelSO.Cards[Random.Range(0, levelSO.Cards.Length)];
                        }
                        else
                            continue;
                    }
                    else
                    {
                        cardState.ScrObj = GetCardSO(typesMap[i]);
                    }

                    cardState.Grid = gridType;
                    cardState.Position = new Vector2Int(x, y);
                    cards[x, y] = cardState;
                    var monobeh = SpawnCard(cardState, gridGameObject);
                    cardState.GameObject = monobeh;
                    monobeh.CardState = cardState;
                }
            }

            return cards;
        }

        #region MyRegion
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
                        if (x - 1 >= 0 && cards[x - 1, z] != null && cards[x - 1, z].ScrObj == cell.ScrObj)
                        {
                            cell = createCard();
                        }
                        else if (z - 1 >= 0 && cards[x, z - 1] != null && cards[x, z - 1].ScrObj == cell.ScrObj)
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
#endregion

        void LoadLevelsSO(LevelSO[] loaded)
        {
            int i = 0;
            LoadArray(Levels.Exploration);
            LoadArray(Levels.ResourcesExtraction);
            LoadArray(Levels.Battle);

            void LoadArray(LevelSO[] levels)
            {
                for (int j = 0; j < levels.Length; j++, i++)
                {
                    loaded[i] = levels[i];
                    loaded[i].ArrayTypeIndex = j;
                }
            }
        }

        int QuantityLevels()
        {
            int quantity = Levels.Exploration.Length;
            quantity += Levels.ResourcesExtraction.Length;
            quantity += Levels.Battle.Length;
            return quantity;
        }

        void LoadGameResave(LevelSO[] levelsSO)
        {
            var levels = _CommonState.Levels;
            LoadLanguage();

            if (_CommonState.Achievements.Length == 0)
            {
                _CommonState.Achievements = new AchieveState[AchievementsSO.Length];
            }
            else
            {
                AchieveState[] l = new AchieveState[AchievementsSO.Length];
                for (int i = 0; i < _CommonState.Achievements.Length; i++)
                {
                    l[i] = _CommonState.Achievements[i];
                }

                _CommonState.Achievements = l;
            }

            for (int i = 0; i < _CommonState.Achievements.Length; i++)
            {
                if (_CommonState.Achievements[i] == null)
                    _CommonState.Achievements[i] = new AchieveState()
                    {
                        Key = AchievementsSO[i].name,
                        Level = 0,
                        MaxProgress = AchievementsSO[i].Levels[0].MaxProgress,
                        Reward = AchievementsSO[i].Levels[0].Reward,
                    };
            }

            bool firstLevel = false;
            if (_CommonState.Levels.Length == 0)
            {
                firstLevel = true;
                _CommonState.Levels = new LevelState[levels.Length];
            }
            else
            {
                LevelState[] l = new LevelState[levels.Length];
                for (int i = 0; i < _CommonState.Levels.Length; i++)
                {
                    l[i] = _CommonState.Levels[i];
                }

                _CommonState.Levels = l;
            }

            int levelIndex = 0;
            for (int i = 0; i < levels.Length; i++)
            {
                if (_CommonState.Levels[levelIndex] == null)
                {
                    _CommonState.Levels[levelIndex] = CreateLevelState(levelsSO[i]);
                }

                levelIndex++;
            }

            if (firstLevel)
            {
                StartNewBattle(0);
            }
            else
            {
                OpenMainMenu();
            }
        }

        LevelState CreateLevelState(LevelSO level)
        {
            LevelType type = default;
            if (level is LevelExplorationSO)
            {
                type = LevelType.Exploration;
            }
            else if (level is LevelExtractionSO)
            {
                type = LevelType.Extraction;
            }
            else if (level is LevelBattleSO)
            {
                type = LevelType.Battle;
            }

            return new LevelState()
            {
                Type = type,
                ScrObj = level
            };
        }
    }
}
