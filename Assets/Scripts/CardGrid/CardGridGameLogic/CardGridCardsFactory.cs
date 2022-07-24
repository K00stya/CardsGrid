using System.Collections;
using System.Collections.Generic;
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
        List<CardSO> _loadedEnemies;
        List<CardSO> _loadedItems;
        
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
            _loadedEnemies = null;
            _loadedItems = null;

            _cardMonobehsPool.Clear();
        }

        Card CreateNewRandomCard()
        {
            CardSO newCard;
            TypeCard type;
            if (Random.Range(0, 1f) > _chanceItemOnFiled)
            {
                newCard = _loadedEnemies[Random.Range(0, _loadedEnemies.Count)];
                type = TypeCard.Enemy;
            }
            else
            {
                newCard = _loadedItems[Random.Range(0, _loadedItems.Count)];
                type = TypeCard.Item;
            }

            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            return new Card {
                name = newCard.Name,
                Type = type,
                Quantity = quantity,
                StartQuantity = quantity};
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
            TypeCard type;
            if (Random.Range(0, 1f) > _chanceItemOnFiled)
            {
                newCard = _loadedEnemies[Random.Range(0, _loadedEnemies.Count)];
                type = TypeCard.Enemy;
            }
            else
            {
                newCard = _loadedItems[Random.Range(0, _loadedItems.Count)];
                type = TypeCard.Item;
            }

            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            card.Type = type;
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
    }
}