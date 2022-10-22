using UnityEngine;

namespace CardGrid
{
    public enum CT
    {
        Em,
        
        Hammer,
        SwordHorizontal,
        SwordVertical,
        SwordLeft_Right,
        SwordRight_Left,
        Swords,
        Bomb,

        G1Blue,
        G1Green,
        G1Purple,
        G1Red,
        G1Yellow,

        G2Blue,
        G2Green,
        G2Purple,
        G2Red,
        G2Yellow,

        G3Blue,
        G3Green,
        G3Purple,
        G3Red,
        G3Yellow,

        G4Blue,
        G4Green,
        G4Purple,
        G4Red,
        G4Yellow,

        G5Blue,
        G5Green,
        G5Purple,
        G5Red,
        G5Yellow,
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
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), },

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2Purple, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G1Blue, 1), (CT.G2Purple, 1),},
                    {(CT.Em, 1), (CT.G3Green, 1), (CT.G3Green, 1), (CT.Em, 1), (CT.G2Red, 1), (CT.G5Red, 1),},
                    {(CT.Em, 1), (CT.G1Blue, 1), (CT.G5Red, 1), (CT.Em, 1), (CT.G1Blue, 1), (CT.G2Purple, 1),},
                    {(CT.G1Blue, 1), (CT.G2Red, 1), (CT.G1Blue, 1), (CT.G3Green, 1), (CT.G1Blue, 1), (CT.G2Purple, 1),},
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

    public static class LevelSample
    {
        public static TutorCardInfo[] Tutor =
        {
            new TutorCardInfo()
            {
                AnyItem = true,
                ItemPosition = new Vector2Int(0, 0),
                FieldPosition = new Vector2Int(0, 0)
            }
        };

        public static (CT, int)[] Inventory = {(CT.Hammer, 1)};

        public static (CT, int)[,] Field =
        {
            {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
            {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
            {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
            {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
            {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
            {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
        };
    }
}