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
        /*
         * First, all the abilities of wounded opponents are activated, then the abilities of the dead.
         * This cycle will be repeated until everyone's abilities are activated.
         * If desired, this can be done in a different configuration.
         */
        IEnumerator ReactOnImpact(List<Card> deaths, List<Card> woundeds)
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
        }
        
        IEnumerator ImpactWounded(Card[] woundeds, List<Card> newWoundeds, List<Card> newDeaths)
        {
            foreach (var wounded in woundeds)
            {
                switch (wounded.name)
                {
                    case "Demons":
                        int[,] attackArray = GetImpactMap<ImpactMaps>(wounded.ImpactMap);
                        var cards = GetImpactedCards(wounded.name, wounded.Position, attackArray);
                        SpawnEffectOnCard(wounded);
                        yield return new WaitForSeconds(SpawnEffectOnCards(wounded, cards));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField(1, cards, ref newDeaths);
                        break;
                }

                newWoundeds.Remove(wounded);
            }
        }

        IEnumerator ImpactDead(List<Card> deaths, List<Card> newWoundeds, List<Card> newDeaths)
        {
            foreach (var dead in deaths.ToArray())
            {
                switch (dead.name)
                {
                    case "Ghost":
                        int[,] attackArray = GetImpactMap<ImpactMaps>(dead.ImpactMap);
                        var cards = GetImpactedCards(dead.name, dead.Position, attackArray);
                        yield return new WaitForSeconds(SpawnEffectOnCard(dead));
                        newWoundeds.AddRange(cards);
                        ImpactDamageOnField(dead.StartQuantity, cards, ref newDeaths);
                        break;
                }

                newDeaths.Remove(dead);
            }
        }
    }
}