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
                
                switch (wounded.Name)
                {
                    case "Demons":
                        _reactOnImpact.Add(wounded);
                        int[,] attackArray = GetImpactMap<ImpactMaps>(wounded.ImpactMap);
                        var cards = GetImpactedCards(wounded.Name, wounded.Position, attackArray);
                        SpawnEffectOnCard(wounded);
                        yield return new WaitForSeconds(SpawnEffectOnCards(wounded, cards));
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
                
                switch (dead.Name)
                {
                    case "Ghost":
                        _reactOnImpact.Add(dead);
                        int[,] attackArray = GetImpactMap<ImpactMaps>(dead.ImpactMap);
                        var cards = GetImpactedCards(dead.Name, dead.Position, attackArray);
                        yield return new WaitForSeconds(SpawnEffectOnCard(dead));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField(dead.StartQuantity, cards, ref newDeaths);
                        break;
                    case "Librarian":
                        _reactOnImpact.Add(dead);
                        attackArray = GetImpactMap<ImpactMaps>(dead.ImpactMap);
                        cards = GetImpactedCards(dead.Name, dead.Position, attackArray);
                        yield return new WaitForSeconds(SpawnEffectOnCard(dead));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField( -dead.StartQuantity , cards, ref newDeaths);
                        break;
                }
            }
        }
    }
}