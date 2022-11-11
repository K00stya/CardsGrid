using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGrid
{
    public enum CT
    {
        Em,
        
        Ha,
        SH,
        SV,
        SLR,
        SRL,
        Sw,
        Bo,

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
        
        Bl,
    }

    public class Level
    {
        public Level(int group, TutorCardInfo[] tutor, (CT, int)[] inventory, (CT, int)[,] field,
            (ColorType, int)[] collectColors, bool spawnNewRandom = false, int[,] chainMap = null,
            bool quantity = false, bool rotation = false)
        {
            Group = group;
            Tutor = tutor;
            Inventory = inventory;
            Field = field;
            CollectColors = collectColors;
            NeedSpawnNewRandom = spawnNewRandom;
            ChainMap = chainMap;
            Quantity = quantity;
            Rotation = rotation;
        }

        public int Group;
        public TutorCardInfo[] Tutor;
        public (CT, int)[] Inventory;
        public (CT, int)[,] Field;
        public (ColorType, int)[] CollectColors; //max 3
        public bool NeedSpawnNewRandom;
        public int[,] ChainMap;
        public bool Quantity;
        public bool Rotation;
    }

    public static class LevelsMaps
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
                {
                    (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1),
                },

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P3, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.P3, 1),},
                    {(CT.Em, 1), (CT.P3, 1), (CT.P3, 1), (CT.Em, 1), (CT.R4, 1), (CT.R4, 1),},
                    {(CT.Em, 1), (CT.B1, 1), (CT.R4, 1), (CT.Em, 1), (CT.B1, 1), (CT.P3, 1),},
                    {(CT.B1, 1), (CT.G2, 1), (CT.B1, 1), (CT.P3, 1), (CT.B1, 1), (CT.P3, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 6),
                    (ColorType.Purple, 7)
                }
            ),

            new Level //1_2
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R4, 1), (CT.Em, 1), (CT.Em, 1), (CT.Y5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B1, 1), (CT.R4, 1), (CT.R4, 1), (CT.G2, 1), (CT.P3, 1), (CT.P3, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Red, 3),
                    (ColorType.Green, 3),
                    (ColorType.Purple, 3)
                }
            ),
            
            new Level //1_3
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.G2, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R4, 1), (CT.Em, 1), (CT.Em, 1), (CT.P3, 1), (CT.Em, 1), (CT.G2, 1),},
                    {(CT.G2, 1), (CT.Em, 1), (CT.Em, 1), (CT.R4, 1), (CT.Em, 1), (CT.G2, 1),},
                    {(CT.G2, 1), (CT.Em, 1), (CT.Em, 1), (CT.R4, 1), (CT.Em, 1), (CT.P3, 1),},
                    {(CT.Y5, 1), (CT.B1, 1), (CT.B1, 1), (CT.G2, 1), (CT.Em, 1), (CT.G2, 1),},
                    {(CT.B1, 1), (CT.Y5, 1), (CT.Y5, 1), (CT.R4, 1), (CT.P3, 1), (CT.P3, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 3),
                    (ColorType.Yellow, 3),
                    (ColorType.Green, 7),
                    (ColorType.Red, 3),
                }
            ),
            
            #endregion

            #region Levels4
            new Level //4_1
            (
                1,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(1, 5)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(5, 3)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(5, 5)
                    },
                },

                new (CT, int)[]
                    {(CT.SH, 1), (CT.SV, 2), (CT.SH, 2)},

                new (CT, int)[,]
                    {
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.R4, 1),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Y5, 1),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1)},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P3, 2),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 3), (CT.Y5, 2),},
                        {(CT.Em, 1), (CT.B1, 1), (CT.B1, 1), (CT.R4, 1), (CT.R4, 1), (CT.G2, 2),},
                        {(CT.R4, 1), (CT.G2, 1), (CT.P3, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1),},
                    },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 6),
                    (ColorType.Yellow, 3),
                    (ColorType.Red, 3),
                }
            ),
            
            new Level //4_3
            (
                1,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(1, 4)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(4, 4)
                    },
                },

                new (CT, int)[]
                    {(CT.SRL, 1), (CT.SLR, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1)},
                    {(CT.Em, 1), (CT.G2, 1), (CT.P3, 1), (CT.B1, 1), (CT.Y5, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.G2, 1), (CT.R4, 1), (CT.R4, 1), (CT.Y5, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 1), (CT.P3, 1), (CT.B1, 1), (CT.R4, 1), (CT.Em, 1),},
                    {(CT.R4, 1), (CT.G2, 1), (CT.P3, 1), (CT.B1, 1), (CT.Y5, 1), (CT.R4, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 3),
                    (ColorType.Yellow, 3),
                    (ColorType.Green, 3),
                    (ColorType.Red, 6),
                }
            ),
            new Level //4_4
            (
                1,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.SRL, 1), (CT.SLR, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.R4, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1) },
                    {(CT.B1, 1), (CT.G2, 1), (CT.Y5, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Y5, 1), (CT.G2, 1), (CT.R4, 1), (CT.B1, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B1, 1), (CT.G2, 1), (CT.R4, 1), (CT.G2, 1), (CT.B1, 1), (CT.Em, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 6),
                    (ColorType.Yellow, 3),
                    (ColorType.Green, 6),
                    (ColorType.Red, 3),
                }
            ),
            new Level //4_5
            (
                1,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(1, 4)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(4, 4)
                    },
                },

                new (CT, int)[]
                    {(CT.Sw, 1), (CT.Bo, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B1, 1), (CT.Em, 1), (CT.P3, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1)},
                    {(CT.B1, 1), (CT.G2, 1), (CT.P3, 1), (CT.B1, 1), (CT.Em, 1), (CT.G2, 1),},
                    {(CT.R4, 1), (CT.G2, 1), (CT.R4, 1), (CT.B1, 1), (CT.R4, 1), (CT.G2, 1),},
                    {(CT.B1, 1), (CT.R4, 1), (CT.P3, 1), (CT.R4, 1), (CT.Y5, 1), (CT.R4, 1),},
                    {(CT.R4, 1), (CT.G2, 1), (CT.R4, 1), (CT.B1, 1), (CT.R4, 1), (CT.G2, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 3),
                    (ColorType.Purple, 3),
                    (ColorType.Green, 6),
                    (ColorType.Red, 7),
                }
            ),
            #endregion
            
            #region Levels5
            new Level //5_3
            (
                1,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(2, 5)
                    },
                },

                new (CT, int)[]
                    {(CT.Ha, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Ha, 10), (CT.Em, 1), (CT.R4, 10), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1),},
                    {(CT.G2, 1), (CT.G2, 1), (CT.Y5, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 3),
                    (ColorType.Green, 3),
                }
            ),
            new Level //5_4
            (
                1,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1),(CT.Ha, 2),(CT.Ha,3),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.P3, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Y5, 6), (CT.Em, 1), (CT.Em, 1), (CT.Ha, 3), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 5), (CT.Ha, 6), (CT.Em, 1), (CT.R4, 2), (CT.Em, 1),},
                    {(CT.P3, 4), (CT.Y5, 1), (CT.G2, 1), (CT.R4, 2), (CT.P3, 9), (CT.R4, 1),},
                    
                    {(CT.Ha, 2), (CT.Em, 2), (CT.Em, 4), (CT.Ha, 8), (CT.Em, 1), (CT.Ha, 2),},
                    {(CT.B1, 4), (CT.R4, 10), (CT.G2, 8), (CT.Y5, 8), (CT.P3, 5), (CT.P3, 6),},
                    {(CT.R4, 2), (CT.R4, 7), (CT.B1, 2), (CT.Y5,5), (CT.R4, 1), (CT.P3, 6),},
                    {(CT.B1, 6), (CT.Y5, 3), (CT.G2, 5), (CT.B1, 2), (CT.P3, 4), (CT.Y5, 1),},
                    {(CT.B1, 1), (CT.Y5, 8), (CT.P3, 5), (CT.Y5, 8), (CT.P3, 4), (CT.P3, 9),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 3),
                    (ColorType.Yellow, 3),
                    (ColorType.Purple, 6),
                    (ColorType.Red, 6),
                }
            ),
            #endregion

            #region Levels6

            new Level //6_1
            (
                1,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]  {
                    (CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1),},
                    {(CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1),},
                    {(CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1),},
                    {(CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1),},
                    {(CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1), (CT.B1, 1), (CT.Y5, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 11),
                    (ColorType.Yellow, 11)
                },
                
                true
            ),
            #endregion
            
            new Level //Rotation
            (
                1,
                
                new TutorCardInfo[]
                {
                    new TutorCardInfo()
                    {
                        RotateLeft = true
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(4, 3)
                    },
                    new TutorCardInfo()
                    {
                        RotateRight = true
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = true,
                        ItemPosition = new Vector2Int(0, 0),
                        FieldPosition = new Vector2Int(0, 1)
                    },
                },

                new (CT, int)[]  {
                    (CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.R4, 1), (CT.P3, 1), (CT.G2, 1), (CT.B1, 1), (CT.R4, 1), (CT.Y5, 1),},
                    {(CT.B1, 1), (CT.Y5, 1), (CT.R4, 1), (CT.Y5, 1), (CT.B1, 1), (CT.R4, 1),},
                    {(CT.R4, 1), (CT.B1, 1), (CT.P3, 1), (CT.P3, 1), (CT.R4, 1), (CT.G2, 1),},
                    {(CT.R4, 1), (CT.P3, 1), (CT.R4, 1), (CT.G2, 1), (CT.Y5, 1), (CT.P3, 1),},
                    {(CT.P3, 1), (CT.B1, 1), (CT.G2, 1), (CT.B1, 1), (CT.B1, 1), (CT.R4, 1),},
                    {(CT.Y5, 1), (CT.Y5, 1), (CT.R4, 1), (CT.P3, 1), (CT.Y5, 1), (CT.P3, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 6),
                    (ColorType.Red, 6)
                },
                
                true,
                
                rotation:true
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

        public static (CT, int)[] Inventory =
        {
            (CT.Ha, 1),
        };

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