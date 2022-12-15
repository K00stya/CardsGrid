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
        private void LoadTutor(TutorCardInfo[] tutor)
        {
            _tutorActive = false;
            TutorHandObj.SetActive(false);
            TutorHandObj.transform.DOKill();
            if (tutor != null)
            {
                _CommonState.BattleState.CurrentTutorial = new List<TutorCardInfo>(tutor);
            }
            else
            {
                _CommonState.BattleState.CurrentTutorial = new List<TutorCardInfo>();
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
            _CommonState.BattleState.Inventory.Items = null;
            _CommonState.BattleState.Filed.Cells = null;
        }

        CardState CreateNewRandomCard()
        {
            CardSO newCard;
            if (Random.Range(0, 1f) > _chanceItemOnFiled)
            {
                newCard = GameSetings.Enemies[Random.Range(1, GameSetings.Enemies.Length)];
            }
            else
            {
                newCard = GameSetings.Items[Random.Range(0, GameSetings.Items.Length)];
            }

            return CreateCard(newCard);
        }
        
        CardState CreateCard(CardSO newCard)
        {
            int quantity = Random.Range(1, _startMaxCellQuantity + 1);
            return new CardState
            {
                ScrObj = newCard,
                Quantity = quantity,
                StartQuantity = quantity
            };
        }

        CardState CreateNewRandomItem()
        {
            int quantity = Random.Range(1, _startMaxCellQuantity);
            var card = new CardState
            {
                ScrObj = GameSetings.Items[Random.Range(0, GameSetings.Items.Length)], //
                Quantity = quantity,
                StartQuantity = quantity
            };
            
            return card;
        }

        void ReCreateCard(ref CardState cardState)
        {
            var levelSo = _CommonState.GetCurrentLevel().ScrObj;
            if (levelSo.Cards.Length > 0)
            {
                CardSO newCard;
                if (Random.Range(0, 1f) > _chanceItemOnFiled)
                {
                    newCard = levelSo.Cards[Random.Range(1, GameSetings.Enemies.Length)];
                }
                else
                {
                    newCard = levelSo.Items[Random.Range(0, GameSetings.Items.Length)];
                }

                if (newCard != null)
                {
                    int quantity = 0;
                    quantity = Random.Range(1, _startMaxCellQuantity + 1);

                    cardState.ScrObj = newCard;
                    cardState.Quantity = quantity;
                    cardState.StartQuantity = quantity;
                    cardState.GameObject.gameObject.SetActive(true);
                    SetCommonCardState(ref cardState, ref cardState.GameObject, newCard);
                }
            }
        }

        void SetCommonCardState(ref CardState cardState, ref CardGameObject cardGameObject, CardSO cardInfo)
        {
            cardGameObject.Sprite.sprite = cardInfo.Sprite;
            if (cardState.ScrObj.Type == TypeCard.Block)
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
            CardGameObject cardGameObject = Instantiate(BattleObjects.CardPrefab, grid.ParentCards);
            cardGameObject.transform.localScale = grid.SlotScale;

            if (cardState.Grid == CardGrid.Field)
            {
                cardGameObject.transform.position = grid.GetSpawnPosition(cardState.Position.x, cardState.Position.y);
                cardGameObject.transform.DOMove(grid.GetCellSpacePosition(cardState.Position),
                    SpeedRecession + 0.3f);
            }
            else
            {
                cardGameObject.transform.position = grid.GetCellSpacePosition(cardState.Position);
            }

            if (cardState.Quantity <= 0)
            {
                cardGameObject.gameObject.SetActive(false);
            }
            if(cardState.ScrObj != null)
                SetCommonCardState(ref cardState, ref cardGameObject, cardState.ScrObj);
            if (!WithQuantity)
            {
                cardGameObject.QuantityText.transform.parent.gameObject.SetActive(false);
            }
            _cardMonobehsPool.Add(cardGameObject);

            cardGameObject.Block.SetActive(cardState.Block > 0);

            return cardGameObject;
        }
    }
}