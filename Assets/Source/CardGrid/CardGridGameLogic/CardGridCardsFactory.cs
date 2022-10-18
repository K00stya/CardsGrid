using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        void LoadLevelCards(int levelID)
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

                foreach (var card in loadedLevel.Enemies)
                {
                    _loadedEnemies.Add(card);
                }

                foreach (var card in loadedLevel.Items)
                {
                    _loadedItems.Add(card);
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
                
                int X = 0;
                foreach (var row in columnsSO)
                {
                    foreach (var card in row)
                    {
                        if (card == null) continue;
                        
                        var cardSO = card.Card;
                        var cardState = new CardState();
                        cardState.CardSO = cardSO;
                        cardState.CardSO.Type =cardSO.Type;
                        cardState.Quantity = card.Quantity;
                        cardState.StartQuantity = card.Quantity;
                    
                        _loadedMap[X].Enqueue(cardState);
                    }
                    if (_loadedMap[X].Count > BattleObjects.Field.SizeZ)
                        BattleUI.LeftCardsPanel.Columns[X].text = (_loadedMap[X].Count - BattleObjects.Field.SizeZ).ToString();
                    else
                        BattleUI.LeftCardsPanel.Columns[X].text = "0";
                    
                    X++;
                }

                _loadedInventory = new Queue<CardState>(loadedLevel.Inventory.Length);
                foreach (var card in loadedLevel.Inventory)
                {
                    var cardSO = card.Card;
                    var cardState = new CardState();
                    cardState.CardSO = cardSO;
                    cardState.CardSO.Type = cardSO.Type;
                    cardState.Quantity = card.Quantity;
                    cardState.StartQuantity = card.Quantity;
                    
                    _loadedInventory.Enqueue(cardState);
                }
                
                BattleUI.Score.gameObject.SetActive(false);
                BattleUI.LeftCardsPanel.gameObject.SetActive(true);
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