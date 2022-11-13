using UnityEngine;

namespace CardGrid
{
    public class GridGameObject : MonoBehaviour
    {
        public Vector3 CommonOffset = new Vector3(-2.45f, 0, 2.5f);
        public float OffsetX = 0.2f;
        public float OffsetZ = 0.2f;
        public int SizeX = 6;
        public int SizeZ = 6;

        public Vector3 CubeSize = new Vector3(1,1,1);    

        public Vector3 GetCellSpacePosition(Vector2 fieldPosition)
        {
            return (new Vector3(transform.position.x,transform.position.z,0) + CommonOffset + 
                   new Vector3(fieldPosition.x + fieldPosition.x * OffsetX, 0,
                       -(fieldPosition.y + fieldPosition.y * OffsetZ))) * transform.lossyScale.x;
        }

        public Vector3 GetSpawnPosition(int x, int upOffset)
        {
            return (new Vector3(transform.position.x,transform.position.z,0) + CommonOffset + 
                    new Vector3(x + x * OffsetX, 0,-(-3 + upOffset * OffsetZ))) * transform.lossyScale.x;
        }

        void OnDrawGizmos()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    Gizmos.DrawCube(GetCellSpacePosition(new Vector2(x,z)), CubeSize * transform.lossyScale.x);
                }
            }
        }
    }
}
