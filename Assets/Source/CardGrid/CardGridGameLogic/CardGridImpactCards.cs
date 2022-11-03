using System.Collections;
using System.Collections.Generic;
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
        Queue<CardState> cards = new Queue<CardState>(6);
        
        IEnumerator CheckMach3()
        {
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
                    yield break;
                }
                
                if(!WithShape) continue;
                if (_enemiesRecession) yield break;
                
                CheckHorizontal(CheckShape, z, false);
                if(Accept())
                {
                    BattleAudioSource.clip = ShapeSound;
                    BattleAudioSource.Play();
                
                    yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), false));
                    ImpactDamageOnField(cards.ToArray());
                    _enemiesRecession = true;
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
                    yield break;
                }
                
                if(!WithShape) continue;
                if (_enemiesRecession) yield break;
                
                CheckVertical(CheckShape, x, false);
                if(Accept())
                {
                    BattleAudioSource.clip = ShapeSound;
                    BattleAudioSource.Play();
                
                    yield return new WaitForSeconds(SpawnEffectOnCards(cards.Peek(), cards.ToArray(), false));
                    ImpactDamageOnField(cards.ToArray());
                    _enemiesRecession = true;
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

        IEnumerator RotateRight()
        {
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