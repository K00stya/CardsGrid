using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CardGrid
{
    /*
     * In this partial Loading/Creating/ReCreate cards;
     */
    public partial class CardGridGame //CardsFactory
    {
        List<Queue<CardState>> _loadedMap;
        Queue<CardState> _loadedInventory;

        List<CardSO> _loadedEnemies;
        List<CardSO> _loadedItems;
        
        /*
         * I could not find a way to load addressable arrays, this shit is taken from official examples:
         * https://github.com/Unity-Technologies/Addressables-Sample/blob/master/Basic/Basic%20AssetReference/Assets/Scripts/ListOfReferences.cs
         */
        IEnumerator LoadLevelCards(int levelID)
        {
            _impactHighlightCards.Clear();
            
            if (levelID < BattleState.CommonLevelID)
            {
                _loadedEnemies = new List<CardSO>();
                _loadedItems = new List<CardSO>();
                
                //off tutor
                TutorHandObj.transform.DOKill();
                TutorHandObj.SetActive(false);
                _CommonState.CurrentTutorial = null;
                
                var loadedLevel = InfiniteLevels[levelID];
                _startMaxCellQuantity = loadedLevel.StartMaxCellQuantity;
                _chanceItemOnFiled = loadedLevel.ChanceItemOnFiled;

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
                
                BattleUI.Score.gameObject.SetActive(true);
                BattleUI.LeftCardsPanel.gameObject.SetActive(false);
            }
            else
            {
                var id = levelID - BattleState.CommonLevelID;
                var level = _CommonState.Levels[id];
                var loadedLevel = CommonLevelsGroups[level.Group].Levels[level.IdInGroup];
                LoadTutor(loadedLevel);
                var columnsSO = loadedLevel.Columns;

                _loadedMap = new List<Queue<CardState>>(columnsSO.Count);

                for (int i = 0; i < loadedLevel.Columns.Count; i++)
                {
                    var row = columnsSO[i];
                    _loadedMap.Add(new Queue<CardState>(row.Length)); //AddEmptyRow
                }

                CardState[][] fieldCadsHandler = new CardState[columnsSO.Count][];
                int quantityCards = 0;
                int X = -1;
                foreach (var row in columnsSO)
                {
                    X++;
                    int Z = -1;
                    fieldCadsHandler[X] = new CardState[row.Length];
                    foreach (var card in row)
                    {
                        Z++;
                        if (card == null) continue;

                        quantityCards++;

                        SubField(card, X, Z);
                    }
                }

                _loadedInventory = new Queue<CardState>(loadedLevel.Inventory.Length);
                CardState[] inventoryCardsHandler = new CardState[loadedLevel.Inventory.Length];
                int items = 0;
                foreach (var card in loadedLevel.Inventory)
                {
                    SubInventory(card, items);
                    items++;
                }

                yield return new WaitUntil(() => quantityCards <= 0 && items <= 0);

                for (int i = 0; i < _loadedMap.Count; i++)
                {
                    foreach (var card in fieldCadsHandler[i])
                    {
                        _loadedMap[i].Enqueue(card);
                    }

                    if (_loadedMap[i].Count > BattleObjects.Field.SizeZ)
                        BattleUI.LeftCardsPanel.Columns[i].text = (_loadedMap[i].Count - BattleObjects.Field.SizeZ).ToString();
                    else
                        BattleUI.LeftCardsPanel.Columns[i].text = "0";
                }

                for (int i = 0; i < inventoryCardsHandler.Length; i++)
                {
                    _loadedInventory.Enqueue(inventoryCardsHandler[i]);
                }

                void SubInventory(CardStartInfo card, int index)
                {
                    card.Card.LoadAssetAsync<CardSO>().Completed += (obj) =>
                    {
                        OnInventoryCardLoaded(obj, card.Quantity, index);
                    };
                }

                void SubField(CardStartInfo card, int X, int Z)
                {
                    card.Card.LoadAssetAsync<CardSO>().Completed += (obj) =>
                    {
                        OnFieldCardLoaded(obj, card.Quantity, X, Z);
                    };
                }
                
                BattleUI.Score.gameObject.SetActive(false);
                BattleUI.LeftCardsPanel.gameObject.SetActive(true);
                
                void OnFieldCardLoaded(AsyncOperationHandle<CardSO> obj, int quantity, int column, int row)
                {
                    var card = obj.Result;
                    var cardState = new CardState();
                    cardState.CardSO = card;
                    cardState.CardSO.Type = card.Type;
                    cardState.Quantity = quantity;
                    cardState.StartQuantity = quantity;
                    
                    fieldCadsHandler[column][row] = cardState;
                    Addressables.Release(obj);
                    quantityCards--;
                }

                void OnInventoryCardLoaded(AsyncOperationHandle<CardSO> obj, int quantity, int index)
                {
                    var card = obj.Result;
                    var cardState = new CardState();
                    cardState.CardSO = card;
                    cardState.CardSO.Type = card.Type;
                    cardState.Quantity = quantity;
                    cardState.StartQuantity = quantity;
                    
                    inventoryCardsHandler[index] = cardState;
                    Addressables.Release(obj);
                    items--;
                }
            }
        }

        private void LoadTutor(LevelSO loadedLevel)
        {
            _handMoving = false;
            TutorHandObj.SetActive(false);
            TutorHandObj.transform.DOKill();
            if (loadedLevel.TutorSequence != null)
            {
                _CommonState.CurrentTutorial = new TutorialSequence()
                {
                    Cards = new List<TutorCardInfo>(loadedLevel.TutorSequence.Cards)
                };
            }
            else
            {
                _CommonState.CurrentTutorial = null;
            }
        }

        void DestroyCards()
        {
            foreach (var monobeh in _cardMonobehsPool.ToArray())
            {
                Destroy(monobeh.gameObject);
            }
            _cardMonobehsPool.Clear();
        }

        void DestroyAndUnloadCards()
        {
            foreach (var monobeh in _cardMonobehsPool.ToArray())
            {
                Destroy(monobeh.gameObject);
            }
            _cardMonobehsPool.Clear();
            
            _loadedEnemies = null;
            _loadedItems = null;
            _loadedMap = null;
            _loadedInventory = null;
        }

        CardState CreateNewRandomCard()
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

            return CreateCard(newCard);
        }
        
        CardState CreateCard(CardSO newCard)
        {
            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            return new CardState {
                CardSO = newCard,
                Quantity = quantity,
                StartQuantity = quantity};
        }

        CardState CreateCard(CardSO newCard, int quantity)
        {
            return new CardState {
                CardSO = newCard,
                Quantity = quantity,
                StartQuantity = quantity};
        }

        CardState CreateNewRandomItem()
        {
            var card = new CardState
            {
                CardSO = _loadedItems[Random.Range(0, _loadedItems.Count)],
                Quantity = Random.Range(1, _startMaxCellQuantity)
            };
            
            return card;
        }

        void ReCreateCard(ref CardState cardState, int row)
        {
            CardSO newCard;
            if (_CommonState.BattleState.LevelID < BattleState.CommonLevelID)
            {
                if (Random.Range(0, 1f) > _chanceItemOnFiled)
                {
                    newCard = _loadedEnemies[Random.Range(0, _loadedEnemies.Count)];
                }
                else
                {
                    newCard = _loadedItems[Random.Range(0, _loadedItems.Count)];
                }

                if (newCard != null)
                {
                    int quantity = 0;
                    quantity = Random.Range(1, _startMaxCellQuantity + 1);

                    cardState.CardSO = newCard;
                    cardState.Quantity = quantity;
                    cardState.StartQuantity = quantity;
                    cardState.GameObject.gameObject.SetActive(true);
                    SetCommonCardState(ref cardState, ref cardState.GameObject, newCard);
                }
            }
            else
            {
                if (_loadedMap[row].Count > 0)
                {
                    CardState loadedCardState = _loadedMap[row].Dequeue();
                    loadedCardState.GameObject = cardState.GameObject;
                    loadedCardState.Grid = cardState.Grid;
                    loadedCardState.Position = cardState.Position;
                    
                    cardState = loadedCardState;
                    cardState.GameObject.CardState = cardState;
                    newCard = cardState.CardSO;

                    cardState.GameObject.gameObject.SetActive(true);
                    SetCommonCardState(ref cardState, ref cardState.GameObject, newCard);

                    BattleUI.LeftCardsPanel.Columns[row].text = _loadedMap[row].Count.ToString();
                }
                else
                {
                    BattleUI.LeftCardsPanel.Columns[row].text = 0.ToString();
                }
            }
        }

        void SetCommonCardState(ref CardState cardState, ref CardGameObject cardGameObject, CardSO cardInfo)
        {
            cardGameObject.Sprite.sprite = cardInfo.Sprite;
            if (cardState.CardSO.Type == TypeCard.Block)
            {
                cardState.Quantity = int.MaxValue;
                cardGameObject.QuantityText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                cardGameObject.QuantityText.text = cardState.Quantity.ToString();
            }
            //cardGameObject.DebugPosition.text = cardState.Position.x.ToString() + cardState.Position.y.ToString();
        }

        CardGameObject SpawnCard(CardState cardState, GridGameObject grid)
        {
            CardGameObject cardGameObject = Instantiate(BattleObjects.CardPrefab, grid.transform);
            
            cardGameObject.transform.position = grid.GetCellSpacePosition(cardState.Position);
            if (cardState.Quantity <= 0)
            {
                cardGameObject.gameObject.SetActive(false);
            }
            if(cardState.CardSO != null)
                SetCommonCardState(ref cardState, ref cardGameObject, cardState.CardSO);
            _cardMonobehsPool.Add(cardGameObject);

            return cardGameObject;
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
            if (_loadedMap != null)
            {
                foreach (var loadedColumn in _loadedMap)
                {
                    foreach (var card in loadedColumn)
                    {
                        if (card.CardSO.Name == name)
                        {
                            //return card;
                        }
                    }
                }
            }
            
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
    }
}