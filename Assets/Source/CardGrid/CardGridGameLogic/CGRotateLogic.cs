using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace CardGrid
{
    public partial class CardGridGame
    {
        private List<GameObject> slots; 
            
        IEnumerator MoveRow(int row, bool right)
        {
            float time = 0.7f;
            var fieldSlots = BattleObjects.Field;
            Vector2 startPos = default;
            Transform nextSlot;
            int starRow = row;
            Debug.Log(row);
            for (int i = 0; i < fieldSlots.SizeX; i++)
            {
                var slot = fieldSlots.transform.GetChild(row);
                if (i == 0)
                {
                    startPos = BattleObjects.Field.GetCellSpacePosition(new Vector2(-1, row));
                }
                
                if (row + BattleObjects.Field.SizeZ <= fieldSlots.transform.childCount &&
                    fieldSlots.transform.GetChild(row + BattleObjects.Field.SizeZ).gameObject.activeSelf)
                    nextSlot = fieldSlots.transform.GetChild(row + BattleObjects.Field.SizeZ);
                else
                    nextSlot = null;

                if (nextSlot != null)
                    slot.transform.DOMove(nextSlot.position, time);
                else
                {
                    slot.transform.position = startPos;
                    slot.transform.DOMove((Vector2)BattleObjects.Field.GetCellSpacePosition(
                        new Vector2(0, starRow)), time);
                }
                
                row += BattleObjects.Field.SizeZ;
            }
            yield return new WaitForSeconds(time);
        }
        
        IEnumerator RotateRight()
        {
            if (_CommonState.BattleState.CurrentTutorial != null && _CommonState.BattleState.CurrentTutorial.Count > 0)
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
            float time = 0.7f;
            BattleObjects.FieldRotator.DOLocalRotate(
                BattleObjects.FieldRotator.localEulerAngles + new Vector3(0, 90, 0), time);
            for (int i = 0; i < BattleObjects.Field.ParentCards.childCount; i++)
            {
                var cell = BattleObjects.Field.ParentCards.GetChild(i);
                cell.DOLocalRotate(cell.localEulerAngles + new Vector3(0, -90, 0), time);
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
            yield return TryGetNewItemsForField(_CommonState.BattleState.Filed.Cells,
                _CommonState.BattleState.Inventory.Items);
            yield return UpdateFields();
            _inputActive = true;
        }

        IEnumerator RotateLeft()
        {
            if (_CommonState.BattleState.CurrentTutorial != null && _CommonState.BattleState.CurrentTutorial.Count > 0)
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
            float time = 0.7f;
            BattleObjects.FieldRotator.DOLocalRotate(
                BattleObjects.FieldRotator.localEulerAngles + new Vector3(0, -90, 0), time);
            for (int i = 0; i < BattleObjects.Field.ParentCards.childCount; i++)
            {
                var cell = BattleObjects.Field.ParentCards.GetChild(i);
                cell.DOLocalRotate(cell.localEulerAngles + new Vector3(0, 90, 0), time);
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
            yield return TryGetNewItemsForField(_CommonState.BattleState.Filed.Cells,
                _CommonState.BattleState.Inventory.Items);
            yield return UpdateFields();
            _inputActive = true;
        }
    }
}