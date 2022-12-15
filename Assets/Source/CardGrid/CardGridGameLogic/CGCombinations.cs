using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace CardGrid
{
    /*
     * In this partial the logic reaction of cards on the field
     * to the effects of the player impact and other cards.
     */
    public partial class CardGridGame
    {
        delegate bool Checking(CardState card1, CardState card2);
        List<CardState> _cards = new List<CardState>(15);
        
        Queue<CardState> checkPool = new Queue<CardState>(6);
        IEnumerator CheckMach3()
        {
            _cards.Clear();
            //horizontal
            for (int z = BattleObjects.Field.SizeZ - 1; z >= 0; z--)
            {
                CheckHorizontal(CheckColor, z);
            }
            
            //vertical
            for (int x = 0; x < BattleObjects.Field.SizeX ; x++)
            {
                CheckVertical(CheckColor, x);
            }
            
            if(Accept(_cards))
            {
                BattleAudioSource.clip = ColorSound;
                BattleAudioSource.Play();
                
                yield return new WaitForSeconds(SpawnEffectOnCards(_cards.ToArray()));
                yield return ImpactDamageOnField(_cards.ToArray());
                _enemiesRecession = true;
            }
        }
            
        void CheckVertical(Checking check, int x)
        {
            checkPool.Clear();
            for (int z = BattleObjects.Field.SizeZ - 1; z >= 0; z--)
            {
                CheckCell(check, x, z, checkPool);
            }
            AddCardRange(checkPool.ToArray());
        }

        void CheckHorizontal(Checking check, int z)
        {
            checkPool.Clear();
            for (int x = BattleObjects.Field.SizeX - 1; x >= 0; x--)
            {
                CheckCell(check, x, z, checkPool);
            }
            AddCardRange(checkPool.ToArray());
        }

        void AddCardRange(CardState[] cards)
        {
            if (Accept(cards))
            {
                _cards.AddRange(cards);
                CombinationAchieve(cards);
            }
        }
        
        void CheckCell(Checking check, int x, int z, Queue<CardState> cards)
        {
            var cell = _CommonState.BattleState.Filed.Cells[x, z];
            if (cell.ScrObj == null || cell.Quantity <= 0 || cell.ScrObj.Type != TypeCard.Crystal)
            {
                AddCardRange(checkPool.ToArray());
                cards.Clear();
                return;
            }
                
            if (cards.Count > 0 && check(cards.Peek(), cell))
            {
                cards.Enqueue(cell);
            }
            else
            {
                AddCardRange(checkPool.ToArray());
                
                cards.Clear();
                cards.Enqueue(cell);
            }
        }

        bool CheckColor(CardState card1, CardState card2)
        {
            return card1.ScrObj.ColorType == card2.ScrObj.ColorType;
        }
            
        bool Accept(ICollection cards)
        {
            return (cards.Count > 2);
        }
    }
}