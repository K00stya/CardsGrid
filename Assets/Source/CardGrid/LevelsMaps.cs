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
        Ch
    }

    public class Level
    {
        public Level(int group, TutorCardInfo[] tutor, (CT, int)[] inventory, (CT, int)[,] field,
            (ColorType, int)[] collectColors, bool spawnNewRandom = false, int[,] chainMap = null)
        {
            Group = group;
            Tutor = tutor;
            Inventory = inventory;
            Field = field;
            CollectColors = collectColors;
            NeedSpawnNewRandom = spawnNewRandom;
            ChainMap = chainMap;
        }

        public int Group;
        public TutorCardInfo[] Tutor;
        public (CT, int)[] Inventory;
        public (CT, int)[,] Field;
        public (ColorType, int)[] CollectColors; //max 3
        public (ShapeType, int)[] CollectShape; 
        public bool NeedSpawnNewRandom;
        public int[,] ChainMap;
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
                {
                    (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1),
                },

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P2, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.P2, 1),},
                    {(CT.Em, 1), (CT.G3, 1), (CT.G3, 1), (CT.Em, 1), (CT.R2, 1), (CT.R5, 1),},
                    {(CT.Em, 1), (CT.B1, 1), (CT.R5, 1), (CT.Em, 1), (CT.B1, 1), (CT.P2, 1),},
                    {(CT.B1, 1), (CT.R2, 1), (CT.B1, 1), (CT.G3, 1), (CT.B1, 1), (CT.P2, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),

            new Level //1_2
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.Em, 1), (CT.Em, 1), (CT.Y3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B2, 1), (CT.R1, 1), (CT.R1, 1), (CT.G2, 1), (CT.P5, 1), (CT.P5, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            
            new Level //1_3
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.G1, 1), (CT.Em, 1), (CT.Em, 1), (CT.B3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.Em, 1), (CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.G1, 1),},
                    {(CT.G2, 1), (CT.Em, 1), (CT.Em, 1), (CT.R2, 1), (CT.Em, 1), (CT.G1, 1),},
                    {(CT.G3, 1), (CT.Em, 1), (CT.Em, 1), (CT.R2, 1), (CT.Em, 1), (CT.P2, 1),},
                    {(CT.Y5, 1), (CT.B4, 1), (CT.B4, 1), (CT.G3, 1), (CT.Em, 1), (CT.G1, 1),},
                    {(CT.B2, 1), (CT.Y5, 1), (CT.Y5, 1), (CT.R2, 1), (CT.P5, 1), (CT.P5, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            
            new Level //1_4
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P4, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.G1, 1), (CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R1, 1), (CT.Em, 1), (CT.G2, 1), (CT.B5, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.G2, 1), (CT.B5, 1), (CT.G2, 1), (CT.R1, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.G3, 1), (CT.P4, 1), (CT.Y3, 1), (CT.P4, 1), (CT.Em, 1),},
                    {(CT.B2, 1), (CT.R1, 1), (CT.R1, 1), (CT.G2, 1), (CT.P5, 1), (CT.P5, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            
            new Level //1_5
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.B3, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.P2, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 1), (CT.Y3, 1), (CT.R2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B2, 1), (CT.Y1, 1), (CT.Y3, 1), (CT.G5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.G5, 1), (CT.R4, 1), (CT.R2, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R1, 1), (CT.R4, 1), (CT.Y3, 1), (CT.B4, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.G5, 1), (CT.P2, 1), (CT.Y5, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.G5, 1), (CT.P2, 1), (CT.B3, 1), (CT.G2, 1), (CT.R3, 1), (CT.R2, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //1_6
            (
                1,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Y2, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 1), (CT.Em, 1), (CT.Em, 1), (CT.R4, 1), (CT.Em, 1),},
                    {(CT.P5, 1), (CT.Y1, 1), (CT.G3, 1), (CT.R4, 1), (CT.P3, 1), (CT.R4, 1),},
                    {(CT.B3, 1), (CT.R4, 1), (CT.G3, 1), (CT.Y2, 1), (CT.P3, 1), (CT.P3, 1),},
                    {(CT.R1, 1), (CT.R4, 1), (CT.B4, 1), (CT.Y2, 1), (CT.R5, 1), (CT.P3, 1),},
                    {(CT.B3, 1), (CT.Y2, 1), (CT.G3, 1), (CT.B4, 1), (CT.P3, 1), (CT.Y1, 1),},
                    {(CT.B3, 1), (CT.Y2, 1), (CT.P5, 1), (CT.Y2, 1), (CT.P3, 1), (CT.P3, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
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
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G5, 1),},
                    {(CT.Y1, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P5, 1),},
                    {(CT.R5, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 1),},
                    {(CT.G1, 1), (CT.Em, 1), (CT.Em, 1), (CT.P3, 1), (CT.Em, 1), (CT.R5, 1),},
                    {(CT.B1, 1), (CT.Y3, 1), (CT.G3, 1), (CT.R1, 1), (CT.Em, 1), (CT.Y5, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //2_2
            (
                2,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B5, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Y1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.R2, 1), (CT.Em, 1), (CT.G5, 1), (CT.Em, 1),},
                    {(CT.G2, 1), (CT.P2, 1), (CT.Y5, 1), (CT.B1, 1), (CT.P5, 1), (CT.G1, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //2_3
            (
                2,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1),(CT.Ha, 1),},

                new (CT, int)[,]
                {
                    {(CT.G5, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Y4, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P1, 1), (CT.Em, 1),},
                    {(CT.B2, 1), (CT.Em, 1), (CT.B5, 1), (CT.Em, 1), (CT.B5, 1), (CT.Em, 1),},
                    {(CT.G4, 1), (CT.Em, 1), (CT.Y1, 1), (CT.Em, 1), (CT.R1, 1), (CT.Em, 1),},
                    {(CT.R4, 1), (CT.G5, 1), (CT.R2, 1), (CT.Em, 1), (CT.G5, 1), (CT.Em, 1),},
                    {(CT.G2, 1), (CT.P2, 1), (CT.Y5, 1), (CT.B1, 1), (CT.P5, 1), (CT.G1, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
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
                        AnyItem = false,
                        ItemPosition = new Vector2Int(1, 0),
                        FieldPosition = new Vector2Int(2, 5)
                    },
                    new TutorCardInfo()
                    {
                        AnyItem = false,
                        ItemPosition = new Vector2Int(1, 0),
                        FieldPosition = new Vector2Int(4, 3)
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
                        FieldPosition = new Vector2Int(5, 3)
                    },
                },

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 2), (CT.Ha, 3), (CT.Ha, 2)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G1, 1), (CT.Y5, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 3), (CT.R3, 3),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1), (CT.G4, 1), (CT.P5, 1),},
                    {(CT.B1, 1), (CT.B1, 1), (CT.G1, 2), (CT.Em, 1), (CT.G1, 1), (CT.B5, 2),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //3_2
            (
                3,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1), (CT.Ha, 2), (CT.Ha, 3),(CT.Ha, 2),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P1, 1), (CT.Y5, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G4, 1), (CT.R3, 3),},
                    {(CT.Em, 1), (CT.G5, 1), (CT.Em, 1), (CT.Em, 1), (CT.B1, 4), (CT.P5, 1),},
                    {(CT.B5, 1), (CT.B1, 3), (CT.Y5, 1), (CT.Em, 1), (CT.G1, 1), (CT.B5, 2),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //3_3
            (
                3,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 2), (CT.Ha, 1), (CT.Ha, 2),(CT.Ha, 3),(CT.Ha, 2),(CT.Ha, 3),(CT.Ha, 1),},

                new (CT, int)[,]
                {
                    {(CT.G5, 3), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Y4, 10), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P1, 7), (CT.Em, 1),},
                    {(CT.B2, 4), (CT.Em, 1), (CT.B5, 7), (CT.Em, 1), (CT.B5, 3), (CT.Em, 1),},
                    {(CT.G4, 5), (CT.Em, 1), (CT.Y1, 1), (CT.Em, 1), (CT.R1, 1), (CT.Em, 1),},
                    {(CT.R4, 6), (CT.G5, 4), (CT.R2, 3), (CT.Em, 1), (CT.G5, 6), (CT.Em, 1),},
                    {(CT.G2, 4), (CT.P2, 8), (CT.Y5, 3), (CT.B1, 4), (CT.P5, 4), (CT.G1, 7),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //3_4
            (
                3,

                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 3), (CT.Ha, 4), (CT.Ha, 2),(CT.Ha, 1),(CT.Ha, 3),
                        (CT.Ha, 8),(CT.Ha,3),(CT.Ha, 2),(CT.Ha, 3),(CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.P5, 3), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Y2, 4), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 5), (CT.Em, 1), (CT.Em, 1), (CT.R4, 5), (CT.Em, 1),},
                    {(CT.P5, 4), (CT.Y1, 7), (CT.G3, 6), (CT.B4, 5), (CT.P3, 5), (CT.R4, 9),},
                    {(CT.B3, 7), (CT.R4, 4), (CT.G3, 10), (CT.Y2, 10), (CT.P3, 3), (CT.P3, 3),},
                    {(CT.R1, 3), (CT.R4, 7), (CT.B4, 3), (CT.Y2, 3), (CT.R5, 4), (CT.P3, 6),},
                    {(CT.B3, 5), (CT.Y2, 5), (CT.G3, 7), (CT.B4, 2), (CT.P3, 8), (CT.Y1, 4),},
                    {(CT.B5, 1), (CT.Y2, 1), (CT.P5, 3), (CT.Y2, 7), (CT.P3, 3), (CT.P3, 7),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),

            #endregion
            
            #region Levels4
            new Level //4_1
            (
                4,
                
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
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.R1, 1),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Y1, 1),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B5, 1),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B5, 1)},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.P4, 2),},
                        {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 3), (CT.Y1, 2),},
                        {(CT.Em, 1), (CT.B4, 1), (CT.B4, 1), (CT.R1, 1), (CT.R1, 1), (CT.G2, 2),},
                        {(CT.R1, 1), (CT.G2, 1), (CT.P3, 1), (CT.B4, 1), (CT.Y2, 1), (CT.B5, 1),},
                    },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //4_2
            (
                4,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.SH, 1), (CT.SV, 5), (CT.SV, 2)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1)},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.R1, 1), (CT.Em, 2),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.B3, 3), (CT.Y1, 2),},
                    {(CT.Em, 1), (CT.G5, 1), (CT.Y5, 1), (CT.Em, 1), (CT.R4, 1), (CT.P5, 2),},
                    {(CT.R3, 1), (CT.B1, 1), (CT.R4, 1), (CT.G5, 1), (CT.G1, 1), (CT.R2, 2),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //4_3
            (
                4,
                
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
                    {(CT.Em, 1), (CT.G2, 1), (CT.P3, 1), (CT.B4, 1), (CT.Y1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.G2, 1), (CT.R5, 1), (CT.R5, 1), (CT.Y1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R5, 1), (CT.P3, 1), (CT.B4, 1), (CT.R5, 1), (CT.Em, 1),},
                    {(CT.R5, 1), (CT.G2, 1), (CT.P3, 1), (CT.B4, 1), (CT.Y1, 1), (CT.R5, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //4_4
            (
                4,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.SRL, 1), (CT.SLR, 1),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.R4, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1)},
                    {(CT.G5, 1), (CT.G3, 1), (CT.Y1, 1), (CT.G5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.P5, 1), (CT.Y1, 1), (CT.B2, 1), (CT.G4, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Y1, 1), (CT.G1, 1), (CT.P4, 1), (CT.B5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B5, 1), (CT.G1, 1), (CT.R4, 1), (CT.G3, 1), (CT.B2, 1), (CT.Em, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //4_5
            (
                4,
                
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
                    {(CT.B1, 1), (CT.G2, 1), (CT.P3, 1), (CT.B4, 1), (CT.Em, 1), (CT.G4, 1),},
                    {(CT.R5, 1), (CT.G2, 1), (CT.R5, 1), (CT.B4, 1), (CT.R5, 1), (CT.G4, 1),},
                    {(CT.B1, 1), (CT.R5, 1), (CT.P3, 1), (CT.R5, 1), (CT.Y3, 1), (CT.R5, 1),},
                    {(CT.R5, 1), (CT.G2, 1), (CT.R5, 1), (CT.B4, 1), (CT.R5, 1), (CT.G4, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            #endregion
            
            #region Levels5
            new Level //5_1
            (
                5,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1),(CT.SH, 1), (CT.SLR, 1), (CT.Sw, 1)},

                new (CT, int)[,]
                {
                    {(CT.P4, 1), (CT.G2, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 2),},
                    {(CT.R5, 1), (CT.G2, 1), (CT.Y1, 1), (CT.Em, 1), (CT.Y1, 1), (CT.Em, 2),},
                    {(CT.P4, 1), (CT.R5, 1), (CT.R2, 1), (CT.Y1, 1), (CT.B5, 1), (CT.B5, 2),},
                    {(CT.P4, 1), (CT.G2, 1), (CT.Y1, 1), (CT.B5, 1), (CT.Y1, 1), (CT.G1, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //5_2
            (
                5,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                {
                    (CT.SLR, 3), (CT.Bo, 3), (CT.SRL, 3), (CT.SH, 3), 
                    (CT.Ha, 3), (CT.Sw, 3), (CT.SV, 3)
                },

                new (CT, int)[,]
                {
                    {(CT.R5, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R5, 1), (CT.Em, 1), (CT.R4, 1), (CT.G3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.G1, 3), (CT.Y4, 1), (CT.G1, 3), (CT.Y5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R2, 2), (CT.G1, 3), (CT.B3, 3), (CT.B3, 3), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.B3, 2), (CT.R5, 2), (CT.G1, 2), (CT.Y5, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.R1, 2), (CT.Y4, 1), (CT.R4, 1), (CT.G1, 2), (CT.P3, 1), (CT.Em, 1),},
                    {(CT.R5, 1), (CT.Y4, 1), (CT.R4, 1), (CT.Y5, 1), (CT.G1, 2), (CT.Y3, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //5_3
            (
                5,
                
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
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.G1, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Ha, 10), (CT.Em, 1), (CT.R2, 10), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.G2, 1), (CT.Em, 1), (CT.Y1, 1), (CT.Em, 1),},
                    {(CT.G2, 1), (CT.G2, 1), (CT.Y3, 1), (CT.Em, 1), (CT.B1, 1), (CT.Em, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //5_4
            (
                5,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]
                    {(CT.Ha, 1),(CT.Ha, 2),(CT.Ha,3),},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.P5, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Y2, 6), (CT.Em, 1), (CT.Em, 1), (CT.SH, 3), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 5), (CT.Bo, 6), (CT.Em, 1), (CT.R4, 2), (CT.Em, 1),},
                    {(CT.P5, 4), (CT.Y1, 1), (CT.G3, 1), (CT.R4, 2), (CT.P3, 9), (CT.R4, 1),},
                    
                    {(CT.SRL, 2), (CT.Em, 2), (CT.Em, 4), (CT.Ha, 8), (CT.Em, 1), (CT.SLR, 2),},
                    {(CT.B3, 4), (CT.R4, 10), (CT.G3, 8), (CT.Y2, 8), (CT.P3, 5), (CT.P3, 6),},
                    {(CT.R1, 2), (CT.R4, 7), (CT.B4, 2), (CT.Y2,5), (CT.R5, 1), (CT.P3, 6),},
                    {(CT.B3, 6), (CT.Y2, 3), (CT.G3, 5), (CT.B4, 2), (CT.P3, 4), (CT.Y1, 1),},
                    {(CT.B3, 1), (CT.Y2, 8), (CT.P5, 5), (CT.Y2, 8), (CT.P3, 4), (CT.P3, 9),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            new Level //5_5
            (
                5,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]  {
                    (CT.Ha, 7),(CT.Bo, 6),(CT.SLR, 2), (CT.SH, 5),(CT.Ha, 10),(CT.SRL, 2),(CT.Sw, 8),(CT.SV, 6)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.R1, 10), (CT.R1, 7), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Y4, 4), (CT.Y3, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.P3, 9), (CT.R1, 2), (CT.R1, 4), (CT.P3, 5), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.P2, 2), (CT.R1, 8), (CT.R1, 1), (CT.P2, 8), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.R4, 5), (CT.B2, 1), (CT.B2, 4), (CT.R3, 5), (CT.Em, 1),},
                    {(CT.R5, 1), (CT.P1, 8), (CT.B2, 3), (CT.B2, 6), (CT.P1, 3), (CT.R5, 1),},
                    {(CT.G4, 6), (CT.R5, 1), (CT.Y3, 5), (CT.Y3, 4), (CT.R5, 1), (CT.G3, 10),},
                    {(CT.Bl, 1), (CT.Y3, 7), (CT.R5, 1), (CT.R5, 1), (CT.Y3, 7), (CT.Bl, 1),},
                    {(CT.Bl, 1), (CT.Bl, 1), (CT.B2, 2), (CT.B2, 8), (CT.Bl, 1), (CT.Bl, 1),},
                    {(CT.Bl, 1), (CT.Bl, 1), (CT.Bl, 1), (CT.Bl, 1), (CT.Bl, 1), (CT.Bl, 1),},
                },
                
                Array.Empty<(ColorType, int)>()
            ),
            
            #endregion

            #region Levels6

            new Level //6_1
            (
                6,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]  {
                    (CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1),},
                    {(CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1), (CT.B1, 1),},
                    {(CT.B1, 1), (CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1),},
                    {(CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1), (CT.B1, 1),},
                    {(CT.B1, 1), (CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1), (CT.B1, 1), (CT.Y2, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Blue, 11),
                    (ColorType.Yellow, 11)
                },
                
                true
            ),
            new Level //6_2
            (
                6,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]  {
                    (CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Y4, 1), (CT.R2, 1), (CT.R2, 1), (CT.Y4, 1), (CT.B1, 1),},
                    {(CT.Em, 1), (CT.R5, 1), (CT.Y3, 1), (CT.Y3, 1), (CT.R5, 1), (CT.Em, 1),},
                    {(CT.Bl, 1), (CT.Em, 1), (CT.R5, 1), (CT.R5, 1), (CT.Em, 1), (CT.Bl, 1),},
                    {(CT.Bl, 1), (CT.Bl, 1), (CT.Y3, 1), (CT.Y3, 1), (CT.Bl, 1), (CT.Bl, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Yellow, 5),
                    (ColorType.Red, 5)
                },
                
                true
            ),
            
            new Level //6_2
            (
                6,
                
                Array.Empty<TutorCardInfo>(),

                new (CT, int)[]  {
                    (CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1),(CT.Ha, 1)},

                new (CT, int)[,]
                {
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1), (CT.Em, 1),},
                    {(CT.Em, 1), (CT.Y4, 1), (CT.R2, 1), (CT.R2, 1), (CT.Y4, 1), (CT.B1, 1),},
                    {(CT.Em, 1), (CT.R5, 1), (CT.Y3, 1), (CT.Y3, 1), (CT.R5, 1), (CT.Em, 1),},
                    {(CT.Bl, 1), (CT.Em, 1), (CT.R5, 1), (CT.R5, 1), (CT.Em, 1), (CT.Bl, 1),},
                },
                
                new (ColorType, int)[]
                {
                    (ColorType.Yellow, 5),
                    (ColorType.Red, 5)
                },
                
                true,
                
                new [,]
                {
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {1, 2, 1, 1, 2, 1},
                    {1, 1, 2, 2, 1, 1},
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