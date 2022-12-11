using System;
using UnityEngine;

namespace CardGrid
{
    [ExecuteInEditMode]
    public class GridGameObject : MonoBehaviour
    {
        public Transform ParentCards;
        public Vector3 SlotScale = Vector3.one;
        public Vector3 CommonOffset = new Vector3(-2.45f, 0, 2.5f);
        public float OffsetX = 0.2f;
        public float OffsetZ = 0.2f;
        public int SizeX = 6;
        public int SizeZ = 6;

        private void OnValidate()
        {
            var position = transform.position;
            int i = 0;
            float offsetX = 0;
            float offsetZ = 0;
            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    if(i >= transform.childCount) return;
                    var child = transform.GetChild(i);
                    child.gameObject.SetActive(true);
                    child.localScale = SlotScale;
                    child.transform.position =
                        (new Vector3(position.x + offsetX, position.y + offsetZ, position.z)
                        + CommonOffset) * transform.lossyScale.x;
                    i++;
                    offsetZ += OffsetZ;
                }
        
                offsetZ = 0;
                offsetX += OffsetX;
            }
            
            for (; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public Vector3 GetCellSpacePosition(Vector2 fieldPosition)
        {
            var pos = transform.position;
            return (new Vector3(pos.x + OffsetX * fieldPosition.x, pos.y + OffsetZ * fieldPosition.y, pos.z - 0.1f) 
                    + CommonOffset) * transform.lossyScale.x;
        }

        public Vector3 GetSpawnPosition(int x, int upOffset)
        {
            var pos = transform.position;
            return (new Vector3(pos.x + OffsetX * x, (pos.y + OffsetZ * upOffset) + pos.y + OffsetZ * -SizeZ, pos.z - 0.1f)
                    + CommonOffset)* transform.lossyScale.x;
        }
    }
}
