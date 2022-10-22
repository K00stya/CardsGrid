using System;
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

        B1,
        G1,
        P1,
        R1,
        Y1,

        B2,
        G2,
        P2,
        R2,
        Y2,

        B3,
        G3,
        P3,
        R3,
        Y3,

        B4,
        G4,
        P4,
        R4,
        Y4,

        B5,
        G5,
        P5,
        R5,
        Y5,
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

    public static partial class LevelsMaps
    {
        public static Level[] Levels =
        {
            #region Levels1
            new Level //1_1
            (
                1,

                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(1, 5)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(2, 5)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(4, 3)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(5, 3)
                    },
                },

                new (CT, int)[]
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P2, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.P2, 1),},
                    {(CT.Em, 1), (CT.G3, 1), (CT.G3, 1), (CT.Em, 1), (CT.R2, 1), (CT.R5, 1),},
                    {(CT.Em, 1), (CT.B1, 1), (CT.R5, 1), (CT.Em, 1), (CT.B1, 1), (CT.P2, 1),},
                    {(CT.B1, 1), (CT.R2, 1), (CT.B1, 1), (CT.G3, 1), (CT.B1, 1), (CT.P2, 1),},
                }
            ),

            new Level //1_2
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.Em, 1), (CT.Em, 1), (CT.Y3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B2, 1), (CT.R1, 1), (CT.R1, 1), (CT.G2, 1), (CT.P5, 1), (CT.P5, 1),},
                }
            ),
            
            new Level //1_3
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.G1, 1), (CT.Em, 1), (CT.Em, 1), (CT.B3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.Em, 1), (CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.G1, 1),},
                    {(CT.G2, 1), (CT.Em, 1), (CT.Em, 1), (CT.R2, 1), (CT.Em, 1), (CT.G1, 1),},
                    {(CT.G3, 1), (CT.Em, 1), (CT.Em, 1), (CT.R2, 1), (CT.Em, 1), (CT.P2, 1),},
                    {(CT.Y5, 1), (CT.B4, 1), (CT.B4, 1), (CT.G3, 1), (CT.Em, 1), (CT.G1, 1),},
                    {(CT.B2, 1), (CT.Y5, 1), (CT.Y5, 1), (CT.R2, 1), (CT.P5, 1), (CT.P5, 1),},
                }
            ),
            
            new Level //1_4
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P4, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.G1, 1), (CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R1, 1), (CT.Em, 1), (CT.G2, 1), (CT.B5, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.G2, 1), (CT.B5, 1), (CT.G2, 1), (CT.R1, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.G3, 1), (CT.P4, 1), (CT.Y3, 1), (CT.P4, 1), (CT.Em, 1),},
                    {(CT.B2, 1), (CT.R1, 1), (CT.R1, 1), (CT.G2, 1), (CT.P5, 1), (CT.P5, 1),},
                }
            ),
            #endregion

            #region Levels2
            new Level //2_1
            (
                2,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(0, 3)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(3, 5)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(5, 3)
                    },
                },

                new (CT, int)[]
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G5, 1),},
                    {(CT.Y1, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P5, 1),},
                    {(CT.R5, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1),},
                    {(CT.G1, 1), (CT.Em, 1), (CT.Em, 1), (CT.P3, 1), (CT.Em, 1), (CT.R5, 1),},
                    {(CT.B1, 1), (CT.Y3, 1), (CT.G3, 1), (CT.R1, 1), (CT.Em, 1), (CT.Y5, 1),},
                }
            ),
            #endregion

            #region Levels3
            new Level //3_1
            (
                3,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(0, 3)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(3, 5)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(5, 3)
                    },
                },

                new (CT, int)[]
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G1, 1), (CT.Y5, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.R3, 1),},
                    {(CT.G1, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1), (CT.G4, 1), (CT.P5, 1),},
                    {(CT.B1, 1), (CT.B1, 1), (CT.G1, 1), (CT.Em, 1), (CT.G1, 1), (CT.B5, 1),},
                }
            ),
            

            #endregion
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