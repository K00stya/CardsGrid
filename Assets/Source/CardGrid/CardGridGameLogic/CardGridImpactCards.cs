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
    public partial class CardGridGame //ImpactCards
    {
        delegate bool Checking(CardState card1, CardState card2);
        List<CardState> _cards = new List<CardState>(15);
        
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
                
                yield return new WaitForSeconds(SpawnEffectOnCards(_cards.ToArray(), true));
                yield return ImpactDamageOnField(_cards.ToArray());
                _enemiesRecession = true;
            }
        }
            
        void CheckVertical(Checking check, int x)
        {
            Queue<CardState> cards = new Queue<CardState>(6);
            for (int z = BattleObjects.Field.SizeZ - 1; z >= 0; z--)
            {
                CheckCell(check, x, z, cards);
            }
            if (Accept(cards))
            {
                _cards.AddRange(cards.ToArray());
            }
        }

        void CheckHorizontal(Checking check, int z)
        {
            Queue<CardState> cards = new Queue<CardState>(6);
            for (int x = BattleObjects.Field.SizeX - 1; x >= 0; x--)
            {
                CheckCell(check, x, z, cards);
            }
            if (Accept(cards))
            {
                _cards.AddRange(cards.ToArray());
            }
        }
        
        void CheckCell(Checking check, int x, int z, Queue<CardState> cards)
        {
            var cell = _CommonState.BattleState.Filed.Cells[x, z];
            if (cell.CardSO == null || cell.Quantity <= 0 || cell.CardSO.Type != TypeCard.Enemy)
            {
                if (Accept(cards))
                {
                    _cards.AddRange(cards.ToArray());
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
                if (Accept(cards))
                {
                    _cards.AddRange(cards.ToArray());
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
            
        bool Accept(ICollection cards)
        {
            return (cards.Count > 2);
        }

        IEnumerator RotateRight()
        {
            if (_CommonState.BattleState.LevelID >= BattleState.CommonLevelID &&
                _CommonState.BattleState.CurrentTutorial != null && _CommonState.BattleState.CurrentTutorial.Count > 0)
            {
                var tutor = _CommonState.BattleState.CurrentTutorial.First();
                if (tutor.RotateRight)
                {
                    _CommonState.BattleState.CurrentTutorial.RemoveAt(0);
                }
                else
                {
                    yield break;
                }
            }
            
            _inputActive = false;
            float time = 1f;
            BattleObjects.FieldRotator.DORotate(
                BattleObjects.FieldRotator.eulerAngles + new Vector3(0, 90, 0), time);
            for (int i = 0; i < BattleObjects.Field.transform.childCount; i++)
            {
                var cell = BattleObjects.Field.transform.GetChild(i);
                cell.DOLocalRotate(cell.localRotation.eulerAngles + new Vector3(0, -90, 0), time);
            }

            var cellsMap = _CommonState.BattleState.Filed.Cells;
            var newCellsMap = new CardState[cellsMap.GetLength(0),cellsMap.GetLength(1)];
            for (int row = 0; row < cellsMap.GetLength(1); row++)
            {
                for (int col = 0; col < cellsMap.GetLength(0); col++)
                {
                    var newRow = col;
                    var newCol = cellsMap.GetLength(1) - (row + 1);
                    newCellsMap[newCol, newRow] = cellsMap[col, row];
                    newCellsMap[newCol, newRow].Position = new Vector2Int(newCol, newRow);
                }
            }

            _CommonState.BattleState.Filed.Cells = newCellsMap;

            yield return new WaitForSeconds(time);
            _inputActive = true;
        }

        IEnumerator RotateLeft()
        {
            if (_CommonState.BattleState.LevelID >= BattleState.CommonLevelID &&
                _CommonState.BattleState.CurrentTutorial != null && _CommonState.BattleState.CurrentTutorial.Count > 0)
            {
                var tutor = _CommonState.BattleState.CurrentTutorial.First();
                if (tutor.RotateLeft)
                {
                    _CommonState.BattleState.CurrentTutorial.RemoveAt(0);
                }
                else
                {
                    yield break;
                }
            }

            _inputActive = false;
            float time = 1f;
            BattleObjects.FieldRotator.DORotate(
                BattleObjects.FieldRotator.eulerAngles + new Vector3(0, -90, 0), time);
            for (int i = 0; i < BattleObjects.Field.transform.childCount; i++)
            {
                var cell = BattleObjects.Field.transform.GetChild(i);
                cell.DOLocalRotate(cell.localRotation.eulerAngles + new Vector3(0, 90, 0), time);
            }

            var cellsMap = _CommonState.BattleState.Filed.Cells;
            var newCellsMap = new CardState[cellsMap.GetLength(0),cellsMap.GetLength(1)];
            for (int row = 0; row < cellsMap.GetLength(1); row++)
            {
                for (int col = 0; col < cellsMap.GetLength(0); col++)
                {
                    var newRow = cellsMap.GetLength(0) - (col + 1);
                    var newCol = row;
                    newCellsMap[newCol, newRow] = cellsMap[col, row];
                    newCellsMap[newCol, newRow].Position = new Vector2Int(newCol, newRow);
                }
            }

            _CommonState.BattleState.Filed.Cells = newCellsMap;

            yield return new WaitForSeconds(time);
            _inputActive = true;
        }
    }
}