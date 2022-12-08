using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace CardGrid
{
    [Serializable]
    public class EffectPool
    {
        public CrystalEffect Effect;
        public int quantity;
    }
    /*
     * In this partial, the logic of the player's attack.
     * Mainly processes Update and its possible results during the battle.
     */
    public partial class CardGridGame //PlayerImpactLogic
    {
        public float SpeedRecession = 0.4f;

        [Header("EffectsPools")] 
        public EffectPool[] Pools;
        private Dictionary<string, Queue<CrystalEffect>> _poolsEffects;
        
        bool _inputActive = true;
        Sequence _mySequence;
        CardGameObject _swayingCard;
        
        //Local update variables, but cached for the glory of the garbage collector
        Ray _ray;
        RaycastHit[] _hits;
        Plane _plane = new Plane(Vector3.up, Vector3.zero);

        CardGameObject _selectedCard;
        float _currentClickTime;
        float _needClickTime = 0.3f;
        CardGameObject _dragGameObjectCard;
        CardGameObject _hitFieldCardOnDrag;
        List<CardGameObject> _impactHighlightCards = new List<CardGameObject>();
        List<CardGameObject> _infoHighlightCards = new List<CardGameObject>();
        bool _itemsRecession;
        bool _enemiesRecession;
        bool _tutorActive;
        bool _playerPressed;
        int _nextLevels;

        void LoadPools()
        {
            _poolsEffects = new Dictionary<string, Queue<CrystalEffect>>();
            foreach (var pool in Pools)
            {
                var queue = new Queue<CrystalEffect>(pool.quantity);
                for (int i = 0; i < pool.quantity; i++)
                {
                    queue.Enqueue(Instantiate(pool.Effect));
                }
                _poolsEffects.Add(pool.Effect.name, queue);
            }
        }

        //Not fixed because I use raycasts for get input, and I don't make changes in physics
        void Update()
        {
            if (RaycastIsHitCard(out var cardGO))
            {
                DebugSystem.DebugLog($"Raycast hit card {cardGO.CardState.CardSO.Name}" +
                                     $" on {cardGO.CardState.Position}", DebugSystem.Type.PlayerInput);
                
                if (_selectedCard != null)
                {
                    TryHighlight(_selectedCard);
                }
                
                if (Input.GetMouseButton(0))
                {
                    _currentClickTime += Time.deltaTime;
                    _playerPressed = true;

                    //Start drag
                    if (_dragGameObjectCard == null)
                    {
                        //Drag only inventory
                        if (cardGO.CardState.Grid == CardGrid.Inventory)
                        {
                            if (_selectedCard != null)
                            {
                                _selectedCard.Highlight.SetActive(false);
                                StartCoroutine(EndDrag(_selectedCard));
                            }
                            _dragGameObjectCard = cardGO;
                        }
                    }
                }
                else
                {
                    if (_playerPressed && _selectedCard == null && _inputActive)
                    {
                        //only inventory items
                        if (cardGO.CardState.Grid == CardGrid.Inventory)
                        {
                            if (_selectedCard != null)
                            {
                                _selectedCard.Highlight.SetActive(false);
                            }

                            //Debug.Log("Selected");
                            _selectedCard = cardGO;
                            _selectedCard.Highlight.SetActive(true);
                        }
                    }

                    //Return item if unpress on input not active
                    if (_dragGameObjectCard != null && !_inputActive)
                    {
                        ReturnDragCard(_dragGameObjectCard);
                        _dragGameObjectCard = null;
                    }
                    
                    StartCoroutine(EndDrag(_dragGameObjectCard));
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (_selectedCard != null)
                    {
                        _selectedCard.Highlight.SetActive(false);
                    }
                    StartCoroutine(EndDrag(_selectedCard));
                }
            }
            else
            {
                DisableHighlightInfo();
            }

            DragAndDrop();

            if (Input.GetMouseButtonDown(0))
            {
                PlayerClick?.Invoke();
            }

            TutorHand();

            if (_CommonState.BattleState.Inventory.Items != null)
                if (!_inputActive)
                {
                    foreach (var item in _CommonState.BattleState.Inventory.Items)
                    {
                        item.GameObject.Sprite.color = Color.gray;
                    }
                }
                else
                {
                    foreach (var item in _CommonState.BattleState.Inventory.Items)
                    {
                        item.GameObject.Sprite.color = Color.white;
                    }
                }
            
            _saveTimer += Time.deltaTime;
        }

        #region DragAndDrop
        
        bool RaycastIsHitCard(out CardGameObject cardMonobeh)
        {
            cardMonobeh = null;
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            _hits = Physics.RaycastAll(_ray);
            return (_hits != null && _hits.Length > 0 &&
                    _hits[0].collider.TryGetComponent<CardGameObject>(out cardMonobeh));
        }

        void DragAndDrop()
        {
            if (_dragGameObjectCard != null)
            {
                if (_plane.Raycast(_ray, out var enter) && IsItemInTutor(_dragGameObjectCard.CardState))
                {
                    DebugSystem.DebugLog($"Drag card {_dragGameObjectCard.CardState.CardSO.Name}",
                        DebugSystem.Type.PlayerInput);
                    var newPosition = _ray.GetPoint(enter);
                    _dragGameObjectCard.transform.position = new Vector3(newPosition.x, 1f, newPosition.z);

                    if (!_inputActive) return;

                    TryHighlight(_dragGameObjectCard);
                }
            }
        }

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
                _hitFieldCardOnDrag = null;
                DisableImpactHighlight();
            }

            void Highight(CardGameObject monobeh)
            {
                var firstCard = activeCard.CardState;
                var secondCard = monobeh.CardState;

                if (firstCard.Grid == CardGrid.Inventory && secondCard.Grid == CardGrid.Field)
                {
                    dragOnField = true;
                    DisableImpactHighlight();
                    _hitFieldCardOnDrag = monobeh;
                    var cards = GetImpactCards(firstCard, secondCard);
                    foreach (var card in cards)
                    {
                        var highlightGO = card.GameObject;
                        highlightGO.Highlight.SetActive(true);
                        _impactHighlightCards.Add(highlightGO);
                    }
                }
            }
        }

        bool IsItemInTutor(CardState card)
        {
            if (_CommonState.BattleState.CurrentTutorial != null && _CommonState.BattleState.CurrentTutorial.Count > 0)
            {
                var tutorCards = _CommonState.BattleState.CurrentTutorial.First();
                if (tutorCards.RotateLeft || tutorCards.RotateRight)
                    return false;
                
                if (tutorCards.AnyItem)
                    return true;

                return tutorCards.ItemPosition == card.Position;
            }

            return true;
        }

        bool IsEnemyInTutorAndRemove(CardState cardState)
        {
            if (_CommonState.BattleState.CurrentTutorial != null && _CommonState.BattleState.CurrentTutorial.Count > 0)
            {
                var tutorCards = _CommonState.BattleState.CurrentTutorial.First();
                if (tutorCards.RotateLeft || tutorCards.RotateRight)
                    return false;
                
                if (tutorCards.FieldPosition == cardState.Position)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
        
        IEnumerator EndDrag(CardGameObject drag)
        {
            Debug.Log(_battleMenuOpen);
            if (drag == null)
            {
                yield break;
            }

            if (!_inputActive || _battleMenuOpen)
            {
                yield break;
            }

            if(_selectedCard)
                _selectedCard.Highlight.SetActive(false);
            _selectedCard = null;
            _dragGameObjectCard = null;
            _inputActive = false;

            if (_impactHighlightCards.Count > 0 &&
                IsEnemyInTutorAndRemove(_hitFieldCardOnDrag.CardState) && 
                IsItemInTutor(drag.CardState))
            {
                if (_CommonState.BattleState.CurrentTutorial.Count > 0)
                    _CommonState.BattleState.CurrentTutorial.RemoveAt(0);
                
                yield return DealImpact(drag);

                while (_nextLevels > 0)
                {
                    yield return NextLevel();
                    
                    _nextLevels--;
                }
                CheckWinOrDefeat(_CommonState.BattleState.Filed.Cells, _CommonState.BattleState.Inventory.Items);
                
                _playerPressed = false;
            }
            else
            {
                ReturnDragCard(drag);
            }
            _inputActive = true;
        }
        
        void ReturnDragCard(CardGameObject drag)
        {
            DisableImpactHighlight();
            var dragCard = drag.CardState;
            if (dragCard.Grid == CardGrid.Field)
            {
                drag.transform.position = 
                    BattleObjects.Field.GetCellSpacePosition(dragCard.Position);
            }
            else
            {
                drag.transform.position = 
                    BattleObjects.Inventory.GetCellSpacePosition(dragCard.Position);
            }
        }

        IEnumerator DealImpact(CardGameObject drag)
        {
            yield return UseItemOnFiled(drag, _impactHighlightCards);
            yield return UpdateFields();
        }

        IEnumerator UpdateFields()
        {
            var cells = _CommonState.BattleState.Filed.Cells;
            var items = _CommonState.BattleState.Inventory.Items;
            do
            {
                _enemiesRecession = false;
                
                RecessionField(_CommonState.BattleState.Filed.Cells);
                
                yield return Filling(cells);

                yield return CheckMach3();
                
                RecessionInventory(items);
                yield return TryGetNewItemsForField(cells, items);
                
            } while (_itemsRecession || _enemiesRecession);
        }

        #region DealImpact
        
        IEnumerator UseItemOnFiled(CardGameObject dragCard, List<CardGameObject> highlightCards)
        {
            CardState impactCardState = dragCard.CardState;
            CardState[] cards = new CardState[highlightCards.Count];
            for (int i = 0; i < highlightCards.Count; i++)
            {
                cards[i] = highlightCards[i].CardState;
            }
            DisableImpactHighlight();
            
            //List<CardState> deaths = new List<CardState>();
            //List<CardState> woundeds = new List<CardState>(cards);

            impactCardState.GameObject.gameObject.SetActive(false);
            yield return ImpactDamageOnField(impactCardState.Quantity, cards); //ref deaths);
            impactCardState.Quantity = 0;
            dragCard.gameObject.SetActive(false);

            //sound effect
            if (impactCardState.CardSO.SoundEffect != null)
            {
                BattleAudioSource.clip = impactCardState.CardSO.SoundEffect;
                BattleAudioSource.Play();
            }
            //visual effect
            SpawnItemEffect(impactCardState, cards);
            //yield return ReactOnImpact(deaths, woundeds);
        }
        
        //optimization
        void RecessionField(CardState[,] cards)
        {
            for (int i = 0; i < cards.GetLength(1); i++)
            {
                for (int x = 0; x < cards.GetLength(0); x++)
                {
                    for (int z = cards.GetLength(1) - 1; z >= 0; z--)
                    {
                        int newZ = z - 1;
                        if (cards[x, z].Quantity <= 0 && newZ >= 0 && cards[x, newZ].Quantity >= 0)
                        {
                            SwapPositions(cards, cards[x, z].Position, cards[x, newZ].Position);
                        }
                    }
                }
            }
        }

        IEnumerator Filling(CardState[,] cards)
        {
            bool needWait = false;
            for (int z = cards.GetLength(1) - 1; z >= 0; z--)
            {
                for (int x = 0; x < cards.GetLength(0); x++)
                {
                    var card = cards[x, z];
                    if (card.Quantity <= 0)
                    {
                        card.GameObject.transform.position =
                            BattleObjects.Field.GetSpawnPosition(cards[x, z].Position.x, z);
                        ReCreateCard(ref cards[x, z], x);
                        MoveCardToSelfPosition(cards[x, z], BattleObjects.Field);
                        needWait = true;
                    }
                    else
                    {
                        if (Vector3.Distance(BattleObjects.Field.GetCellSpacePosition(card.Position),
                                card.GameObject.transform.position) > 0.1f)
                        {
                            MoveCardToSelfPosition(card, BattleObjects.Field);
                            needWait = true;
                        }
                    }
                }
            }
            if(needWait)
                yield return new WaitForSeconds(SpeedRecession);
        }
        
        void MoveCardToSelfPosition(CardState cardState, GridGameObject grid)
        {
            if(cardState.GameObject == _dragGameObjectCard) return;
            cardState.GameObject.transform.DOMove(grid.
                GetCellSpacePosition(cardState.Position), SpeedRecession);
        }

        List<int> _deaths = new List<int>(20);
        private List<CardState> _collection = new List<CardState>(20);
        IEnumerator ImpactDamageOnField(int damage, CardState[] cards)
        {
            _deaths.Clear();
            _collection.Clear();
            foreach (var card in cards)
            {
                Damage(card, damage);
            }
            
            int sum = 0;
            foreach (var val in _deaths)
            {
                sum += val;
            }
            if(_CommonState.BattleState.LevelID == 0)
                yield return UpdateLevel(sum);
            
            foreach (var col in _collection)
            {
                yield return CollectColorAndShape(col);
            }
        }
        
        IEnumerator ImpactDamageOnField(CardState[] cards)
        {
            _deaths.Clear();
            _collection.Clear();
            foreach (var card in cards)
            {
                Damage(card, card.Quantity);
            }

            int sum = 0;
            foreach (var val in _deaths)
            {
                sum += val;
            }
            if(_CommonState.BattleState.LevelID == 0)
                yield return UpdateLevel(sum);
            
            foreach (var col in _collection)
            {
                yield return CollectColorAndShape(col);
            }
        }

        void Damage(CardState card, int damage)
        {
            if (card.Chains > 0)
            {
                card.Chains--;
                
                for (int i = 0; i < card.GameObject.Chains.Length; i++)
                {
                    card.GameObject.Chains[i].SetActive(card.Chains > i);
                }
            }
            else
            {
                if(WithQuantity)
                    card.Quantity -= damage;
                else
                    card.Quantity -= card.Quantity;
                
                if (card.Quantity <= 0)
                {
                    card.GameObject.gameObject.SetActive(false);

                    if (_CommonState.BattleState.LevelID < BattleState.CommonLevelID)
                    {
                        _deaths.Add(1);
                        
                        if (WithQuantity)
                            _deaths.Add(-card.Quantity);
                    }
                    
                    _collection.Add(card);

                    CollectGemsAchive(card);
                    
                    return;
                }
                card.GameObject.QuantityText.text = card.Quantity.ToString();
            }
        }

        IEnumerator CollectColorAndShape(CardState card)
        {
            var state = _CommonState.BattleState;
            if (state.CollectColors == null || state.CollectColors.Length <= 0)
            {
                if (_CommonState.BattleState.LevelID < BattleState.CommonLevelID)
                {
                    state.CollectColors = GenerateNewCollectColors();
                    UpdateCompleteQuest(state.CollectColors);
                }
                else
                {
                    yield break;
                }
            }

            bool win = true;

            for (int i = 0; i < state.CollectColors.Length; i++)
            {
                if (card.CardSO.ColorType == state.CollectColors[i].Item1)
                {
                    state.CollectColors[i].Item2 -= 1;
                    if (state.CollectColors[i].Item2 <= 0)
                    {
                        state.CollectColors[i].Item2 = 0;
                    }
                    else
                    {
                        win = false;
                    }
                }
                else
                {
                    if (state.CollectColors[i].Item2 > 0)
                    {
                        win = false;
                    }
                }
            }

            if (win)
                if (_CommonState.BattleState.LevelID >= BattleState.CommonLevelID)
                {
                    Win();
                }
                else if (_CommonState.BattleState.LevelID < BattleState.CommonLevelID)
                {
                    MenuAudioSource.clip = QuestComplete;
                    MenuAudioSource.Play();
                    yield return Quest();

                    state.CollectColors = GenerateNewCollectColors();
                    UpdateCompleteQuest(state.CollectColors);
                }
            
            UpdateRequires(state.CollectColors);
        }

        IEnumerator Quest()
        {
            var panel  = BattleUI.QuestCompletePanel;
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, LevelUpSeed);
            panel.gameObject.SetActive(true);
            yield return new WaitForSeconds(LevelUpSeed + 1f);
            yield return UpdateLevel(reawardForCompleteTask);
            foreach (var require in panel.Requires)
            {
                require.Quantity.gameObject.SetActive(false);
                require.ToggleCheck.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
            
            panel.transform.DOScale(Vector3.zero, LevelUpSeed);
            yield return new WaitForSeconds(LevelUpSeed);
            panel.gameObject.SetActive(false);
        }

        void GenerateColorTypesList()
        {
            var v = Enum.GetValues (typeof (ColorType));
            for (int i = 0; i < v.Length; i++)
            {
                ColorTypes.Add( (ColorType) v.GetValue(i) );
            }
        }
        
        (ColorType, int)[] GenerateNewCollectColors()
        {
            List<ColorType> types = new List<ColorType>(ColorTypes);

            int quantity = Random.Range(2, 4);
            (ColorType, int)[] collection = new (ColorType, int)[quantity];
            for (int i = 0; i < quantity; i++)
            {
                int index = Random.Range(0, types.Count);
                ColorType type = types[index];
                types.RemoveAt(index);
                collection[i] = (type, Random.Range(3, 11));
            }
            
            return collection;
        }
        
        #endregion

        //The X coordinates require a second dimension of the attack map
        //and so, I don't know if this is my mistake or a necessity.
        CardState[] GetImpactedCards(string name, Vector2Int position, int[,] attackMap)
        {
            int lenghtX = attackMap.GetLength(1); //1
            int lenghtZ = attackMap.GetLength(0); //0
            List<CardState> cards = new List<CardState>(lenghtX * lenghtZ);

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
                    if (cell is {Quantity: > 0})
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

        void SwapPositions(CardState[,] cards, Vector2Int first, Vector2Int second)
        {
            (cards[first.x, first.y].Position, cards[second.x, second.y].Position) =
                (cards[second.x, second.y].Position, cards[first.x, first.y].Position);

            (cards[first.x, first.y], cards[second.x, second.y])
                = (cards[second.x, second.y], cards[first.x, first.y]);
        }

        //TODO Pool efffects
        float SpawnItemEffect(CardState impactCardState, CardState[] cards)
        {
            bool needWait = false;
            var effect = impactCardState.CardSO.Effect;

            if (effect)
            {
                foreach (var card in cards)
                {
                    if (card.Quantity >= 0) //Target live?
                    {
                        var go = GetEffect(effect.name);
                        go.transform.position = BattleObjects.Field.GetCellSpacePosition(card.Position)
                                                + new Vector3(0,2f,0);
                        needWait = true;
                    }
                }
                
                if(needWait)
                    return effect.System.main.duration;
            }

            return 0;
        }

        float SpawnEffectOnCards(CardState[] cards)
        {
            float duration = 0;

            foreach (var card in cards)
            {
                if (card.Quantity >= 0) //Target live?
                {
                    var go = GetEffect(card.CardSO.Effect.name);

                    go.Color.SetColor(card.CardSO.ColorType);
                    go.transform.position = BattleObjects.Field.GetCellSpacePosition(card.Position)
                                            + new Vector3(0, 2f, 0);
                    if(duration == 0)
                        duration = go.System.main.duration;
                }
            }

            return duration;
        }

        private Queue<CrystalEffect> _effects;
        CrystalEffect _currentEffect;
        CrystalEffect GetEffect(string name)
        {
            _effects = _poolsEffects[name];
            _currentEffect = _effects.Dequeue();
            _effects.Enqueue(_currentEffect);
            _currentEffect.gameObject.SetActive(true);
            _currentEffect.System.Play();
            return _currentEffect;
        }

        float SpawnEffectOnCard(CardState cardState)
        {
            var effect = cardState.CardSO.Effect;
            if (effect)
            {
                var go = Instantiate(effect, BattleObjects.ParentEffects);
                go.transform.position = BattleObjects.Field.GetCellSpacePosition(cardState.Position);

                return effect.GetComponent<ParticleSystem>().main.duration;
            }

            return 0;
        }

        // GameObject GetEffect(string name)
        // {
        //     
        // }

        void DisableImpactHighlight()
        {
            foreach (var highlighted in _impactHighlightCards)
            {
                highlighted.Highlight.SetActive(false);
            }

            _impactHighlightCards = new List<CardGameObject>();
        }

        #endregion
        
        void SetSwaying(CardGameObject cardGO)
        {
            if (_inputActive && _mySequence is not {active: true} && _swayingCard != cardGO && _dragGameObjectCard == null)
            {
                _swayingCard = cardGO;
                var cardTransform = cardGO.transform;
                var startScale = cardTransform.localScale;
                _mySequence = DOTween.Sequence();
                _mySequence
                    .Append(cardTransform.DORotate(new Vector3(90,6,0), .06f))
                    .Append(cardTransform.DORotate(new Vector3(90,-6,0), .12f))
                    .Append(cardTransform.DORotate(new Vector3(90,0,0), .06f))
                    .Insert(0, cardTransform.DOScale(startScale * 1.2f, _mySequence.Duration()/2))
                    .Insert(0, cardTransform.DOScale(startScale, _mySequence.Duration()/2));
                
                DisableHighlightInfo();
                var cardState = cardGO.CardState;
                if (cardState.CardSO.Type == TypeCard.Enemy && cardState.Grid == CardGrid.Field)
                {
                    var cards = GetImpactCards(cardState);
                    foreach (var card in cards)
                    {
                        var highlightGO = card.GameObject;
                        highlightGO.Highlight.SetActive(true);
                        _infoHighlightCards.Add(highlightGO);
                    }
                }
            }
            else if (!_inputActive)
            {
                DisableHighlightInfo();
            }
        }
        
        void DisableHighlightInfo()
        {
            foreach (var highlighted in _infoHighlightCards)
            {
                highlighted.Highlight.SetActive(false);
            }

            _infoHighlightCards = new List<CardGameObject>();
        }

        CardState[] GetImpactCards(CardState itemCardState, CardState fieldCardState)
        {
            DebugSystem.DebugLog($"Item {itemCardState.CardSO.Name}, impact on {fieldCardState.CardSO.Name}" +
                                 $" X:{fieldCardState.Position.x} Y:{fieldCardState.Position.y}",
                DebugSystem.Type.PlayerInput);

            //TODO Push PickUp
            int[,] attackArray = GetImpactMap<ImpactMaps>(itemCardState.CardSO.ImpactMap);

            return GetImpactedCards(itemCardState.CardSO.Name, fieldCardState.Position, attackArray);
        }

        CardState[] GetImpactCards(CardState fieldCardState)
        {
            DebugSystem.DebugLog($" X:{fieldCardState.Position.x} Y:{fieldCardState.Position.y}",
                DebugSystem.Type.PlayerInput);

            //TODO Push PickUp
            int[,] attackArray = GetImpactMap<ImpactMaps>(fieldCardState.CardSO.ImpactMap);

            return GetImpactedCards(fieldCardState.CardSO.Name, fieldCardState.Position, attackArray);
        }
    }
}