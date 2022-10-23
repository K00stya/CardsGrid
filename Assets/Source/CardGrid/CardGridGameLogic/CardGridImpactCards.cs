using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGrid
{
    /*
     * In this partial the logic reaction of cards on the field
     * to the effects of the player impact and other cards.
     */
    public partial class CardGridGame //ImpactCards
    {
        delegate bool Checking(CardState card1, CardState card2);
        Queue<CardState> cards = new Queue<CardState>(6);
        
        IEnumerator CheckMach3()
        {
            _enemiesRecession = false;

            //horizontal
            for (int z = BattleObjects.Field.SizeZ - 1; z >= 0; z--)
            {
                if (_enemiesRecession) yield break;

                CheckHorizontal(CheckColor, z, true);
                if(Accept())
                {
                    BattleAudioSource.clip = ColorSound;
                    BattleAudioSource.Play();
                
                    yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), true));
                    ImpactDamageOnField(cards.ToArray());
                    _enemiesRecession = true;
                
                    Debug.Log("Accept");
                    yield break;
                }
                
                if (_enemiesRecession) yield break;
                
                CheckHorizontal(CheckShape, z, false);
                if(Accept())
                {
                    BattleAudioSource.clip = ShapeSound;
                    BattleAudioSource.Play();
                
                    yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), false));
                    ImpactDamageOnField(cards.ToArray());
                    _enemiesRecession = true;
                
                    Debug.Log("Accept");
                    yield break;
                }
            }
            
            //vertical
            for (int x = 0; x < BattleObjects.Field.SizeX ; x++)
            {
                if (_enemiesRecession) yield break;
                
                CheckVertical(CheckColor, x, true);
                if(Accept())
                {
                    BattleAudioSource.clip = ColorSound;
                    BattleAudioSource.Play();
                
                    yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), true));
                    ImpactDamageOnField(cards.ToArray());
                    _enemiesRecession = true;
                
                    Debug.Log("Accept");
                    yield break;
                }
                
                if (_enemiesRecession) yield break;
                
                CheckVertical(CheckShape, x, false);
                if(Accept())
                {
                    BattleAudioSource.clip = ShapeSound;
                    BattleAudioSource.Play();
                
                    yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), false));
                    ImpactDamageOnField(cards.ToArray());
                    _enemiesRecession = true;
                
                    Debug.Log("Accept");
                    yield break;
                }
            }
        }
            
        void CheckVertical(Checking check, int x, bool color)
        {
            cards.Clear();
            for (int z = BattleObjects.Field.SizeZ - 1; z >= 0; z--)
            {
                if (_enemiesRecession) return;
                CheckCell(check, x, z, color);
            }
        }

        void CheckHorizontal(Checking check, int z, bool color)
        {
            cards.Clear();
            for (int x = BattleObjects.Field.SizeX - 1; x >= 0; x--)
            {
                if (_enemiesRecession) return;
                CheckCell(check, x, z, color);
            }
        }
        
        void CheckCell(Checking check, int x, int z, bool color)
        {
            Debug.Log("CheckCell");
            var cell = _CommonState.BattleState.Filed.Cells[x, z];
            if (cell.CardSO == null || cell.Quantity <= 0 || cell.CardSO.Type != TypeCard.Enemy)
            {
                if (Accept())
                {
                    _enemiesRecession = true;
                    return;
                }
                cards.Clear();
                return;
            }
                
            if (cards.Count > 0 && check(cards.Peek(), cell))
            {
                cards.Enqueue(cell);
            }
            else
            {
                if (Accept())
                {
                    _enemiesRecession = true;
                    return;
                }
                cards.Clear();
                cards.Enqueue(cell);
            }
        }

        bool CheckShape(CardState card1, CardState card2)
        {
            return card1.CardSO.Shape == card2.CardSO.Shape;
        }

        bool CheckColor(CardState card1, CardState card2)
        {
            return card1.CardSO.ColorType == card2.CardSO.ColorType;
        }
            
        bool Accept()
        {
            return (cards.Count > 2);
        }

        List<CardState> _reactOnImpact = new List<CardState>(25);
        /*
         * First, all the abilities of wounded opponents are activated, then the abilities of the dead.
         * This cycle will be repeated until everyone's abilities are activated.
         * If desired, this can be done in a different configuration.
         */
        IEnumerator ReactOnImpact(List<CardState> deaths, List<CardState> woundeds)
        {
            int debug = 0;
            do
            {
                if (DebugLoopCount()) yield break;
                do
                {
                    if (DebugLoopCount()) yield break;
                    yield return StartCoroutine(
                        ImpactWounded(woundeds.ToArray(),woundeds, deaths));
                    
                } while (woundeds.Count > 0);
                
                do
                {
                    if (DebugLoopCount()) yield break;
                    yield return StartCoroutine(
                        ImpactDead(deaths, woundeds, deaths));
                    
                } while (deaths.Count > 0);

            } while (deaths.Count > 0 || woundeds.Count > 0);

            bool DebugLoopCount()
            {
                debug++;
                if (debug > 300)
                {
                    DebugSystem.DebugLog("Infinity impact loop =(", DebugSystem.Type.Error);
                    return true;
                }

                return false;
            }
            
            _reactOnImpact.Clear();
        }
        
        IEnumerator ImpactWounded(CardState[] woundeds, List<CardState> newWoundeds, List<CardState> newDeaths)
        {
            foreach (var wounded in woundeds)
            {
                newWoundeds.Remove(wounded);
                if(_reactOnImpact.Contains(wounded)) continue;
                
                switch (wounded.CardSO.Name)
                {
                    case "Demons":
                        _reactOnImpact.Add(wounded);
                        int[,] attackArray = GetImpactMap<ImpactMaps>(wounded.CardSO.ImpactMap);
                        var cards = GetImpactedCards(wounded.CardSO.Name, wounded.Position, attackArray);
                        SpawnEffectOnCard(wounded);
                        //yield return new WaitForSeconds(SpawnEffectOnCards(wounded, cards));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField(wounded.Quantity, cards, ref newDeaths);
                        break;
                    case "Hare":
                        _reactOnImpact.Add(wounded);
                        ImpactDamageOnField(wounded.Quantity, new [] {wounded}, ref newDeaths);
                        yield return new WaitForSeconds(SpawnEffectOnCard(wounded));
                        break;
                    case "Warrior":
                        _reactOnImpact.Add(wounded);
                        ImpactDamageOnField(wounded.Quantity/2, new [] {wounded}, ref newDeaths);
                        break;
                }
            }
        }

        IEnumerator ImpactDead(List<CardState> deaths, List<CardState> newWoundeds, List<CardState> newDeaths)
        {
            foreach (var dead in deaths.ToArray())
            {
                newDeaths.Remove(dead);
                if(_reactOnImpact.Contains(dead)) continue;
                
                switch (dead.CardSO.Name)
                {
                    case "Ghost":
                        _reactOnImpact.Add(dead);
                        int[,] attackArray = GetImpactMap<ImpactMaps>(dead.CardSO.ImpactMap);
                        var cards = GetImpactedCards(dead.CardSO.Name, dead.Position, attackArray);
                        yield return new WaitForSeconds(SpawnEffectOnCard(dead));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField(dead.StartQuantity, cards, ref newDeaths);
                        break;
                    case "Librarian":
                        _reactOnImpact.Add(dead);
                        attackArray = GetImpactMap<ImpactMaps>(dead.CardSO.ImpactMap);
                        cards = GetImpactedCards(dead.CardSO.Name, dead.Position, attackArray);
                        yield return new WaitForSeconds(SpawnEffectOnCard(dead));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField( -dead.StartQuantity , cards, ref newDeaths);
                        break;
                }
            }
        }
    }
}