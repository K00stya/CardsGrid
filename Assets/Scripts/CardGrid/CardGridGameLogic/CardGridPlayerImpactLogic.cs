using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CardGrid
{
    /*
     * In this partial, the logic of the player's attack.
     * Mainly processes Update and its possible results during the battle.
     */
    public partial class CardGridGame //PlayerImpactLogic
    {
        public float SpeedRecession = 0.1f;
        public float SpeedFilling = 0.1f;
        
        bool _inputActive = true;
        
        //Local update variables, but cached for the glory of the garbage collector
        Ray _ray;
        RaycastHit[] _hits;
        Plane _plane = new Plane(Vector3.up, Vector3.zero);

        CardGameObject _dragGameObjectCard;
        CardGameObject _hitFieldCardOnDrag;
        List<CardGameObject> _highlightCards = new List<CardGameObject>();
        bool _needRecession;

        //Not fixed because I use raycasts for get input, and I don't make changes in physics
        void Update()
        {
            if (!_inputActive) return;
            
            if (RaycastIsHitCard(out var cardGO))
            {
                DebugSystem.DebugLog($"Raycast hit card {cardGO.CardState.name}" +
                                     $" on {cardGO.CardState.Position}", DebugSystem.Type.PlayerInput);

                if (Input.GetMouseButton(0))
                {
                    //Start drag
                    if (_dragGameObjectCard == null)
                    {
                        _dragGameObjectCard = cardGO;
                    }
                }
                else
                {
                    _inputActive = false;
                    StartCoroutine(EndDrag());
                }
            }

            if (_dragGameObjectCard != null && _plane.Raycast(_ray, out var enter))
            {
                Drag(enter);
            }

            bool RaycastIsHitCard(out CardGameObject cardMonobeh)
            {
                cardMonobeh = null;
                _ray = _camera.ScreenPointToRay(Input.mousePosition);
                _hits = Physics.RaycastAll(_ray);
                return (_hits != null && _hits.Length > 0 &&
                        _hits[0].collider.TryGetComponent<CardGameObject>(out cardMonobeh));
            }
        }

        void Drag(float enter)
        {
            DebugSystem.DebugLog($"Drag card {_dragGameObjectCard.CardState.name}", DebugSystem.Type.PlayerInput);
            var newPosition = _ray.GetPoint(enter);
            _dragGameObjectCard.transform.position = new Vector3(newPosition.x, 1f, newPosition.z);

            TryHighlight(_dragGameObjectCard);

            void TryHighlight(CardGameObject activeCard)
            {
                if (_hits == null) return;

                bool dragOnField = false;
                foreach (var obj in _hits)
                {
                    var monobeh = obj.collider.GetComponent<CardGameObject>();
                    if (!monobeh || activeCard == monobeh)
                        continue;
                    if (_hitFieldCardOnDrag == monobeh)
                    {
                        dragOnField = true;
                        continue;
                    }

                    Highight(monobeh);
                }

                //If player not drag on field we disable highlights
                if (!dragOnField)
                {
                    DisableHighlight();
                }

                void Highight(CardGameObject monobeh)
                {
                    var firstCard = activeCard.CardState;
                    var secondCard = monobeh.CardState;

                    if (firstCard.Grid == CardGrid.Inventory && secondCard.Grid == CardGrid.Field)
                    {
                        dragOnField = true;
                        DisableHighlight();
                        _hitFieldCardOnDrag = monobeh;
                        var cards = GetImpactCards(firstCard, secondCard);
                        foreach (var card in cards)
                        {
                            var highlightGO = card.GameObject;
                            highlightGO.Highlight.SetActive(true);
                            _highlightCards.Add(highlightGO);
                        }
                    }
                }

                Card[] GetImpactCards(Card itemCard, Card fieldCard)
                {
                    DebugSystem.DebugLog($"Item {itemCard.name}, impact on {fieldCard.name}" +
                                         $" X:{fieldCard.Position.x} Y:{fieldCard.Position.y}",
                        DebugSystem.Type.Battle);

                    //TODO Push PickUp
                    int[,] attackArray = GetImpactMap<ImpactMaps>(itemCard.ImpactMap);

                    return GetImpactedCards(itemCard.name, fieldCard.Position, attackArray);
                }
            }
        }

        IEnumerator EndDrag()
        {
            if (_dragGameObjectCard == null)
            {
                _inputActive = true;
                yield break;
            }

            if (_highlightCards.Count > 0)
            {
                yield return StartCoroutine(DealImpact());
            }
            else
            {
                DisableHighlight();
                var dragCard = _dragGameObjectCard.CardState;
                if (dragCard.Grid == CardGrid.Field)
                {
                    _dragGameObjectCard.transform.position = 
                        BattleObjects.Field.GetCellSpacePosition(dragCard.Position);
                }
                else
                {
                    _dragGameObjectCard.transform.position = 
                        BattleObjects.Inventory.GetCellSpacePosition(dragCard.Position);
                }
            }

            _dragGameObjectCard = null;
            _inputActive = true;
        }

        IEnumerator DealImpact()
        {
            yield return StartCoroutine(UseItemOnFiled(_dragGameObjectCard, _highlightCards));
            var cells = _CommonState.BattleState.Filed.Cells;
            var items = _CommonState.BattleState.Inventory.Items;

            do
            {
                RecessionField(cells);
                yield return StartCoroutine(Filling(cells));
                //yield return StartCoroutine(CellsCombinations(cells));

                RecessionInventory(items);
                //yield return StartCoroutine(ItemsCombinations(items));
                yield return StartCoroutine(TryGetNewItemsForField(cells, items));
            } while (_needRecession);

            CheckDefeat(items);
        }

        #region DealImpact
        
        IEnumerator UseItemOnFiled(CardGameObject dragCard, List<CardGameObject> highlightCards)
        {
            Card impactCard = dragCard.CardState;
            Card[] cards = new Card[highlightCards.Count];
            for (int i = 0; i < highlightCards.Count; i++)
            {
                cards[i] = highlightCards[i].CardState;
            }
            DisableHighlight();

            yield return SpawnEffectOnCards(impactCard, cards);

            List<Card> deaths = new List<Card>();
            List<Card> woundeds = new List<Card>(cards);

            ImpactDamageOnField(impactCard.Quantity, cards, ref deaths);
            impactCard.Quantity = 0;
            dragCard.gameObject.SetActive(false);

            yield return ReactOnImpact(deaths, woundeds);
        }
        
        void RecessionField(Card[,] cards)
        {
            for (int i = 0; i < cards.GetLength(1); i++)
            {
                for (int x = 0; x < cards.GetLength(0); x++)
                {
                    for (int z = cards.GetLength(1) - 1; z >= 0; z--)
                    {
                        int newZ = z - 1;
                        if (cards[x, z].Quantity <= 0 && newZ >= 0)
                        {
                            SwapPositions(cards, cards[x, z].Position, cards[x, newZ].Position);
                        }
                    }
                }
            }

            foreach (var card in cards)
            {
                if (card.Quantity <= 0)
                {
                    card.GameObject.transform.position = 
                        BattleObjects.Field.GetSpawnPosition(card.Position.x);
                }
                else
                {
                    StartCoroutine(MoveCardToSelfPosition(card, BattleObjects.Field));
                }
            }
        }

        IEnumerator Filling(Card[,] cards)
        {
            for (int z = cards.GetLength(1) - 1; z >= 0; z--)
            {
                for (int x = 0; x < cards.GetLength(0); x++)
                {
                    if (cards[x, z].Quantity <= 0)
                    {
                        ReCreateCard(cards[x, z]);
                        yield return MoveCardToSelfPosition(cards[x, z], BattleObjects.Field);
                    }
                }
                //for smoothness
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(SpeedFilling);
        }
        
        IEnumerator CellsCombinations(Card[,] cards)
        {
            yield return null;
        }
        
        private IEnumerator MoveCardToSelfPosition(Card card, GridGameObject grid)
        {
            yield return card.GameObject.transform.DOMove(grid.
                GetCellSpacePosition(card.Position), SpeedRecession);
        }

        void ImpactDamageOnField(int damage, Card[] cards, ref List<Card> deaths)
        {
            foreach (var card in cards)
            {
                card.Quantity -= damage;
                if (card.Quantity <= 0)
                {
                    deaths.Add(card);
                    card.GameObject.gameObject.SetActive(false);
                    
                    _CommonState.BattleState.Score += card.StartQuantity;
                    BattleUI.Score.text = _CommonState.BattleState.Score.ToString();
                    if (_CommonState.BattleState.Score > _CommonState.BestScore)
                        _CommonState.BestScore = _CommonState.BattleState.Score;
                    continue;
                }

                card.GameObject.QuantityText.text = card.Quantity.ToString();
            }
        }
        
        #endregion

        //The X coordinates require a second dimension of the attack map
        //and so, I don't know if this is my mistake or a necessity.
        Card[] GetImpactedCards(string name, Vector2Int position, int[,] attackMap)
        {
            int lenghtX = attackMap.GetLength(1); //1
            int lenghtZ = attackMap.GetLength(0); //0
            List<Card> cards = new List<Card>(lenghtX * lenghtZ);

            int offset = 0;
            if (attackMap.Length == 1) //1X1
                offset = 0;
            else if (attackMap.Length == 9) //3X3
                offset = 1;
            else if (attackMap.Length == 25) //5X5
                offset = 2;
            else
                DebugSystem.DebugLog($"Invalid impact map {name}", DebugSystem.Type.Error);

            var startX = position.x - offset;
            var startY = position.y - offset;

            for (int x = startX, i = 0;
                 x < BattleObjects.Field.SizeX && i < lenghtX;
                 x++, i++)
            {
                for (int z = startY, j = 0;
                     z < BattleObjects.Field.SizeZ && j < lenghtZ;
                     z++, j++)
                {
                    if (x < 0 || z < 0) continue;

                    if (attackMap[j, i] == 0) continue;

                    var cell = _CommonState.BattleState.Filed.Cells[x, z];

                    //If card is a live she get impact
                    if (cell.Quantity > 0)
                        cards.Add(cell);
                }
            }

            return cards.ToArray();
        }

        /*
        Reflection is a good substitute for a huge dynamic switch,
        I think until the number of maps reaches several tens of thousands,
        there should be no performance problems,
        until then you can enjoy the elegance of my solution
        */
        int[,] GetImpactMap<T>(Maps map)
        {
            var filed = typeof(T).GetField(map.ToString());
            if (filed != null)
            {
                return (int[,]) filed.GetValue(null);
            }

            DebugSystem.DebugLog($"Invalid impact map requested {map.ToString()}", DebugSystem.Type.Error);
            return null;
        }

        int[,] GetImpactMap<T>(string name)
        {
            var filed = typeof(T).GetField(name);
            if (filed != null)
            {
                return (int[,]) filed.GetValue(null);
            }

            DebugSystem.DebugLog($"Invalid impact name requested {name}", DebugSystem.Type.Error);
            return null;
        }

        void SwapPositions(Card[,] cards, Vector2Int first, Vector2Int second)
        {
            (cards[first.x, first.y].Position, cards[second.x, second.y].Position) =
                (cards[second.x, second.y].Position, cards[first.x, first.y].Position);

            (cards[first.x, first.y], cards[second.x, second.y])
                = (cards[second.x, second.y], cards[first.x, first.y]);
        }

        //TODO Pool efffects
        float SpawnEffectOnCards(Card impactCard, Card[] cards)
        {
            var effect = impactCard.Effect;
            if (effect)
            {
                foreach (var card in cards)
                {
                    var go = Instantiate(effect, BattleObjects.ParentEffects);
                    go.transform.position = BattleObjects.Field.GetCellSpacePosition(card.Position);
                }

                return effect.GetComponent<ParticleSystem>().main.duration;
            }

            return 0;
        }

        float SpawnEffectOnCard(Card card)
        {
            var effect = card.Effect;
            if (effect)
            {
                var go = Instantiate(effect, BattleObjects.ParentEffects);
                go.transform.position = BattleObjects.Field.GetCellSpacePosition(card.Position);

                return effect.GetComponent<ParticleSystem>().main.duration;
            }

            return 0;
        }

        void DisableHighlight()
        {
            foreach (var highlighted in _highlightCards)
            {
                highlighted.Highlight.SetActive(false);
            }

            _highlightCards = new List<CardGameObject>();
        }
        
        void CheckDefeat(Card[,] items)
        {
            bool haveItems = false;
            foreach (var item in items)
            {
                if (item.Quantity > 0)
                {
                    haveItems = true;
                    break;
                }
            }

            if (!haveItems)
            {
                DebugSystem.DebugLog("Defeat", DebugSystem.Type.Battle);
                BattleUI.Defeat.SetActive(true);
                _CommonState.InBattle = false;
                _inputActive = false;
            }
        }
    }
}