using System.Collections;
using UnityEngine;

namespace CardGrid
{
    /*
     * In this partial, the logic of the inventory.
     */
    public partial class CardGridGame
    {
        /*
        * Instead of removing an object from the inventory and creating a new one on the field,
        * I'll just send the item from the field to the inventory, and the extra item(disabled) to the field
        *
        * Repeats as long as there are items in the bottom line.
        * With a lack of resources, string comparison can also be avoided,
        * for example, by the state of the card state (InInventory or OnFiled)
        */
        IEnumerator TryGetNewItemsForField(CardState[,] cells, CardState[,] items)
        {
            bool newItems = false;
            int lowerZ = cells.GetLength(1) - 1;
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                var card = cells[x, lowerZ];
                
                if (!card.CardSO || card.CardSO.Type != TypeCard.Item) continue;

                newItems = true;
                _itemsRecession = true;
                yield return MoveInventory(x);

                card.Grid = CardGrid.Inventory;
                card.Position = new Vector2Int(0, 0);
                items[0, 0] = card;
                yield return MoveCardToSelfPosition(items[0, 0], BattleObjects.Inventory);
                break;
            }

            if (!newItems)
            {
                _itemsRecession = false;
                foreach (var card in items)
                {
                    yield return MoveCardToSelfPosition(card, BattleObjects.Inventory);
                }

                yield return new WaitForSeconds(SpeedRecession);
            }

            /*
             * When a new item is added, all cards move to the right.
             * If the row has ended, it moves to the bottom.
             * If the row is already lower, then the item is excess and the empty card is removed to the field.
             */
            IEnumerator MoveInventory(int currentX)
            {
                for (int z = items.GetLength(1) - 1; z >= 0; z--)
                {
                    for (int x = items.GetLength(0) - 1; x >= 0; x--)
                    {
                        int newX = x + 1;
                        int newZ = z + 1;
                        if (newX < items.GetLength(0))
                        {
                            items[newX, z] = items[x, z];
                            items[newX, z].Position = new Vector2Int(newX, z);
                            yield return MoveCardToSelfPosition(items[newX, z], BattleObjects.Inventory);
                        }
                        else if (newZ < items.GetLength(1))
                        {
                            items[0, newZ] = items[x, z];
                            items[0, newZ].Position = new Vector2Int(0, newZ);
                            yield return MoveCardToSelfPosition(items[0, newZ], BattleObjects.Inventory);
                        }
                        else
                        {
                            var excessItem = items[x, z];
                            excessItem.Grid = CardGrid.Field;
                            excessItem.Quantity = 0;
                            excessItem.GameObject.gameObject.SetActive(false);
                            excessItem.Position = new Vector2Int(currentX, lowerZ);
                            cells[currentX, lowerZ] = excessItem;
                        }
                    }
                }
            }
        }
        
        /*
         * When the item is spent, all cards move to the left.
         * If the row has ended, it moves to the top one.
         * If the row is already top, then the object is not moving anywhere.
         */
        void RecessionInventory(CardState[,] items)
        {
            for (int z = 0; z < items.GetLength(1); z++)
            {
                for (int x = 0; x < items.GetLength(0); x++)
                {
                    if (items[x, z].Quantity <= 0)
                    {
                        int newX = x + 1;
                        int newZ = z + 1;
                        if (newX < items.GetLength(0))
                        {
                            SwapPositions(items, items[x, z].Position, items[newX, z].Position);
                        }
                        else if (newZ < items.GetLength(1))
                        {
                            SwapPositions(items, items[x, z].Position, items[0, newZ].Position);
                        }
                    }
                }
            }
        }
        
        IEnumerator ItemsCombinations(CardState[,] items)
        {
            yield return null;
        }
    }
}