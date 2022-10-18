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

                yield return CheckHorizontal(CheckColor, z, true);
                yield return Accept(true);
                
                yield return CheckHorizontal(CheckShape, z, false);
                yield return Accept(false);
            }
            
            //vertical
            for (int x = 0; x < BattleObjects.Field.SizeX ; x++)
            {
                if (_enemiesRecession) yield break;
                yield return CheckVertical(CheckColor, x, true);
                yield return Accept(true);
                
                yield return CheckVertical(CheckShape, x, false);
                yield return Accept(false);
            }
        }
            
        IEnumerator CheckVertical(Checking check, int x, bool color)
        {
            cards.Clear();
            for (int z = BattleObjects.Field.SizeZ - 1; z >= 0; z--)
            {
                yield return CheckCell(check, x, z, color);
            }
        }

        IEnumerator CheckHorizontal(Checking check, int z, bool color)
        {
            cards.Clear();
            for (int x = BattleObjects.Field.SizeX - 1; x >= 0; x--)
            {
                yield return CheckCell(check, x, z, color);
            }
        }
        
        IEnumerator CheckCell(Checking check, int x, int z, bool color)
        {
            if (_enemiesRecession) yield break;
            var cell = _CommonState.BattleState.Filed.Cells[x, z];
            if (cell.CardSO == null || cell.Quantity <= 0 || cell.CardSO.Type != TypeCard.Enemy)
            {
                yield return Accept(color);
                cards.Clear();
                yield break;
            }
                
            if (cards.Count > 0 && check(cards.Peek(), cell))
            {
                cards.Enqueue(cell);
            }
            else
            {
                yield return Accept(color);
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
            
        IEnumerator Accept(bool color)
        {
            if (cards.Count > 2)
            {
                yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), color));
                ImpactDamageOnField(cards.ToArray());
                _enemiesRecession = true;
                yield break;
            }
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