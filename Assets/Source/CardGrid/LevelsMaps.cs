using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGrid
{
    public enum CT
    {
        Empty,
        Block,
        
        Blue,
        Green,
        Purple,
        Red,
        Yellow,
        
        Hammer,
        SwordHor,
        SwordVer,
        SwordL_R,
        SwordR_L,
        Swords,
        Bomb,
        Boots,
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

    public static class LevelsMapsDeprecade
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
                    (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1),
                },

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Purple, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Blue, 1), (CT.Purple, 1),},
                    {(CT.Empty, 1), (CT.Purple, 1), (CT.Purple, 1), (CT.Empty, 1), (CT.Red, 1), (CT.Red, 1),},
                    {(CT.Empty, 1), (CT.Blue, 1), (CT.Red, 1), (CT.Empty, 1), (CT.Blue, 1), (CT.Purple, 1),},
                    {(CT.Blue, 1), (CT.Green, 1), (CT.Blue, 1), (CT.Purple, 1), (CT.Blue, 1), (CT.Purple, 1),},
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
                    {(CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Purple, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Red, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Yellow, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Blue, 1), (CT.Red, 1), (CT.Red, 1), (CT.Green, 1), (CT.Purple, 1), (CT.Purple, 1),},
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
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Blue, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Red, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Purple, 1), (CT.Empty, 1), (CT.Green, 1),},
                    {(CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Red, 1), (CT.Empty, 1), (CT.Green, 1),},
                    {(CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Red, 1), (CT.Empty, 1), (CT.Purple, 1),},
                    {(CT.Yellow, 1), (CT.Blue, 1), (CT.Blue, 1), (CT.Green, 1), (CT.Empty, 1), (CT.Green, 1),},
                    {(CT.Blue, 1), (CT.Yellow, 1), (CT.Yellow, 1), (CT.Red, 1), (CT.Purple, 1), (CT.Purple, 1),},
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
                    {(CT.SwordHor, 1), (CT.SwordVer, 2), (CT.SwordHor, 2)},

                new (CT, int)[,]
                    {
                        {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Red, 1),},
                        {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Yellow, 1),},
                        {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Blue, 1),},
                        {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Blue, 1)},
                        {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Purple, 2),},
                        {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 3), (CT.Yellow, 2),},
                        {(CT.Empty, 1), (CT.Blue, 1), (CT.Blue, 1), (CT.Red, 1), (CT.Red, 1), (CT.Green, 2),},
                        {(CT.Red, 1), (CT.Green, 1), (CT.Purple, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1),},
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
                    {(CT.SwordR_L, 1), (CT.SwordL_R, 1),},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1)},
                    {(CT.Empty, 1), (CT.Green, 1), (CT.Purple, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Green, 1), (CT.Red, 1), (CT.Red, 1), (CT.Yellow, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Red, 1), (CT.Purple, 1), (CT.Blue, 1), (CT.Red, 1), (CT.Empty, 1),},
                    {(CT.Red, 1), (CT.Green, 1), (CT.Purple, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Red, 1),},
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
                    {(CT.SwordR_L, 1), (CT.SwordL_R, 1),},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Red, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1) },
                    {(CT.Blue, 1), (CT.Green, 1), (CT.Yellow, 1), (CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Green, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Yellow, 1), (CT.Green, 1), (CT.Red, 1), (CT.Blue, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Blue, 1), (CT.Green, 1), (CT.Red, 1), (CT.Green, 1), (CT.Blue, 1), (CT.Empty, 1),},
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
                    {(CT.Swords, 1), (CT.Bomb, 1),},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Blue, 1), (CT.Empty, 1), (CT.Purple, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1)},
                    {(CT.Blue, 1), (CT.Green, 1), (CT.Purple, 1), (CT.Blue, 1), (CT.Empty, 1), (CT.Green, 1),},
                    {(CT.Red, 1), (CT.Green, 1), (CT.Red, 1), (CT.Blue, 1), (CT.Red, 1), (CT.Green, 1),},
                    {(CT.Blue, 1), (CT.Red, 1), (CT.Purple, 1), (CT.Red, 1), (CT.Yellow, 1), (CT.Red, 1),},
                    {(CT.Red, 1), (CT.Green, 1), (CT.Red, 1), (CT.Blue, 1), (CT.Red, 1), (CT.Green, 1),},
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
                    {(CT.Hammer, 1),},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Blue, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Hammer, 10), (CT.Empty, 1), (CT.Red, 10), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Green, 1), (CT.Empty, 1), (CT.Blue, 1), (CT.Empty, 1),},
                    {(CT.Green, 1), (CT.Green, 1), (CT.Yellow, 1), (CT.Empty, 1), (CT.Blue, 1), (CT.Empty, 1),},
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
                    {(CT.Hammer, 1),(CT.Hammer, 2),(CT.Hammer,3),},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Purple, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Yellow, 6), (CT.Empty, 1), (CT.Empty, 1), (CT.Hammer, 3), (CT.Empty, 1),},
                    {(CT.Empty, 1), (CT.Red, 5), (CT.Hammer, 6), (CT.Empty, 1), (CT.Red, 2), (CT.Empty, 1),},
                    {(CT.Purple, 4), (CT.Yellow, 1), (CT.Green, 1), (CT.Red, 2), (CT.Purple, 9), (CT.Red, 1),},
                    
                    {(CT.Hammer, 2), (CT.Empty, 2), (CT.Empty, 4), (CT.Hammer, 8), (CT.Empty, 1), (CT.Hammer, 2),},
                    {(CT.Blue, 4), (CT.Red, 10), (CT.Green, 8), (CT.Yellow, 8), (CT.Purple, 5), (CT.Purple, 6),},
                    {(CT.Red, 2), (CT.Red, 7), (CT.Blue, 2), (CT.Yellow,5), (CT.Red, 1), (CT.Purple, 6),},
                    {(CT.Blue, 6), (CT.Yellow, 3), (CT.Green, 5), (CT.Blue, 2), (CT.Purple, 4), (CT.Yellow, 1),},
                    {(CT.Blue, 1), (CT.Yellow, 8), (CT.Purple, 5), (CT.Yellow, 8), (CT.Purple, 4), (CT.Purple, 9),},
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
                    (CT.Hammer, 1),(CT.Hammer, 1),(CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
                    {(CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1), (CT.Hammer, 1),},
                    {(CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1),},
                    {(CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1),},
                    {(CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1),},
                    {(CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Yellow, 1),},
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
                    (CT.Hammer, 1),(CT.Hammer, 1),(CT.Hammer, 1),(CT.Hammer, 1),(CT.Hammer, 1),(CT.Hammer, 1)},

                new (CT, int)[,]
                {
                    {(CT.Red, 1), (CT.Purple, 1), (CT.Green, 1), (CT.Blue, 1), (CT.Red, 1), (CT.Yellow, 1),},
                    {(CT.Blue, 1), (CT.Yellow, 1), (CT.Red, 1), (CT.Yellow, 1), (CT.Blue, 1), (CT.Red, 1),},
                    {(CT.Red, 1), (CT.Blue, 1), (CT.Purple, 1), (CT.Purple, 1), (CT.Red, 1), (CT.Green, 1),},
                    {(CT.Red, 1), (CT.Purple, 1), (CT.Red, 1), (CT.Green, 1), (CT.Yellow, 1), (CT.Purple, 1),},
                    {(CT.Purple, 1), (CT.Blue, 1), (CT.Green, 1), (CT.Blue, 1), (CT.Blue, 1), (CT.Red, 1),},
                    {(CT.Yellow, 1), (CT.Yellow, 1), (CT.Red, 1), (CT.Purple, 1), (CT.Yellow, 1), (CT.Purple, 1),},
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
            (CT.Hammer, 1),
        };

        public static (CT, int)[,] Field =
        {
            {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
            {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
            {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
            {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
            {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
            {(CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1), (CT.Empty, 1),},
        };
    }
}