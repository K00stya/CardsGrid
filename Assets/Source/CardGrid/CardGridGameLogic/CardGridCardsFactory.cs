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
                _CommonState.CurrentTutorial = new List<TutorCardInfo>();
                
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

                for (int i = 0; i < BattleUI.Requires.Length; i++)
                {
                    BattleUI.Requires[i].gameObject.SetActive(false);
                }
                BattleUI.LevelProgress.gameObject.SetActive(true);
                BattleUI.LeftCardsPanel.gameObject.SetActive(false);
            }
            else
            {
                _chanceItemOnFiled = StandardChanceItemOnField;

                var id = levelID - BattleState.CommonLevelID;
                //LoadLevel1(id);
                var level = LoadLevel(id);

                BattleUI.LevelProgress.gameObject.SetActive(false);
                BattleUI.LeftCardsPanel.gameObject.SetActive(!level.NeedSpawnNewRandom);
            }
        }

        Level LoadLevel(int id)
        {
            var fieldInfo = typeof(LevelsMaps).GetField("Levels");
            var levels = (Level[]) fieldInfo.GetValue(null);
            var level = levels[id];

            TutorCardInfo[] tutor = level.Tutor;
            (CT, int)[,] field = level.Field;
            (CT, int)[] inventory = level.Inventory;
            
            //new array, no reference
            _CommonState.BattleState.CollectColors = 
                new (ColorType, int)[level.CollectColors.Length];
            for (int j = 0; j < level.CollectColors.Length; j++)
            {
                _CommonState.BattleState.CollectColors[j] = 
                    (level.CollectColors[j].Item1, level.CollectColors[j].Item2);
            }

            LoadTutor(tutor);
            LoadField(field, level);
            LoadInventory(inventory);

            int i = 0;
            foreach (var color in level.CollectColors)
            {
                BattleUI.Requires[i].GemSprite.sprite = BattleUI.GetColorSprite(color.Item1);
                BattleUI.Requires[i].Quantity.text = color.Item2.ToString();
                BattleUI.Requires[i].gameObject.SetActive(true);
                i++;
            }
            
            for (; i < BattleUI.Requires.Length; i++)
            {
                BattleUI.Requires[i].gameObject.SetActive(false);
            }

            return level;
        }

        private void LoadField((CT, int)[,] field, Level level)
        {
            _loadedMap = new List<Queue<CardState>>(field.GetLength(1));

            for (int x = 0; x < field.GetLength(1); x++)
            {
                _loadedMap.Add(new Queue<CardState>(field.GetLength(0))); //AddEmptyRow
            }

            var chainMap = level.ChainMap;
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //up side revers
                for (int z = field.GetLength(0) - 1; z >= 0; z--)
                {
                    var cardSO = GetCardSO(field[z, x].Item1); //z ,x revers
                    CardState cardState;
                    if (cardSO == null)
                    {
                        if (level.NeedSpawnNewRandom)
                        {
                            cardState = CreateNewRandomCard();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        cardState = new CardState();
                        cardState.CardSO = cardSO;
                        cardState.CardSO.Type = cardSO.Type;
                    }

                    var quantity = field[z, x].Item2;
                    if (quantity <= 0)
                        quantity = 1;
                    cardState.Quantity = quantity;
                    if(chainMap != null)
                        if (chainMap.GetLength(0) > z && chainMap.GetLength(1) > x)
                        {
                            cardState.Chains = chainMap[z, x];
                        }

                    _loadedMap[x].Enqueue(cardState);
                }

                if (_loadedMap[x].Count > BattleObjects.Field.SizeZ)
                    BattleUI.LeftCardsPanel.Columns[x].text =
                        (_loadedMap[x].Count - BattleObjects.Field.SizeZ).ToString();
                else
                    BattleUI.LeftCardsPanel.Columns[x].text = "0";
            }
        }

        private void LoadInventory((CT, int)[] inventory)
        {
            _loadedInventory = new Queue<CardState>(inventory.Length);
            for (int x = 0; x < inventory.Length; x++)
            {
                var cardSO = GetCardSO(inventory[x].Item1);
                if (cardSO == null) continue;

                var cardState = new CardState();
                cardState.CardSO = cardSO;
                cardState.CardSO.Type = cardSO.Type;
                cardState.Quantity = inventory[x].Item2;

                _loadedInventory.Enqueue(cardState);
            }
        }
        
        private void LoadTutor(TutorCardInfo[] tutor)
        {
            _tutorActive = false;
            TutorHandObj.SetActive(false);
            TutorHandObj.transform.DOKill();
            if (tutor != null)
            {
                _CommonState.CurrentTutorial = new List<TutorCardInfo>(tutor);
            }
            else
            {
                _CommonState.CurrentTutorial = new List<TutorCardInfo>();
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
                newCard = Enemies[Random.Range(1, Enemies.Length)];
            }
            else
            {
                newCard = Items[Random.Range(0, Items.Length)];
            }

            return CreateCard(newCard);
        }
        
        CardState CreateCard(CardSO newCard)
        {
            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            return new CardState
            {
                CardSO = newCard,
                Quantity = quantity,
            };
        }

        CardState CreateCard(CardSO newCard, int quantity)
        {
            return new CardState
            {
                CardSO = newCard,
                Quantity = quantity,
            };
        }

        CardState CreateNewRandomItem()
        {
            var card = new CardState
            {
                CardSO = Items[Random.Range(0, Items.Length)],
                Quantity = Random.Range(1, _startMaxCellQuantity)
            };
            
            return card;
        }

        void ReCreateCard(ref CardState cardState, int row)
        {
            CardSO newCard;
            if (_CommonState.BattleState.LevelID < BattleState.CommonLevelID ||
                _CommonState.GetCurrentLevel().NeedSpawnNewRandom)
            {
                if (Random.Range(0, 1f) > _chanceItemOnFiled)
                {
                    newCard = Enemies[Random.Range(1, Enemies.Length)];
                }
                else
                {
                    newCard = Items[Random.Range(0, Items.Length)];
                }

                if (newCard != null)
                {
                    int quantity = 0;
                    quantity = Random.Range(1, _startMaxCellQuantity + 1);

                    cardState.CardSO = newCard;
                    cardState.Quantity = quantity;
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
            if (!WithQuantity)
            {
                cardGameObject.QuantityText.transform.parent.gameObject.SetActive(false);
            }
            _cardMonobehsPool.Add(cardGameObject);

            for (int i = 0; i < cardGameObject.Chains.Length; i++)
            {
                cardGameObject.Chains[i].SetActive(cardState.Chains > i);
            }

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

        CardSO GetCardSO(CT card)
        {
            if (card == CT.Em)
                return null;
            
            foreach (var enemy in Enemies)
            {
                if (enemy.CardType == card)
                    return enemy;
            }

            foreach (var item in Items)
            {
                if (item.CardType == card)
                    return item;
            }

            return null;
        }
    }
}