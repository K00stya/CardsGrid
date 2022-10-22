using UnityEngine;

namespace CardGrid
{
    public enum CT
    {
        Em,
        
        Hammer,

        G1Blue,
        G1Green,
        G1Red,

        G2Blue,
        G2Green,
        G2Red
    }

    public class Level
    {
        public Level(int group, TutorCardInfo[] tutor, (CT, int)[] inventory, (CT, int)[,] field)
        {
            Group = group;
            Tutor = tutor;
            Inventory = inventory;
            Field = field;
        }

        public int Group;
        public TutorCardInfo[] Tutor;
        public (CT, int)[] Inventory;
        public (CT, int)[,] Field;
    }

    public static class LevelsMaps
    {
        public static Level[] Levels =
        {
            new Level
            (
                0,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(2, 5)
                    }
                },

                new (CT, int)[] 
                    {(CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.G1Red, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.G1Red, 0), (CT.G2Blue, 0), (CT.G1Red, 0), (CT.Em, 0), (CT.Em, 0)},
                }
            ),
            
            new Level
            (
                1,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(2, 5)
                    }
                },

                new (CT, int)[] 
                    {(CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.Em, 0), (CT.G1Red, 0), (CT.G2Green, 0), (CT.Em, 0), (CT.Em, 0)},
                    {(CT.Em, 0), (CT.G1Red, 0), (CT.G2Blue, 0), (CT.G1Red, 0), (CT.G2Green, 0), (CT.G2Green, 0)},
                }
            ),
        };
    }

    public static class Level5_2
    {
        public static TutorCardInfo[] Tutor =
        {
            new TutorCardInfo()
            {
                ItemPosition = new Vector2Int(0, 0),
                FieldPosition = new Vector2Int(2, 5)
            }
        };

        public static (CT, int)[] Inventory = {(CT.Hammer, 1)};

        public static (CT, int)[,] Field =
        {
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.G1Red, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.G1Red, 0), (CT.G2Blue, 0), (CT.G1Red, 0), (CT.Em, 0), (CT.Em, 0)},
        };
    }


    public static class Level5_3
    {
        public static TutorCardInfo[] Tutor =
        {
            new TutorCardInfo()
            {
                ItemPosition = new Vector2Int(0, 0),
                FieldPosition = new Vector2Int(2, 5)
            }
        };

        public static (CT, int)[,] Field =
        {
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.Em, 0), (CT.G1Red, 0), (CT.G2Green, 0), (CT.Em, 0), (CT.Em, 0)},
            {(CT.Em, 0), (CT.G1Red, 0), (CT.G2Blue, 0), (CT.G1Red, 0), (CT.G2Green, 0), (CT.G2Green, 0)},
        };
    }
}